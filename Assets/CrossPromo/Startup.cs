using CrossPromo.Controllers;
using CrossPromo.VideoPlayer.Players;
using UnityEngine;

namespace CrossPromo
{
    public class Startup : MonoBehaviour
    {
      
        
        private void Start() 
        {
            new CrossPromoVideoPlayerController().CrossPromotionVideoPlayerView(typeof(CrossPromoVideoPlayer));
        }
        
    }
}