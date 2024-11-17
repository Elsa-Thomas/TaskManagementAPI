using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Model
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedDate { get; set; }

        public ICollection<TaskEntry> Tasks { get; set; }
    }
}
