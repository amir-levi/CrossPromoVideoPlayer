using System.Collections.Generic;
using System.Linq;
using CrossPromo.Models;
using UnityEngine;

namespace CrossPromo.VideoPlayer
{
    public class CrossPromoVideoTrackFactory : IVideoTrackFactory 
    {
        private readonly List<CrossPromoVideoTrack> _videoTracks;
        private readonly Transform _parent;

        public CrossPromoVideoTrackFactory(int trackCount, Transform parent)
        {
            _videoTracks = new List<CrossPromoVideoTrack>(trackCount);
            _parent = parent;
        }
        public CrossPromoVideoTrack GetVideoTrack(CrossPromoVideoInfo info)
        {
            var track = _videoTracks.FirstOrDefault(item => item.Id == info.Id);
            if (track == null)
                track = CreateVideoTrack(info.Id, info.LocalPath);

            return track;
        }
        
        private CrossPromoVideoTrack CreateVideoTrack(int id, string localPath)
        {
            var go = new GameObject($"Video Player_{id}" );
            go.transform.SetParent(_parent);
            var videoTrack = go.AddComponent<CrossPromoVideoTrack>();
            videoTrack.Init(id,localPath);
            _videoTracks.Add(videoTrack);
            return videoTrack;
        }
        
    }
}