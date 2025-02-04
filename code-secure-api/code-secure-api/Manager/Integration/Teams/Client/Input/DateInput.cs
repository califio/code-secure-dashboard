namespace CodeSecure.Manager.Integration.Teams.Client.Input
{
    public class DateInput : IInput
    {
        public InputType Type => InputType.DateInput;
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public bool IsRequired { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }
}