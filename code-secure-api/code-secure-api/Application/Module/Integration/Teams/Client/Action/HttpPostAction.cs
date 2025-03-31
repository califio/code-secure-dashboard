using System.Runtime.Serialization;

namespace CodeSecure.Application.Module.Integration.Teams.Client.Action
{
    public class HttpPostAction(string name) : IAction(ActionType.HttpPOST, name)
    {
        public class Header(string name, string value)
        {
            public string Name { get; } = name;

            public string Value { get; } = value;
        }
        public enum ContentType
        {
            [EnumMember(Value = "application/json")]
            Json,
            [EnumMember(Value = "application/x-www-form-urlencoded")]
            Form
        }
        
        public HttpPostAction(string name, string target) : this(name)
        {
            Target = target;
        }

        public string Body { get; set; } = string.Empty;
        public ContentType BodyContentType { get; set; } = ContentType.Json;
        public IList<Header> Headers { get; set; } = new List<Header>();
        public string Target { get; set; } = string.Empty;
        
        public void AddHeader(string name, string value)
        {
            Headers.Add(new Header(name, value));
        }
    }
}