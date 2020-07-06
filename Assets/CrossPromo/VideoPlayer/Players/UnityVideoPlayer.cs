using System;
using System.Collections.Generic;
using System.Linq;
using CrossPromo.Models;
using UnityEngine;
using UnityEngine.UI;

namespace CrossPromo.VideoPlayer.Players
{
    [Serializable]
    public class UnityVideoPlayer : CrossPromotionVideoPlayer
    {
        private RawImage _screen;
        private int[] _videoTracksIndices; 
        private VideoPlayerFactory<UnityVideoTrack> _videoPlayerFactory;
        private UnityVideoTrack _previousPlayingVideoTrack;
        private UnityVideoTrack _currentPlayingVideoTrack;

        [SerializeField]private int _cacheSize;
        private const int MaxCacheSize = 3;
        
        public override void Init(List<CrossPromotionVideoPlayerTrack> videoPlayerTracks, RawImage screen)
        {
            if (videoPlayerTracks == null || videoPlayerTracks.Count <= 0)
            {
                Debug.LogError("Assign VideoClips from the Editor");
                return;
            }
            
            VideoPlayerTracks = videoPlayerTracks;
            _screen = screen;

            _cacheSize = videoPlayerTracks.Count < MaxCacheSize ? videoPlayerTracks.Count : MaxCacheSize;
            
            _videoTracksIndices = new int[_cacheSize];
            _videoPlayerFactory = new UnityVideoPlayerFactory(VideoPlayerTracks.Count,transform);
            RunNextTrack(CurrentPlayedTrackIndex);
        }

        private void RunNextTrack(int index)
        {
            _currentPlayingVideoTrack = _videoPlayerFactory.GetVideoTrack(VideoPlayerTracks[index]);
            if(_previousPlayingVideoTrack != null)
                _previousPlayingVideoTrack.Stop();
            
            _videoTracksIndices[0] = index;
            CurrentPlayedTrackIndex = index;
            
            _currentPlayingVideoTrack.OnTrackPrepared = () =>
            {
                _currentPlayingVideoTrack.Play(_screen);
            };

            _currentPlayingVideoTrack.OnTrackFinish = () =>
            {
                _previousPlayingVideoTrack = _currentPlayingVideoTrack;
                RunNextTrack(_videoTracksIndices[1]);
            };
            
            StartCoroutine(_currentPlayingVideoTrack.Prepare());

            var nextIndex = (index + VideoPlayerTracks.Count + 1) % VideoPlayerTracks.Count;
            if(PrepareTrack(index, nextIndex))
                _videoTracksIndices[1] = nextIndex;
            
            var prevIndex = (index + VideoPlayerTracks.Count - 1) % VideoPlayerTracks.Count;
            if(PrepareTrack(index, prevIndex))
                _videoTracksIndices[2] = prevIndex;
        }

        private bool PrepareTrack(int currentIndex, int trackIndexToPrepare)
        {
            // check if VideoPlayerTracks doesnt have only 1 element
            if (trackIndexToPrepare != currentIndex)
            {
                // check if VideoPlayerTracks in trackIndexToPrepare is not null
                if (VideoPlayerTracks.ElementAtOrDefault(trackIndexToPrepare) != null)
                {
                    var player  = _videoPlayerFactory.GetVideoTrack(VideoPlayerTracks[trackIndexToPrepare]);
                    StartCoroutine(player.Prepare());
                    return true;
                }
            }

            return false;
        }
        


        public override void Next()
        {
            _previousPlayingVideoTrack = _currentPlayingVideoTrack;
            RunNextTrack(_videoTracksIndices[1]);
        }

        public override void Previous()
        {
            _previousPlayingVideoTrack = _currentPlayingVideoTrack;
            RunNextTrack(_videoTracksIndices[2]);
        }

        public override void Pause()
        {
            _currentPlayingVideoTrack.Pause();
        }

        public override void Resume()
        {
            _currentPlayingVideoTrack.Resume();
        }

        public override bool IsPlaying()
        {
            return false;
        }
       
    }
}