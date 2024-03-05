namespace Shared.Model.Concrete
{
    public class BasicApiUser : MongoDbDocument
    {
        public string ApiName { get; set; }
        public string UserName { get; set; }
        public string HashedPassword { get; set; }
        public string Salt { get; set; }
    }
}
