namespace RegistrationApi.Models
{
    public class Registration
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int IsActive { get; set; } // Corrected property name from IsActie to IsActive
    }
}
