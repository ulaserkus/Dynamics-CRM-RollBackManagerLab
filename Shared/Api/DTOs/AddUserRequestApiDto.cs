namespace Shared.Api.DTOs
{
    public class AddUserRequestApiDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public DateTime BirthDate { get; set; }
        public string AppKey { get; set; }
        public string AppSecret { get; set; }

    }
}
