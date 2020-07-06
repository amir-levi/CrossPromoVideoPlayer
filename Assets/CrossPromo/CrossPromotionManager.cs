using CrossPromo.Controllers;
using CrossPromo.VideoPlayer.Players;
using UnityEngine;

namespace CrossPromo
{
    public class CrossPromotionManager : MonoBehaviour
    {
        [SerializeField] private string ServerUrl;
        
        private void Start()
        {
            new CrossPromotionVideoPlayerController().CrossPromotionVideoPlayerView(typeof(UnityVideoPlayer),ServerUrl);
        }
        
    }
}