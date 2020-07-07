using System.Collections.Generic;
using System.Linq;
using CrossPromo.Models;
using UnityEngine;

namespace CrossPromo.VideoPlayer
{
    public class UnityVideoTrackFactory : VideoTrackFactory<UnityVideoTrack>
    {
        private readonly List<UnityVideoTrack> _videoTracks;
        private readonly Transform _parent;

        public UnityVideoTrackFactory(int trackCount, Transform parent)
        {
            _videoTracks = new List<UnityVideoTrack>(trackCount);
            _parent = parent;
        }
        public override UnityVideoTrack GetVideoTrack(CrossPromotionVideoPlayerTrack crossPromotionTrack)
        {
            var track = _videoTracks.FirstOrDefault(t => t.CrossPromotionTrack.Id == crossPromotionTrack.Id);
            if (track == null)
                track = CreateVideoTrack(crossPromotionTrack);

            return track;
        }
        
        private UnityVideoTrack CreateVideoTrack(CrossPromotionVideoPlayerTrack crossPromotionTrack)
        {
            var go = new GameObject($"Video Player_{crossPromotionTrack.Id}" );
            go.transform.SetParent(_parent);
            var videoTrack = go.AddComponent<UnityVideoTrack>();
            videoTrack.Init(crossPromotionTrack);
            _videoTracks.Add(videoTrack);
            return videoTrack;
        }
        
    }
}