using System.Collections.Generic;
using CrossPromo.Models;
using UnityEngine;
using UnityEngine.UI;

namespace CrossPromo.VideoPlayer.Players
{
    public abstract class CrossPromotionVideoPlayer : MonoBehaviour,IVideoPlayer
    {
        [SerializeField]protected int CurrentPlayedTrackIndex;
        [SerializeField]protected List<CrossPromotionVideoPlayerTrack> VideoPlayerTracks = new List<CrossPromotionVideoPlayerTrack>();


        public abstract void Init(List<CrossPromotionVideoPlayerTrack> videoPlayerTracks,RawImage screen);
        public abstract void Next();
        public abstract void Previous();
        public abstract void Pause();
        public abstract void Resume();
        public abstract bool IsPlaying();

    }
}