using System.Text;
using FluentResults;
using Newtonsoft.Json;

namespace CodeSecure.Application.Module.Integration.Teams.Client
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

        public async Task<Result<bool>> TestConnectionAsync()
        {
            var message = new MessageCard("Test Notification")
            {
                Text = "This is test message"
            };
            try
            {
                var response = await PostAsync(message);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                return Result.Fail($"Error response status {response.StatusCode}");
            }
            catch (Exception e)
            {
                return Result.Fail(e.Message);
            }
        }
    }
}