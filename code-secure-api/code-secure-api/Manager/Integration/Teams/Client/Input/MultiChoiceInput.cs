using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace CodeSecure.Manager.Integration.Teams.Client.Input
{
    public class MultiChoiceInput(string title) : IInput
    {
        public enum Style
        {
            [EnumMember(Value = "normal")]
            Normal,
            [EnumMember(Value = "expanded")]
            Expanded
        }

        public IList<Choice> Choices { get; set; } = new List<Choice>();
        public bool IsMultiSelect { get; set; }
        [JsonProperty("style")] 
        public Style DisplayStyle { get; set; } = Style.Normal;
        
        public void AddChoice(string display, string value, bool isDefault = false)
        {
            Choices.Add(new Choice(display, value));
            if (isDefault)
            {
                Value = value;
            }
        }

        public string Id { get; set; } = Guid.NewGuid().ToString();
        public bool IsRequired { get; set; }
        public string Title { get; set; } = title;
        public InputType Type => InputType.MultiChoiceInput;
        public string Value { get; set; } = string.Empty;
    }
}