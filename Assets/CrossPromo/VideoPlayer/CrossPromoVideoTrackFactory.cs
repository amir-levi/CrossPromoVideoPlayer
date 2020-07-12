using System;
using System.Collections.Generic;
using System.Linq;
using CrossPromo.Models;
using UnityEngine;

namespace CrossPromo.VideoPlayer
{
    [Serializable]
    public class CrossPromoVideoTrackFactory : IVideoTrackFactory 
    {
        private List<CrossPromoVideoTrack> _videoTracks;
        private List<CrossPromoVideoTrack> _recycledVideoTracks;
        private readonly Transform _parent;

        public CrossPromoVideoTrackFactory(Transform parent)
        {
            _videoTracks = new List<CrossPromoVideoTrack>();
            _recycledVideoTracks = new List<CrossPromoVideoTrack>();
            _parent = parent;
        }
        public CrossPromoVideoTrack GetVideoTrack(CrossPromoVideoInfo info)
        {
            return _videoTracks.FirstOrDefault(item => item.Id == info.Id);
        }

        public void Update(List<CrossPromoVideoInfo> videoInfos)
        {
            var infos = videoInfos.Where(i => _videoTracks.All(x => x.Id != i.Id)).ToList();
            
            foreach (var info in infos)
            {
                _videoTracks.Add(CreateVideoTrack(info.Id,info.LocalPath));
            }
            
            var tracksToRecycle = _videoTracks.Where(a => videoInfos.All(x => x.Id != a.Id)).ToList();

            foreach (var v in tracksToRecycle)
            {
                v.gameObject.SetActive(false);
                _videoTracks.Remove(v);
                _recycledVideoTracks.Add(v);
            }
        }

        private CrossPromoVideoTrack CreateVideoTrack(int id, string localPath)
        {
            CrossPromoVideoTrack videoTrack;
            
            if (_recycledVideoTracks.Count > 0)
            {
                videoTrack = _recycledVideoTracks.First();
                videoTrack.gameObject.name = $"Video Player_{id}";
                videoTrack.gameObject.SetActive(true);
                _recycledVideoTracks.RemoveAt(0);
            }
            else
            {
                var go = new GameObject($"Video Player_{id}" );
                go.transform.SetParent(_parent);
                videoTrack = go.AddComponent<CrossPromoVideoTrack>();
            }
            
            videoTrack.Init(id,localPath);
            return videoTrack;
        }
        
    }
}