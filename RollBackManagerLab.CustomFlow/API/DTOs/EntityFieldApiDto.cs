using System.Runtime.Serialization;

namespace RollBackManagerLab.CustomFlow.API.DTOs
{
    [DataContract]
    public class EntityFieldApiDto
    {
        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "value")]
        public string Value { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}
