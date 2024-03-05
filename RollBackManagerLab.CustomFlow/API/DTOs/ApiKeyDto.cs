using System.Runtime.Serialization;

namespace RollBackManagerLab.CustomFlow.API.DTOs
{
    [DataContract]
    public class ApiKeyDto
    {
        [DataMember(Name = "key")]
        public string Key { get; set; }

        [DataMember(Name = "expiresInUTC")]
        public string ExpiresInUTC { get; set; }
    }
}
