namespace Shared.Api.DTOs
{
    public class GetEntityHistoryByConfigurationApiDto
    {
        public Guid ConfigurationId { get; set; }
        public string UserId { get; set; }
        public int Page { get; set; }
        public int Count { get; set; }
    }
}
