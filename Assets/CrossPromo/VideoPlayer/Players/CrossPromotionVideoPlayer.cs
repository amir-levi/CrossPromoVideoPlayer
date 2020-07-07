using System;
using System.Collections.Generic;
using CrossPromo.Models;
using UnityEngine;

namespace CrossPromo.VideoPlayer.Players
{
    public abstract class CrossPromotionVideoPlayer : MonoBehaviour
    {
        public Action OnNextVideoTrackReady;
        public Action OnPreviousVideoTrackReady;
        public Action<CrossPromotionVideoPlayerTrack> OnVideoClicked;

        public abstract void Init(List<CrossPromotionVideoPlayerTrack> videoPlayerTracks,VideoPlayerScreen screen);
        public abstract void Next();
        public abstract void Previous();
        public abstract void Pause();
        public abstract void Resume();
        public abstract bool IsPlaying();

    }
}