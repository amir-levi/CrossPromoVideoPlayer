using System;
using System.Collections.Generic;
using UnityEngine;

namespace SuperSonic.VideoPlayer
{
    
    public class CrossPromotionVideoPlayer : MonoBehaviour
    {

        [SerializeField] private int InstanceId;
        [SerializeField] private bool Loop;

        private IVideoPlayer _videoPlayer;
        private int _currentTrackIndex ;
        public List<IVideoPlayerTrack> VideoPlayerTracks = new List<IVideoPlayerTrack>();

        public void Init(IVideoPlayer videoPlayer, List<IVideoPlayerTrack> videoPlayerTracks)
        {
            _videoPlayer = videoPlayer;
        }


        public void Play()
        {
            _videoPlayer.Next();
        }


        private void Next()
        {
           
            
        }

        private void Previous()
        {
            _videoPlayer.Previous();
        }

        private void Pause()
        {
            _videoPlayer.Pause();
        }

        private void Resume()
        {
            _videoPlayer.Resume();
        }
    }
}