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
    }
}