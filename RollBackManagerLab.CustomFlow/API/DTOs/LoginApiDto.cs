using System.Runtime.Serialization;

namespace RollBackManagerLab.CustomFlow.API.DTOs
{
    [DataContract]
    public class LoginApiDto
    {
        [DataMember]
        public string EmailAddress { get; set; } = string.Empty;
        [DataMember]
        public string Password { get; set; } = string.Empty;
        [DataMember]
        public string ApiKey { get; set; } = string.Empty;
    }
}
