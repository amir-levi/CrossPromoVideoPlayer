using System;
using System.Threading.Tasks;
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

        public async void CrossPromotionVideoPlayerView(Type videoPlayerType,string serverUrl)
        {

           var playList = await CrossPromoWebOperations.FetchVideoPlaylist(serverUrl,
            message =>
            {
                Debug.LogError(message);
            });
            
           _videoPlayerView.Init(videoPlayerType,playList);
        }

    
     
    }
}