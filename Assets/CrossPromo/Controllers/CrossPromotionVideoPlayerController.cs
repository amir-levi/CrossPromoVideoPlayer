using System;
using CrossPromo.Networking;
using CrossPromo.Views;
using UnityEngine;

namespace CrossPromo.Controllers
{
    public class CrossPromotionVideoPlayerController
    {
        private readonly CrossPromotionVideoPlayerView _videoPlayerView;
        
        public CrossPromotionVideoPlayerController()
        {
            _videoPlayerView = GameObject.FindGameObjectWithTag("CrossPromotionVideoPlayer").GetComponent<CrossPromotionVideoPlayerView>();
            _videoPlayerView.VideoClicked = (instanceId, track) =>
            {
                var trackingUrl = track.TrackingUrl;
                var clickUrl = track.ClickUrl;
            
                Debug.Log($"instanceId: {instanceId} trackingUrl: {trackingUrl}  clickUrl: {clickUrl} ");
            };
        }

       


        public async void CrossPromotionVideoPlayerView(Type videoPlayerType,string serverUrl)
        {
            var tracks = await CrossPromotionVideoPlayerWebRequest.FetchVideoTracksAsync(serverUrl);
            _videoPlayerView.Init(videoPlayerType,tracks);
        }

    
     
    }
}