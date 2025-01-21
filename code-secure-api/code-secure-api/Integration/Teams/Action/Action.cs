using Newtonsoft.Json;

namespace CodeSecure.Integration.Teams.Action
{
    public enum ActionType
    {
        ActionCard,
        InvokeAddInCommand,
        HttpPOST,
        OpenUri
    }
    
    public abstract class IAction(ActionType type, string name)
    {
        [JsonProperty("@type")]
        public ActionType Type { get; } = type;

        public string Name { get; set; } = name;
    }
}