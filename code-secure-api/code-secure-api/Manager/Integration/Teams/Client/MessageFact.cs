namespace CodeSecure.Manager.Integration.Teams.Client
{
    public class MessageFact(string name, string value)
    {
        public string Name { get; private set; } = name;
        public string Value { get; private set; } = value;
    }
}