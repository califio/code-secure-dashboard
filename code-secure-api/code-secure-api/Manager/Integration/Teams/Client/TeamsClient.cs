using System.Text;
using Newtonsoft.Json;

namespace CodeSecure.Manager.Integration.Teams.Client
{
    public class TeamsClient(string webhookUrl, HttpClient? client = null)
    {
        private readonly HttpClient client = client ?? new HttpClient();
        private readonly JsonSerializerSettings jsonSettings = new TeamsJsonSettings();

        public Task<HttpResponseMessage> PostAsync(TeamsCard card)
        {
            var content = JsonConvert.SerializeObject((object)card, jsonSettings);
            return client.PostAsync(webhookUrl, new StringContent(content, Encoding.UTF8, "application/json"));
        }
    }
}