using System;
using System.Collections.Generic;
using CrossPromo.Models;
using UnityEngine;

namespace CrossPromo.VideoPlayer
{
    [Serializable]
    public class VideoPlayerTray
    {
        public CrossPromoVideoTrack CurrentTrack;// { get; private set; }
        public CrossPromoVideoTrack NextTrack;// { get; private set; }
        public CrossPromoVideoTrack PreviousTrack;// { get; private set; }
        private readonly IVideoTrackFactory _videoTrackFactory;
        private List<CrossPromoVideoInfo> _videoPlayerTracks;
        private int _currentTrackIndex;
        [SerializeField]private CrossPromoVideoTrack[] _trackArray;
        private int _traySize = 3;

        private int _cacheForward = 1;
        private int _cacheBackward = 1;
        
        public VideoPlayerTray(List<CrossPromoVideoInfo> videoPlayerTracks, Transform transform)
        {
            _videoPlayerTracks = videoPlayerTracks;
            _videoTrackFactory = new CrossPromoVideoTrackFactory(videoPlayerTracks.Count,transform);
            CurrentTrack =  _videoTrackFactory.GetVideoTrack(_videoPlayerTracks[_currentTrackIndex]);

            var cacheSize = 1 + _cacheBackward + _cacheForward;

            _trackArray = _videoPlayerTracks.Count < cacheSize ? new CrossPromoVideoTrack[_videoPlayerTracks.Count] : new CrossPromoVideoTrack[cacheSize];
            _trackArray[0] = _videoTrackFactory.GetVideoTrack(_videoPlayerTracks[_currentTrackIndex]);

        }

        
        public void Rotate(int count)
        {
            if (_trackArray == null || _trackArray.Length < 2) return;
            count %= _trackArray.Length;
            if (count == 0) return;
            var left = count < 0 ? -count : _trackArray.Length + count;
            var right = count > 0 ? count : _trackArray.Length - count;
            if (left <= right)
            {
                for (var i = 0; i < left; i++)
                {
                    var temp = _trackArray[0];
                    Array.Copy(_trackArray, 1, _trackArray, 0, _trackArray.Length - 1);
                    _trackArray[_trackArray.Length - 1] = temp;
                }
            }
            else
            {
                for (var i = 0; i < right; i++)
                {
                    var temp = _trackArray[_trackArray.Length - 1];
                    Array.Copy(_trackArray, 0, _trackArray, 1, _trackArray.Length - 1);
                    _trackArray[0] = temp;
                }
            }
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
                _trackArray[1] = _videoTrackFactory.GetVideoTrack(_videoPlayerTracks[1]);
            }
            
            

            if (_videoPlayerTracks.Count == 2)
            {
                var nextIndex = (_currentTrackIndex + _videoPlayerTracks.Count + 1) % _videoPlayerTracks.Count;
                NextTrack = PreviousTrack = _videoTrackFactory.GetVideoTrack(_videoPlayerTracks[nextIndex]);
                
                
                
            }
            else if (_videoPlayerTracks.Count > 2)
            {
                var nextIndex = (_currentTrackIndex + _videoPlayerTracks.Count + 1) % _videoPlayerTracks.Count;
                NextTrack = _videoTrackFactory.GetVideoTrack(_videoPlayerTracks[nextIndex]) as CrossPromoVideoTrack;
                var prevIndex = (_currentTrackIndex + _videoPlayerTracks.Count - 1) % _videoPlayerTracks.Count;
                PreviousTrack = _videoTrackFactory.GetVideoTrack(_videoPlayerTracks[prevIndex]);
            }
            else
            {
                Debug.Log("Video Player Tracks are 1 or lower no need to update tracks");
            }
        }
        
    }
}