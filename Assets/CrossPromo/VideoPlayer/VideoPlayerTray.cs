using System;
using System.Collections.Generic;
using System.Linq;
using CrossPromo.Models;
using UnityEngine;

namespace CrossPromo.VideoPlayer
{
    [Serializable]
    public class VideoPlayerTray
    {
        private IVideoTrackFactory _videoTrackFactory;
        private CrossPromoVideoInfo[] _videosInfo;

        private int _cacheForward;
        private int _cacheBackward;
        
        public VideoPlayerTray(List<CrossPromoVideoInfo> videoInfos, Transform transform, int cachedForward = 1, int cacheBackward = 1)
        {
            _cacheBackward = cacheBackward;
            _cacheForward = cachedForward;
            _videosInfo = videoInfos.ToArray();
            _videoTrackFactory = new CrossPromoVideoTrackFactory(transform);
            _videoTrackFactory.Update(GetNextTracks());
        }

        public Action TrackPrepared;


        public void Rotate( int count)
        {
            if (_videosInfo == null || _videosInfo.Length < 2) return;
            count %= _videosInfo.Length;
            if (count == 0) return;
            var left = count < 0 ? -count : _videosInfo.Length + count;
            var right = count > 0 ? count : _videosInfo.Length - count;
            if (left <= right)
            {
                for (var i = 0; i < left; i++)
                {
                    var temp = _videosInfo[0];
                    Array.Copy(_videosInfo, 1, _videosInfo, 0, _videosInfo.Length - 1);
                    _videosInfo[_videosInfo.Length - 1] = temp;
                }
            }
            else
            {
                for (var i = 0; i < right; i++)
                {
                    var temp = _videosInfo[_videosInfo.Length - 1];
                    Array.Copy(_videosInfo, 0, _videosInfo, 1, _videosInfo.Length - 1);
                    _videosInfo[0] = temp;
                }
            }

            _videoTrackFactory.Update(GetNextTracks());
        }

        private List<CrossPromoVideoInfo> GetNextTracks()
        {
            var index = 0;
            
            List<CrossPromoVideoInfo> videoInfos = new List<CrossPromoVideoInfo>();
            videoInfos.Add(_videosInfo[0]);
            
            for (var i = 1; i <= _cacheForward; i++)
            {
                index = (index + _videosInfo.Length + 1) % _videosInfo.Length;
                videoInfos.Add(_videosInfo[index]);
            }
            
            index = 0;
            
            for (var i = 1; i <= _cacheBackward; i++)
            {
                index = (index + _videosInfo.Length - 1) % _videosInfo.Length;
                videoInfos.Add(_videosInfo[index]);
            }

            return videoInfos.Distinct().ToList();
        }
        
        public CrossPromoVideoTrack GetCurrentTrack()
        {
            return _videoTrackFactory.GetVideoTrack(_videosInfo[0]);
        }
    }
}