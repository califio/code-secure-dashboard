using Newtonsoft.Json;

namespace CodeSecure.Integration.Teams
{
    public abstract class TeamsCard(CardType type)
    {
        [JsonProperty("@type")]
        public CardType Type { get; } = type;

        [JsonProperty("@context")]
        public string Context => "http://schema.org/extensions";
    }
}