using System;
using System.Collections.Generic;
using CrossPromo.Models;
using CrossPromo.VideoPlayer.Actions;
using UnityEngine;

namespace CrossPromo.VideoPlayer.Players
{
    [Serializable]
    public class CrossPromoVideoPlayer : IVideoPlayer,IVideoClickedAction
    {
        private VideoPlayerScreen _screen;
        private VideoPlayerTray _videoPlayerTray;

        private CrossPromoVideoTrack _currentTrack;
        private CrossPromoVideoTrack _previousTrack;
        

        public Action<int> OnVideoClicked { get; set; }

        public CrossPromoVideoPlayer(List<CrossPromoVideoInfo> videoPlayerTracks, VideoPlayerScreen screen, Transform transform)
        {
            if (videoPlayerTracks == null || videoPlayerTracks.Count <= 0)
            {
                Debug.LogError("No Video Player Tracks - Check Data fetch from the server");
                return;
            }
            
            _screen = screen;
            _screen.OnClick = () =>
            {
                OnVideoClicked?.Invoke(_videoPlayerTray.GetCurrentTrack().Id);
            };
            _videoPlayerTray = new VideoPlayerTray(videoPlayerTracks,transform);
            _currentTrack = _videoPlayerTray.GetCurrentTrack();
            RunTrack();
        }
        
        private void RunTrack()
        {
            
            _currentTrack.OnTrackPrepared = () =>
            {
                _currentTrack.Play(_screen);
            };
            
            _currentTrack.OnTrackFinish = Next;
            _currentTrack.Prepare();
            
          
        }

        public void Next()
        {
            _previousTrack = _currentTrack;
            _videoPlayerTray.Rotate(-1);
            _currentTrack = _videoPlayerTray.GetCurrentTrack();
            
            if(_previousTrack != _currentTrack)
                _previousTrack.Stop();
            
            RunTrack(); 
        }

        public void Previous()
        {
            _previousTrack = _currentTrack;
            _videoPlayerTray.Rotate(1);
            _currentTrack = _videoPlayerTray.GetCurrentTrack();

            if(_previousTrack != _currentTrack)
                _previousTrack.Stop();
            
            RunTrack();
            
        }

        public void Pause()
        {
          _videoPlayerTray.GetCurrentTrack().Pause();
        }

        public void Resume()
        {
           _videoPlayerTray.GetCurrentTrack().Resume();
        }

        public bool IsPlaying()
        {
           return _videoPlayerTray.GetCurrentTrack().IsPlaying();
        }

      
    }
}