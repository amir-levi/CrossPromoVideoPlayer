using System.Collections.Generic;
using CrossPromo.Models;
using UnityEngine;

namespace CrossPromo.VideoPlayer
{
    [System.Serializable]
    public class VideoPlayerTray
    {
        public UnityVideoTrack CurrentTrack { get; private set; }
        public UnityVideoTrack NextTrack { get; private set; }
        public UnityVideoTrack PreviousTrack { get; private set; }
        private readonly VideoTrackFactory<UnityVideoTrack> _videoTrackFactory;
        [SerializeField] private List<CrossPromotionVideoPlayerTrack> _videoPlayerTracks;
        [SerializeField]private int _currentTrackIndex;
        
        public VideoPlayerTray(List<CrossPromotionVideoPlayerTrack> videoPlayerTracks, Transform transform)
        {
            _videoPlayerTracks = videoPlayerTracks;
            _videoTrackFactory = new UnityVideoTrackFactory(videoPlayerTracks.Count,transform);
            CurrentTrack = _videoTrackFactory.GetVideoTrack(_videoPlayerTracks[_currentTrackIndex]);
        }

        public UnityVideoTrack GetNextTrack()
        {
            _currentTrackIndex = (_currentTrackIndex + _videoPlayerTracks.Count + 1) % _videoPlayerTracks.Count;
            return NextTrack;
        }

        public UnityVideoTrack GetPreviousTrack()
        {
            _currentTrackIndex = (_currentTrackIndex + _videoPlayerTracks.Count - 1) % _videoPlayerTracks.Count;
            return PreviousTrack;
        }

        public void UpdateTray(UnityVideoTrack currentTrack)
        {
            CurrentTrack = currentTrack;

            if (_videoPlayerTracks.Count == 2)
            {
                var nextIndex = (_currentTrackIndex + _videoPlayerTracks.Count + 1) % _videoPlayerTracks.Count;
                NextTrack = PreviousTrack = _videoTrackFactory.GetVideoTrack(_videoPlayerTracks[nextIndex]);
                
            }
            else if (_videoPlayerTracks.Count > 2)
            {
                var nextIndex = (_currentTrackIndex + _videoPlayerTracks.Count + 1) % _videoPlayerTracks.Count;
                NextTrack = _videoTrackFactory.GetVideoTrack(_videoPlayerTracks[nextIndex]);
                var prevIndex = (_currentTrackIndex + _videoPlayerTracks.Count - 1) % _videoPlayerTracks.Count;
                Debug.Log(prevIndex);
                PreviousTrack = _videoTrackFactory.GetVideoTrack(_videoPlayerTracks[prevIndex]);
            }
            else
            {
                Debug.Log("Video Player Tracks are 1 or lower no need to update tracks");
            }
        }
        
    }
}