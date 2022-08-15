using System.ComponentModel.DataAnnotations;

namespace NetCoreAPIMySQL.Model.Requests
{
    public class RegisterRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
