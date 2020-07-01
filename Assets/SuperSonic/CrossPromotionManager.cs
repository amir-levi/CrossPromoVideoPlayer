using System;
using SuperSonic.VideoPlayer;
using SuperSonic.VideoPlayer.Players;
using UnityEngine;

namespace SuperSonic
{
    public class CrossPromotionManager : MonoBehaviour
    {
        [SerializeField] private string ServerUrl;
        [SerializeField] private CrossPromotionVideoPlayer VideoPlayer;
        
        private void Start()
        {
            IVideoPlayer player = new UnityVideoPlayer(null);
            VideoPlayer.Init(player, null);;

        }
    }
}