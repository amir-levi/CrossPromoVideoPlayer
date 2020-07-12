
using System.Collections.Generic;
using CrossPromo.Models;

namespace CrossPromo.VideoPlayer
{
    public interface IVideoTrackFactory
    {
       CrossPromoVideoTrack GetVideoTrack(CrossPromoVideoInfo info);
       void Update(List<CrossPromoVideoInfo> videoInfos);
    }
}
