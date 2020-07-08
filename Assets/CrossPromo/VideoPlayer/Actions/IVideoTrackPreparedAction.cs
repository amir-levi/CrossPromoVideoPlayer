using System;

namespace CrossPromo.VideoPlayer.Actions
{
    public interface IVideoTrackPreparedAction
    {
        Action OnNextVideoTrackReady { get; set; }
        Action OnPreviousVideoTrackReady { get; set; }
    }
}