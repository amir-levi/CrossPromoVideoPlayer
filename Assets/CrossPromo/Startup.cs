using CrossPromo.Controllers;
using CrossPromo.VideoPlayer.Players;
using UnityEngine;

namespace CrossPromo
{
    public class Startup : MonoBehaviour
    {
        [SerializeField] private string ServerUrl;
        
        private void Start() 
        {
            new CrossPromotionVideoPlayerController().CrossPromotionVideoPlayerView(typeof(UnityVideoPlayer),ServerUrl);
           // new MockCrossPromoController().CrossPromotionVideoPlayerView(typeof(UnityVideoPlayer),ServerUrl, MockCrossPromoController.TrackOptions.One);
        }
        
    }
}