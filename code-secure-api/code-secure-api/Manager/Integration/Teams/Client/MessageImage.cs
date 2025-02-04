namespace CodeSecure.Manager.Integration.Teams.Client
{
    public class MessageImage(string image, string title)
    {
        public string Image { get; set; } = image;
        public string Title { get; set; } = title;
    }
}