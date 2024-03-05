using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RollBackManagerLab.CustomFlow.API.DTOs
{
    [DataContract]
    public class EntityHistoryApiDto
    {
        [DataMember(Name = "configurationId")]
        public Guid ConfigurationId { get; set; }

        [DataMember(Name = "executionOwnerId")]
        public Guid ExecutionOwnerId { get; set; }

        [DataMember(Name = "operationDate")]
        public string OperationDate { get; set; }

        [DataMember(Name = "operationType")]
        public int OperationType { get; set; }

        [DataMember(Name = "logicalName")]
        public string LogicalName { get; set; }

        [DataMember(Name = "recordUniqueIdentifier")]
        public Guid RecordUniqueIdentifier { get; set; }

        [DataMember(Name = "attributes")]
        public List<EntityFieldApiDto> Attributes { get; set; }
    }
}
