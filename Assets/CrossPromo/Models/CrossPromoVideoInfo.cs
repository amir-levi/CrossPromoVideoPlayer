using System;
using System.Text;

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


        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Id: {Id}");
            sb.AppendLine($"VideoUrl: {VideoUrl}");
            sb.AppendLine($"LocalPath: {LocalPath}");
            sb.AppendLine($"TrackingUrl: {TrackingUrl}");
    
            return sb.ToString();
        }
    }
}