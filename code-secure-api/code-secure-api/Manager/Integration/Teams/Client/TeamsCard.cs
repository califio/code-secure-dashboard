using Newtonsoft.Json;

namespace CodeSecure.Manager.Integration.Teams.Client
{
    public abstract class TeamsCard(CardType type)
    {
        [JsonProperty("@type")]
        public CardType Type { get; } = type;

        [JsonProperty("@context")]
        public string Context => "http://schema.org/extensions";
    }
}