namespace TaskManagement.Model
{
    public class DTOs
    {
    }
    public class UserRegisterDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class UserLoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class TaskCreateDto
    {
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
    }

    public class TaskUpdateDto
    {
        public string Title { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; }
    }
}
