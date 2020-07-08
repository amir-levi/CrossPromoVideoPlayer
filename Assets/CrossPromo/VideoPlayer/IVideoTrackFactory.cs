
using CrossPromo.Models;

namespace CrossPromo.VideoPlayer
{
    public interface IVideoTrackFactory
    {
       CrossPromoVideoTrack GetVideoTrack(CrossPromoVideoInfo info);
    }
}
