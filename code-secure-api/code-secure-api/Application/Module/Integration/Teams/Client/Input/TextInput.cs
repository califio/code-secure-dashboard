namespace CodeSecure.Application.Module.Integration.Teams.Client.Input
{
    public class TextInput(string title) : IInput
    {
        public bool IsMultiline { get; set; }
        public bool MaxLength { get; set; }
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public bool IsRequired { get; set; }
        public string Title { get; set; } = title;
        public InputType Type => InputType.TextInput;
        public string Value { get; set; } = string.Empty;
    }
}