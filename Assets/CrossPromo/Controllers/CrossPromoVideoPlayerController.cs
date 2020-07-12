using System;
using CrossPromo.Networking;
using CrossPromo.Views;
using UnityEngine;

namespace CrossPromo.Controllers
{
    public class CrossPromoVideoPlayerController
    {
        private readonly CrossPromotionVideoPlayerView _videoPlayerView;
        
        public CrossPromoVideoPlayerController()
        {
            _videoPlayerView = GameObject.FindGameObjectWithTag("CrossPromotionVideoPlayer").GetComponent<CrossPromotionVideoPlayerView>();
            _videoPlayerView.VideoClicked = (instanceId, track) =>
            {
                var trackingUrl = track.TrackingUrl;
                var clickUrl = track.ClickUrl;
                
                SendCrossPromoRequest(instanceId,trackingUrl,clickUrl);
            };
        }


        private async void SendCrossPromoRequest(int instanceId, string trackingUrl,  string clickUrl)
        {
            var success = await CrossPromoWebOperations.SendRequest(trackingUrl,instanceId,
            message =>
            {
                Debug.LogError(message);
            });
            
            
            if(success)
                Application.OpenURL(clickUrl);
        }

        public async void CrossPromotionVideoPlayerView(Type videoPlayerType)
        {

           var playList = await CrossPromoWebOperations.FetchVideoPlaylist(_videoPlayerView.ServerUrl,
            message =>
            {
                Debug.LogError(message);
            });
            
           _videoPlayerView.Init(playList);
        }

    
     
    }
}