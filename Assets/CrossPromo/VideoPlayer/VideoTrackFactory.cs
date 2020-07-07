using CrossPromo.Models;

namespace CrossPromo.VideoPlayer
{
    public abstract class VideoTrackFactory<T>
    {
        public abstract T GetVideoTrack(CrossPromotionVideoPlayerTrack crossPromotionTrack);
    }
}