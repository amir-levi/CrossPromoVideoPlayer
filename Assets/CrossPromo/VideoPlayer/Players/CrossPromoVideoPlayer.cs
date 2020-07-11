using System;
using System.Collections.Generic;
using CrossPromo.Models;
using CrossPromo.VideoPlayer.Actions;
using UnityEngine;

namespace CrossPromo.VideoPlayer.Players
{
    [Serializable]
    public class CrossPromoVideoPlayer : IVideoPlayer,IVideoClickedAction,IVideoTrackPreparedAction
    {
        private VideoPlayerScreen _screen;
        [SerializeField]private VideoPlayerTray _videoPlayerTray;

        public Action OnNextVideoTrackReady { get; set; }
        public Action OnPreviousVideoTrackReady { get; set; }
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
            RunTrack(_videoPlayerTray.GetCurrentTrack());
        }
        
        private void RunTrack(CrossPromoVideoTrack track)
        {
            track.OnTrackPrepared = () =>
            {
                Debug.Log($"track: {track}  CurrentTrack: {_videoPlayerTray.GetCurrentTrack()}");
                if(track == _videoPlayerTray.GetCurrentTrack())
                    track.Play(_screen);
            };

            track.OnTrackFinish = () =>
            {
                track.Stop();
               _videoPlayerTray.Rotate(-1);
               RunTrack(_videoPlayerTray.GetCurrentTrack());
               
            };
            track.Prepare();
            _videoPlayerTray.PrepareCachedTracks(() =>
            {
                OnNextVideoTrackReady?.Invoke();
            });

          
        }

        public void Next()
        {
            var tempTrack = _videoPlayerTray.GetCurrentTrack();
            _videoPlayerTray.Rotate(-1);
            RunTrack(_videoPlayerTray.GetCurrentTrack());
            tempTrack.Stop();
        }

        public void Previous()
        {
            var tempTrack = _videoPlayerTray.GetCurrentTrack();
            _videoPlayerTray.Rotate(1);
            RunTrack(_videoPlayerTray.GetCurrentTrack());
            tempTrack.Stop();
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