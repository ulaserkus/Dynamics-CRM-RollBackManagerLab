namespace Identity.Framework.Core.DTOs
{
    public class AddUserRequestDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public DateTime BirthDate { get; set; }

    }
}
