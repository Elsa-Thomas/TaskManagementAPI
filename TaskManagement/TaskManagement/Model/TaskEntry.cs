using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Model
{
    public class TaskEntry
    {
        [Key]
        public int TaskId { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; }

        public User User { get; set; }
    }
}
