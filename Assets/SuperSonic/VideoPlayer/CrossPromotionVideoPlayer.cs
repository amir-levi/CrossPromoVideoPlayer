using System;
using System.Collections.Generic;
using UnityEngine;

namespace SuperSonic.VideoPlayer
{
    
    public class CrossPromotionVideoPlayer : MonoBehaviour, IVideoPlayer
    
    {

        [SerializeField] private int InstanceId;
        [SerializeField] private bool Loop;
        
        
        public List<IVideoPlayerTrack> VideoPlayerTracks = new List<IVideoPlayerTrack>();

        public void Init(List<IVideoPlayerTrack> videoPlayerTracks)
        {
            
        }



        public void Next()
        {
            Debug.Log("Play Next Track");
        }

        public void Previous()
        {
            Debug.Log("Play Previous Track");
        }

        public void Pause()
        {
            Debug.Log("Pause Track");
        }

        public void Resume()
        {
            Debug.Log("Resume Track");
        }
    }
}