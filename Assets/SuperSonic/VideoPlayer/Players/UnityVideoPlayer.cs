using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace SuperSonic.VideoPlayer.Players
{
    public class UnityVideoPlayer : IVideoPlayer
    {
       private UnityEngine.Video.VideoPlayer _player;

        private List<UnityEngine.Video.VideoClip> _clips = new List<VideoClip>();
        protected int CurrentTrackIndex = -1;

        public UnityVideoPlayer(List<IVideoPlayerTrack> videoPlayerTracks)
        {
            _player = Camera.main.gameObject.AddComponent<UnityEngine.Video.VideoPlayer>();
        }
        
        public void Next()
        {
            CurrentTrackIndex++;
            _player.clip = _clips[CurrentTrackIndex];
            _player.Play();
        }

        public void Previous()
        {
            CurrentTrackIndex--;
            _player.clip = _clips[CurrentTrackIndex];
            _player.Play();
        }

        public void Pause()
        {
            if(_player.isPlaying)
                _player.Pause();
        }

        public void Resume()
        {
            if(_player.isPaused)
                _player.Play();
        }
    }
}