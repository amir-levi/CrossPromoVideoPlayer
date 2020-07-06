using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CrossPromo.Models;
using UnityEngine;

namespace CrossPromo.Networking
{
    public static class CrossPromotionVideoPlayerWebRequest
    {
        
        public static async Task<List<CrossPromotionVideoPlayerTrack>> FetchVideoTracksAsync(string url)
        {
            var client = new HttpClient();
            var jsonString = await client.GetStringAsync(url).ConfigureAwait(false);
            var jsonObject = JsonUtility.FromJson<RootObject> (jsonString);
            return jsonObject.results.Select(result => new CrossPromotionVideoPlayerTrack {Id = result.id, ClickUrl = result.click_url, TrackingUrl = result.tracking_url, VideoUrl = result.video_url}).ToList();
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