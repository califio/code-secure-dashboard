using Newtonsoft.Json;

namespace CodeSecure.Manager.Integration.Teams.Client.Action
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