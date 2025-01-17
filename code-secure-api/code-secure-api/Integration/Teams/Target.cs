namespace CodeSecure.Integration.Teams
{
    public enum TargetOs
    {
        @default,
        iOS,
        Android,
        Windows
    }
    public class Target()
    {
        public Target(TargetOs os, string uri) : this()
        {
            this.OS = os;
            this.Uri = uri;
        }

        public TargetOs OS { get; set; } = TargetOs.@default;
        public string Uri { get; set; } = string.Empty;
    }
}