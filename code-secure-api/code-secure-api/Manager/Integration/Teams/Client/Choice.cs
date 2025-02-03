namespace CodeSecure.Manager.Integration.Teams.Client
{
    public class Choice(string display, string value)
    {
        public string Display { get; set; } = display;
        public string Value { get; set; } = value;
    }
}