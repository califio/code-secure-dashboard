namespace CodeSecure.Integration.Teams.Action
{
    public class OpenUriAction() : IAction(ActionType.OpenUri, "")
    {
        public OpenUriAction(string name) : this()
        {
            Name = name;
        }

        public OpenUriAction(string name, string uri, TargetOs targetOs = TargetOs.@default) : this(name)
        {
            Targets =
            [
                new Target
                {
                    OS = targetOs,
                    Uri = uri
                }
            ];
        }

        public IList<Target> Targets { get; set; } = new List<Target>();

        public void AddTarget(TargetOs targetOs, string uri)
        {
            Targets.Add(new Target(targetOs, uri));
        }
    }
}