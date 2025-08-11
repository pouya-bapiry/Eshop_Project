namespace Eshop.Application.Utilities
{
    public class AuthViewModel
    {
        public long Id { get; set; }
        public long RoleId { get; set; }
        public string Role { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Picture { get; set; }
        public string Mobile { get; set; }


        public AuthViewModel()
        {
        }

        public AuthViewModel(long roleId, string role, string firstName, string lastName, string username, string picture, string mobile)
        {
            RoleId = roleId;
            Role = role;
            FirstName = firstName;
            LastName = lastName;
            Username = username;
            Picture = picture;
            Mobile = mobile;
        }
    }
}