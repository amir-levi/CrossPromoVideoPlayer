using System.Collections.Generic;
using CrossPromo.Models;
using UnityEngine;

namespace CrossPromo.VideoPlayer
{
    [System.Serializable]
    public class VideoPlayerTray
    {
        public CrossPromoVideoTrack CurrentTrack { get; private set; }
        public CrossPromoVideoTrack NextTrack { get; private set; }
        public CrossPromoVideoTrack PreviousTrack { get; private set; }
        private readonly IVideoTrackFactory _videoTrackFactory;
        private List<CrossPromoVideoInfo> _videoPlayerTracks;
        private int _currentTrackIndex;
        
        public VideoPlayerTray(List<CrossPromoVideoInfo> videoPlayerTracks, Transform transform)
        {
            _videoPlayerTracks = videoPlayerTracks;
            _videoTrackFactory = new CrossPromoVideoTrackFactory(videoPlayerTracks.Count,transform);
            CurrentTrack =  _videoTrackFactory.GetVideoTrack(_videoPlayerTracks[_currentTrackIndex]) as CrossPromoVideoTrack;
        }

        public CrossPromoVideoTrack RotateForward()
        {
            _currentTrackIndex = (_currentTrackIndex + _videoPlayerTracks.Count + 1) % _videoPlayerTracks.Count;
            return NextTrack;
        }

        public CrossPromoVideoTrack RotateBackward()
        {
            _currentTrackIndex = (_currentTrackIndex + _videoPlayerTracks.Count - 1) % _videoPlayerTracks.Count;
            return PreviousTrack;
        }

        public void UpdateTray(CrossPromoVideoTrack currentTrack)
        {
            CurrentTrack = currentTrack;

            if (_videoPlayerTracks.Count == 2)
            {
                var nextIndex = (_currentTrackIndex + _videoPlayerTracks.Count + 1) % _videoPlayerTracks.Count;
                NextTrack = PreviousTrack = _videoTrackFactory.GetVideoTrack(_videoPlayerTracks[nextIndex]) as CrossPromoVideoTrack;
                
            }
            else if (_videoPlayerTracks.Count > 2)
            {
                var nextIndex = (_currentTrackIndex + _videoPlayerTracks.Count + 1) % _videoPlayerTracks.Count;
                NextTrack = _videoTrackFactory.GetVideoTrack(_videoPlayerTracks[nextIndex]) as CrossPromoVideoTrack;
                var prevIndex = (_currentTrackIndex + _videoPlayerTracks.Count - 1) % _videoPlayerTracks.Count;
                PreviousTrack = _videoTrackFactory.GetVideoTrack(_videoPlayerTracks[prevIndex]) as CrossPromoVideoTrack;
            }
            else
            {
                Debug.Log("Video Player Tracks are 1 or lower no need to update tracks");
            }
        }
        
    }
}