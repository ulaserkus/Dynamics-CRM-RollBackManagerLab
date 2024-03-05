namespace Shared.Api.DTOs
{
    public class EntityHistoryApiDto
    {
        public Guid ConfigurationId { get; set; }
        public DateTime OperationDate { get; set; }
        public Guid ExecutionOwnerId { get; set; }
        public int OperationType { get; set; }
        public string? LogicalName { get; set; }
        public string? UserId { get; set; }
        public string? Id { get; set; }
        public Guid RecordUniqueIdentifier { get; set; }
        public List<EntityFieldApiDto>? Attributes { get; set; }
    }
}
