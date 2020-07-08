using System;

namespace CrossPromo.Models
{
    [Serializable]
    public class CrossPromoVideoInfo
    {
        public int Id;
        public string VideoUrl;
        public string LocalPath;
        public string ClickUrl;
        public string TrackingUrl;
    }
}