namespace SuperSonic.VideoPlayer
{
    public interface IVideoPlayerTrack
    {
        int Id { get; set; }
        string VideoUrl { get; set; }
        string ClickUrl { get; set; }
        string TrackingUrl { get; set; }
    }
}