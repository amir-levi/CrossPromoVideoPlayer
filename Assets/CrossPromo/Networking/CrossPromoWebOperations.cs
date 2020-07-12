using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CrossPromo.Extensions;
using CrossPromo.Models;
using UnityEngine;

namespace CrossPromo.Networking
{
    public static class CrossPromoWebOperations
    {
        
        private static HttpClientHandler _clientHandler = new HttpClientHandler();
        public static async Task<List<CrossPromoVideoInfo>> FetchVideoPlaylist(string url, Action<string> onFail)
        {
            var playList = new List<CrossPromoVideoInfo>();
            try
            {
               using (HttpClient client = new HttpClient(_clientHandler, false))
               {
                   var response = await client.GetAsync(url);

                   if (response.IsSuccessStatusCode)
                   {
                       var jsonString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                       var jsonObject = JsonUtility.FromJson<RootObject> (jsonString);
                       var responseList = jsonObject.results.Select(result => new CrossPromoVideoInfo {Id = result.id, ClickUrl = result.click_url, TrackingUrl = result.tracking_url, VideoUrl = result.video_url}).ToList();
                       foreach (var item in responseList)
                       {
                           item.LocalPath = await DownloadVideo(item.VideoUrl);
                           if (!string.IsNullOrEmpty(item.LocalPath))
                           {
                               playList.Add(item);
                           }
                       }
                   }
                   else
                   {
                       var message = $"{(int) response.StatusCode}: {response.StatusCode}";
                       onFail(message);
                   }
               }
             
            }
            catch (Exception e)
            {
                onFail(e.Message);
            }
            
            
            return playList;
        }
        
        
        
        

        public static async Task<bool> SendRequest(string trackingUrl,int instanceId, Action<object> onFail)
        {
            var success = false;
            try
            {
                using (HttpClient client = new HttpClient(_clientHandler,false ))
                {
                    var uri = new Uri(trackingUrl).SetQueryVal("pid", instanceId.ToString());
                    var content = new StringContent("",Encoding.UTF8);
                    var response = await client.PostAsync(uri, content);
                    
                    if (response.IsSuccessStatusCode)
                    {
                        success = true;
                    }
                    else
                    {
                        var message = $"{(int) response.StatusCode}: {response.StatusCode}";
                        onFail(message);
                    }
                }
                
                
              
            }
            catch (Exception e)
            {
                onFail(e.Message);
            }
            return success;
        }


        public static async Task<string> DownloadVideo(string url)
        {
            if (url == null)
                throw new ArgumentNullException(nameof(url)); 
            
            
            var localPath = "";
            try
            {
                var path = Path.Combine(Application.persistentDataPath, Path.GetFileName(url));
                using (var client = new HttpClient(_clientHandler, false))
                {

                    using (var request = new HttpRequestMessage(HttpMethod.Get, url))
                    {
                        using (Stream contentStream = await (await client.SendAsync(request)).Content.ReadAsStreamAsync(),
                            stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 4096,true ))
                        {
                            await contentStream.CopyToAsync(stream);
                            localPath = path;
                        } 
                    }
                 
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
            

            return localPath;
        }
        
        
        private class RootObject
        {
            [Serializable]
            public class Result
            {
                public int id;
                public string video_url;
                public string click_url;
                public string tracking_url;
            }

            public Result[] results;
        }
       
    }
}