using CrossPromo.Models;

namespace CrossPromo.VideoPlayer
{
    public abstract class VideoPlayerFactory<T>
    {
        public abstract T GetVideoTrack(CrossPromotionVideoPlayerTrack crossPromotionTrack);
    }
}