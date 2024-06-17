using System.ComponentModel.DataAnnotations;

namespace SuperComUserTasks_.Models
{
    public class CreateUserDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
