namespace CodeSecure.Manager.Integration.Teams.Client.Input
{
    public enum InputType
    {
        TextInput,
        DateInput,
        MultiChoiceInput
    }
    public interface IInput
    {
        public string Id { get; set; }

        public bool IsRequired { get; set; }

        public string Title { get; set; }

        public InputType Type { get; }

        public string Value { get; set; }
    }
}