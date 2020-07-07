using System;
using System.Collections.Generic;
using CrossPromo.Models;
using CrossPromo.Networking;
using CrossPromo.VideoPlayer.Players;
using CrossPromo.Views;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CrossPromo.Controllers
{
    public class MockCrossPromoController
    {
        private readonly CrossPromotionVideoPlayerView _videoPlayerView;
        
        public enum TrackOptions
        {
            Zero, 
            One,
            Two,
            ThreeOrMore,
            
        }
        
        public MockCrossPromoController()
        {
            _videoPlayerView = GameObject.FindGameObjectWithTag("CrossPromotionVideoPlayer").GetComponent<CrossPromotionVideoPlayerView>();
        }
        
        public async void CrossPromotionVideoPlayerView(Type videoPlayerType,string serverUrl,TrackOptions trackOptions)
        {
            var tracks = await CrossPromotionVideoPlayerWebRequest.FetchVideoTracksAsync(serverUrl);

            
            Debug.Log(tracks.Count);
            
            switch (trackOptions)
            {
                case TrackOptions.Zero:
                    tracks.Clear();
                    break;
                case TrackOptions.One:
                    tracks.RemoveRange(1,tracks.Count-1);
                    break;
                case TrackOptions.Two:
                    tracks.RemoveRange(1,tracks.Count-2);
                    break;
                case TrackOptions.ThreeOrMore:
                    var tmpTracks = new List<CrossPromotionVideoPlayerTrack>(tracks);
                    var rndCount = Random.Range(1, 20);
                    int lastId = tracks[tracks.Count - 1].Id;
                    for (int i = 0; i < rndCount; i++)
                    {
                        var track = new CrossPromotionVideoPlayerTrack
                        {
                            Id = lastId + i + 1,
                            VideoUrl = tracks[i % (tracks.Count - 1)].VideoUrl,
                            ClickUrl = tracks[i % (tracks.Count - 1)].ClickUrl,
                            TrackingUrl = tracks[i % (tracks.Count - 1)].TrackingUrl
                        };
                        Debug.Log(track.VideoUrl);
                        tmpTracks.Add(track);
                    }
                    tracks.Clear();
                    tracks = tmpTracks;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(trackOptions), trackOptions, null);
            }
            
            _videoPlayerView.Init(videoPlayerType,tracks);
        }
    }
}