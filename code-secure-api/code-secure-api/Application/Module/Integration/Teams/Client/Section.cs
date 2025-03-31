using System.Text.Json.Serialization;
using CodeSecure.Application.Module.Integration.Teams.Client.Action;

namespace CodeSecure.Application.Module.Integration.Teams.Client
{
    public class Section(string title = "")
    {
        public string? ActivityImage { get; set; }
        public string? ActivitySubtitle { get; set; }
        public string? ActivityText { get; set; }
        public string? ActivityTitle { get; set; }
        public IList<MessageFact> Facts { get; set; } = new List<MessageFact>();
        public string? HeroImage { get; set; }
        public IList<MessageImage>? Images { get; set; }
        [JsonPropertyName("potentialAction")]
        public IList<IAction>? Actions { get; set; }
        public bool StartGroup { get; set; }
        public string Text { get; set; } = string.Empty;
        public string Title { get; set; } = title;
        public void AddFact(string name, string value)
        {
            Facts.Add(new MessageFact(name, value));
        }
    }
}