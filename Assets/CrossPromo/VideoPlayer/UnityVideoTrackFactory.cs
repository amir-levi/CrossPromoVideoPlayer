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
        public override UnityVideoTrack GetVideoTrack(VideoPlayerListItem playerListItem)
        {
            var track = _videoTracks.FirstOrDefault(item => item.Id == playerListItem.Id);
            if (track == null)
                track = CreateVideoTrack(playerListItem.Id, playerListItem.VideoUrl);

            return track;
        }
        
        private UnityVideoTrack CreateVideoTrack(int id, string url)
        {
            var go = new GameObject($"Video Player_{id}" );
            go.transform.SetParent(_parent);
            var videoTrack = go.AddComponent<UnityVideoTrack>();
            videoTrack.Init(id,url);
            _videoTracks.Add(videoTrack);
            return videoTrack;
        }
        
    }
}