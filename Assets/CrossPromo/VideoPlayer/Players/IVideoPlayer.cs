using System;

namespace CrossPromo.VideoPlayer.Players
{
    public interface IVideoPlayer
    {
        void Next();
        void Previous();
        void Pause();
        void Resume();
        bool IsPlaying();

        Action OnNextVideoTrackReady { get; set; }
        Action OnPreviousVideoTrackReady { get; set; }
        Action<int> OnVideoClicked { get; set; }
    }
}