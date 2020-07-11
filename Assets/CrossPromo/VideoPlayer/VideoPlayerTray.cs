using System;
using System.Collections.Generic;
using CrossPromo.Models;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CrossPromo.VideoPlayer
{
    [Serializable]
    public class VideoPlayerTray
    {
        private readonly IVideoTrackFactory _videoTrackFactory;
        private List<CrossPromoVideoInfo> _videoPlayerTracks;
        [SerializeField]private CrossPromoVideoTrack[] _cachedTracks;
        [SerializeField] private CrossPromoVideoInfo[] _VideosInfo;

        private int _cacheForward;
        private int _cacheBackward;
        
        public VideoPlayerTray(List<CrossPromoVideoInfo> videoInfos, Transform transform, int cachedForward = 1, int cacheBackward = 1)
        {
            _cacheBackward = cacheBackward;
            _cacheForward = cachedForward;
            _videoPlayerTracks = new List<CrossPromoVideoInfo>(videoInfos);
            _videoPlayerTracks.RemoveAt(2);
            _videoPlayerTracks.RemoveAt(1);
            
            // for (int i = 0; i < Random.Range(10,20); i++)
            // {
            //     var index = i % (_videoPlayerTracks.Count - 1);
            //     CrossPromoVideoInfo info = new CrossPromoVideoInfo();
            //
            //     info.Id = i + 4;
            //     info.VideoUrl = _videoPlayerTracks[index].VideoUrl;
            //     info.LocalPath = _videoPlayerTracks[index].LocalPath;
            //     info.ClickUrl = _videoPlayerTracks[index].ClickUrl;
            //     info.TrackingUrl = _videoPlayerTracks[index].TrackingUrl;
            //     
            //     _videoPlayerTracks.Add(info);
            // }
            

            _VideosInfo = _videoPlayerTracks.ToArray();
            _videoTrackFactory = new CrossPromoVideoTrackFactory(videoInfos.Count,transform);

            var cacheSize = 1 + _cacheBackward + _cacheForward;
            _cachedTracks =  new CrossPromoVideoTrack[cacheSize];
            UpdateCachedTracks();
        }

        
        public void Rotate( int count)
        {
            if (_VideosInfo == null || _VideosInfo.Length < 2) return;
            count %= _VideosInfo.Length;
            if (count == 0) return;
            var left = count < 0 ? -count : _VideosInfo.Length + count;
            var right = count > 0 ? count : _VideosInfo.Length - count;
            if (left <= right)
            {
                for (var i = 0; i < left; i++)
                {
                    var temp = _VideosInfo[0];
                    Array.Copy(_VideosInfo, 1, _VideosInfo, 0, _VideosInfo.Length - 1);
                    _VideosInfo[_VideosInfo.Length - 1] = temp;
                }
            }
            else
            {
                for (var i = 0; i < right; i++)
                {
                    var temp = _VideosInfo[_VideosInfo.Length - 1];
                    Array.Copy(_VideosInfo, 0, _VideosInfo, 1, _VideosInfo.Length - 1);
                    _VideosInfo[0] = temp;
                }
            }
            
            UpdateCachedTracks();
        }
        
        public CrossPromoVideoTrack GetCurrentTrack()
        {
            return _cachedTracks[0];
        }
        
        private void UpdateCachedTracks()
        {
            _cachedTracks[0] =  _videoTrackFactory.GetVideoTrack(_VideosInfo[0]);

            var index = 0;
            var pointer = 1;

            for (var i = 1; i <= _cacheForward; i++)
            {
                index = (index + _videoPlayerTracks.Count + 1) % _videoPlayerTracks.Count;
                _cachedTracks[pointer] = _videoTrackFactory.GetVideoTrack(_VideosInfo[index]);
                pointer++;
            }

            index = 0;
            
            for (var i = 1; i <= _cacheBackward; i++)
            {
                index = (index + _videoPlayerTracks.Count - 1) % _videoPlayerTracks.Count;
                _cachedTracks[pointer] = _videoTrackFactory.GetVideoTrack(_VideosInfo[index]);
                pointer++;
            }
        }

        public void PrepareCachedTracks(Action onReady)
        {
            for (int i = 1; i < _cachedTracks.Length; i++)
            {
                _cachedTracks[i].OnTrackPrepared = onReady;
                _cachedTracks[i].Prepare();
            }
        }
       


       
    }
}