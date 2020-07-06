using System;

namespace CrossPromo.Models
{
    [Serializable]
    public class CrossPromotionVideoPlayerTrack
    {
        public int Id;
        public string VideoUrl;
        public string ClickUrl;
        public string TrackingUrl;
    }
}