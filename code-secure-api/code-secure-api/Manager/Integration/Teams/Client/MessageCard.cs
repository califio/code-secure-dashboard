using CodeSecure.Manager.Integration.Teams.Client.Action;
using Newtonsoft.Json;

namespace CodeSecure.Manager.Integration.Teams.Client
{
    public class MessageCard() : TeamsCard(CardType.MessageCard)
    {
        public MessageCard(string title) : this()
        {
            Title = title;
        }

        public string Title { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public string? ThemeColor { get; set; }
        public string? Summary { get; set; }
        public IList<Section> Sections { get; set; } = new List<Section>();
        [JsonProperty("potentialAction")]
        public List<IAction> Actions { get; set; } = new List<IAction>();

        public IList<string> ExpectedActors { get; set; } = new List<string>();
        public bool HideOriginalBody { get; set; }
        public string? Originator { get; set; }

        public void AddAction(IAction action)
        {
            Actions.Add(action);
        }

        public void AddSection(Section section)
        {
            Sections.Add(section);
        }

        public void AddExpectedActor(string actor)
        {
            ExpectedActors.Add(actor);
        }
    }
}