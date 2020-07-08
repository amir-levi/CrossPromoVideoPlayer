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
        private VideoPlayerTray _videoPlayerTray;

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
                OnVideoClicked?.Invoke(_videoPlayerTray.CurrentTrack.Id);
            };
            _videoPlayerTray = new VideoPlayerTray(videoPlayerTracks,transform);
            RunTrack(_videoPlayerTray.CurrentTrack);
        }
        
        private void RunTrack(CrossPromoVideoTrack track)
        {
            track.OnTrackPrepared = () =>
            {
                track.Play(_screen);
            };

            track.OnTrackFinish = () =>
            {
                track.Stop();
                var nextTrack = _videoPlayerTray.RotateForward();
                RunTrack(nextTrack);
            };
            
            track.Prepare();
            _videoPlayerTray.UpdateTray(track);

            if (_videoPlayerTray.NextTrack != null)
            {
                _videoPlayerTray.NextTrack.OnTrackPrepared = () =>
                {
                    OnNextVideoTrackReady?.Invoke();
                };
                _videoPlayerTray.NextTrack.Prepare();
            }
            
            if (_videoPlayerTray.PreviousTrack != null)
            {
                if (_videoPlayerTray.PreviousTrack != _videoPlayerTray.NextTrack)
                {
                    _videoPlayerTray.PreviousTrack.OnTrackPrepared = () =>
                    {
                        OnPreviousVideoTrackReady?.Invoke();
                    };
                    _videoPlayerTray.PreviousTrack.Prepare();
                }
            }
        }

        public void Next()
        {
            _videoPlayerTray.CurrentTrack.Stop();
            var track = _videoPlayerTray.RotateForward();
            RunTrack(track);
        }

        public void Previous()
        {
            _videoPlayerTray.CurrentTrack.Stop();
            var track = _videoPlayerTray.RotateBackward();
            RunTrack(track);
        }

        public void Pause()
        {
          _videoPlayerTray.CurrentTrack.Pause();
        }

        public void Resume()
        {
           _videoPlayerTray.CurrentTrack.Resume();
        }

        public bool IsPlaying()
        {
           return _videoPlayerTray.CurrentTrack.IsPlaying();
        }

      
    }
}