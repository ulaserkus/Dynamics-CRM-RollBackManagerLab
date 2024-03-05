using Shared.Model.Concrete;

namespace Queue.Framework.Data.Model
{
    public class EntityHistoryHashedData : MongoDbDocument
    {
        public string CrmHashedData { get; set; } = string.Empty;
        public Guid ConfigurationId { get; set; }
        public DateTime OperationDate { get; set; }
        public Guid ExecutionOwnerId { get; set; }
        public int OperationType { get; set; }
        public string LogicalName { get; set; }
        public string UserId { get; set; }
        public Guid RecordUniqueIdentifier { get; set; }
    }
}
