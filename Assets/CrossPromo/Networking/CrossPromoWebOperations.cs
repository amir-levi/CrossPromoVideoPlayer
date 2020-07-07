using System;
using System.Collections.Generic;
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
        public static async Task<List<VideoPlayerListItem>> FetchVideoPlaylist(string url, Action<string> onFail)
        {
            var playList = new List<VideoPlayerListItem>();
            try
            {
                var client = new HttpClient();
                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var jsonObject = JsonUtility.FromJson<RootObject> (jsonString);
                    playList = jsonObject.results.Select(result => new VideoPlayerListItem {Id = result.id, ClickUrl = result.click_url, TrackingUrl = result.tracking_url, VideoUrl = result.video_url}).ToList();
                }
                else
                {
                    var message = $"{(int) response.StatusCode}: {response.StatusCode}";
                    onFail(message);
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
                var client = new HttpClient();
                var uri = new Uri(trackingUrl).SetQueryVal("pid", instanceId.ToString());
                var content = new StringContent("",Encoding.UTF8);
                var response = await client.PostAsync(uri, content);
                    
                if (response.IsSuccessStatusCode)
                {
                    Debug.Log($"Status Code: {response.StatusCode} {response.Headers}");
                    success = true;
                }
                else
                {
                    var message = $"{(int) response.StatusCode}: {response.StatusCode}";
                    onFail(message);
                }
            }
            catch (Exception e)
            {
                onFail(e.Message);
            }
            return success;
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