using System;

namespace CrossPromo.Models
{
    [Serializable]
    public class VideoPlayerListItem
    {
        public int Id;
        public string VideoUrl;
        public string ClickUrl;
        public string TrackingUrl;
    }
}