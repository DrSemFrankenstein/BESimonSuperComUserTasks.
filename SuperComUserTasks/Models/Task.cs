using System.ComponentModel.DataAnnotations;

namespace SuperComUserTasks_.Models
{
    public class Task
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(200)]
        public string Description { get; set; }
    }
}
