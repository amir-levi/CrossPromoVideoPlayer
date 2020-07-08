using System;

namespace CrossPromo.VideoPlayer.Actions
{
    public interface IVideoClickedAction
    {
        Action<int> OnVideoClicked { get; set; }
    }
}