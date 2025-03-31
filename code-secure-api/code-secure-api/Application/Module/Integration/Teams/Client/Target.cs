namespace CodeSecure.Application.Module.Integration.Teams.Client
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
            OS = os;
            Uri = uri;
        }

        public TargetOs OS { get; set; } = TargetOs.@default;
        public string Uri { get; set; } = string.Empty;
    }
}