using Shared.Model.Concrete;

namespace Identity.Framework.Data.Model
{
    public class User : MongoDbDocument
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public ICollection<ApiKey> ApiKeys { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public DateOnly BirthDate { get; set; }
        public DateTime SubscriptionExpireDateUTC { get; set; }
        public bool HasActiveSubscription { get; set; }
        public bool IsActiveUser { get; set; }
    }

    public class ApiKey
    {
        public string Key { get; set; }
        public DateTime ExpiresInUTC { get; set; }
    }
}
