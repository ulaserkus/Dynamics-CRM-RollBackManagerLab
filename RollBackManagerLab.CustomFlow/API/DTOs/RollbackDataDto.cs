using System;
using System.Runtime.Serialization;

namespace RollBackManagerLab.CustomFlow.API.DTOs
{
    [DataContract]
    public class RollbackDataDto
    {
        [DataMember(Name = "configurationId")]
        public Guid ConfigurationId { get; set; }

        [DataMember(Name = "historyId")]
        public string HistoryId { get; set; }
    }
}
