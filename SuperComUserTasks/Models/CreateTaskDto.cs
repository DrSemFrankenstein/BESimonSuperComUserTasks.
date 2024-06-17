using System.ComponentModel.DataAnnotations;

namespace SuperComUserTasks_.Models
{
    public class CreateTaskDto
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
