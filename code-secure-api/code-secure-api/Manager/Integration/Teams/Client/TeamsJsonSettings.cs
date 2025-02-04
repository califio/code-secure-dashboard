using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using JsonConverter = Newtonsoft.Json.JsonConverter;

namespace CodeSecure.Manager.Integration.Teams.Client;

public class TeamsJsonSettings : JsonSerializerSettings
{
    public TeamsJsonSettings()
    {
        NullValueHandling = NullValueHandling.Ignore;
        ContractResolver = new CamelCasePropertyNamesContractResolver();
        Converters = new List<JsonConverter>()
        {
            new StringEnumConverter()
        };
    }
};