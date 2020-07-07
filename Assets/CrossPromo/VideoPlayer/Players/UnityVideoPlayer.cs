using System;
using System.Collections.Generic;
using CrossPromo.Models;
using UnityEngine;

namespace CrossPromo.VideoPlayer.Players
{
    [Serializable]
    public class UnityVideoPlayer : CrossPromotionVideoPlayer
    {
        private VideoPlayerScreen _screen;
        private VideoPlayerTray _videoPlayerTray;
        
        public override void Init(List<CrossPromotionVideoPlayerTrack> videoPlayerTracks, VideoPlayerScreen screen)
        {
            if (videoPlayerTracks == null || videoPlayerTracks.Count <= 0)
            {
                Debug.LogError("No Video Player Tracks - Check Data fetch from the server");
                return;
            }
            
            _screen = screen;
            _screen.OnClick = () =>
            {
                OnVideoClicked?.Invoke(_videoPlayerTray.CurrentTrack.CrossPromotionTrack);
            };
            _videoPlayerTray = new VideoPlayerTray(videoPlayerTracks,transform);
            RunTrack(_videoPlayerTray.CurrentTrack);
        }
        
        private void RunTrack(UnityVideoTrack track)
        {
            track.OnTrackPrepared = () =>
            {
                track.Play(_screen);
            };

            track.OnTrackFinish = () =>
            {
                track.Stop();
                var nextTrack = _videoPlayerTray.GetNextTrack();
                RunTrack(nextTrack);
            };
            
            StartCoroutine(track.Prepare());
            _videoPlayerTray.UpdateTray(track);

            if (_videoPlayerTray.NextTrack != null)
            {
                _videoPlayerTray.NextTrack.OnTrackPrepared = () =>
                {
                    OnNextVideoTrackReady?.Invoke();
                };
                StartCoroutine(_videoPlayerTray.NextTrack.Prepare());
            }
            
            if (_videoPlayerTray.PreviousTrack != null)
            {

                if (_videoPlayerTray.PreviousTrack != _videoPlayerTray.NextTrack)
                {
                    _videoPlayerTray.PreviousTrack.OnTrackPrepared = () =>
                    {
                        OnPreviousVideoTrackReady?.Invoke();
                    };
                    StartCoroutine(_videoPlayerTray.PreviousTrack.Prepare());
                }
            }
        }

        public override void Next()
        {
            _videoPlayerTray.CurrentTrack.Stop();
            var track = _videoPlayerTray.GetNextTrack();
            RunTrack(track);
        }

        public override void Previous()
        {
            _videoPlayerTray.CurrentTrack.Stop();
            var track = _videoPlayerTray.GetPreviousTrack();
            RunTrack(track);
        }

        public override void Pause()
        {
          _videoPlayerTray.CurrentTrack.Pause();
        }

        public override void Resume()
        {
           _videoPlayerTray.CurrentTrack.Resume();
        }

        public override bool IsPlaying()
        {
           return _videoPlayerTray.CurrentTrack.IsPlaying();
        }
       
    }
}