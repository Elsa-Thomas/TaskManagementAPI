using TaskManagement.Model;
using TaskManagement.Data;
using TaskManagement.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace TaskManagement.Services
{
    public class TaskService : ITaskService
    {
        private readonly TaskMgmtDbContext _context;

        public TaskService(TaskMgmtDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseJson<TaskEntry>> CreateTaskAsync(TaskCreateDto request)
        {
            var user = await _context.Users.FindAsync(request.UserId);
            if (user == null)
                return new ResponseJson<TaskEntry> { Success = false, Message = "User not found." };

            var task = new TaskEntry
            {
                UserId = request.UserId,
                Title = request.Title,
                Description = request.Description,
                DueDate = request.DueDate,
                Status = "Pending"
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return new ResponseJson<TaskEntry> { Data = task };
        }

        public async Task<ResponseJson<List<TaskEntry>>> GetTasksByUserIdAsync(int userId)
        {
            var tasks = await _context.Tasks.Where(t => t.UserId == userId).ToListAsync();
            return new ResponseJson<List<TaskEntry>> { Data = tasks };
        }

        public async Task<ResponseJson<string>> UpdateTaskAsync(int id, TaskUpdateDto request)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
                return new ResponseJson<string> { Success = false, Message = "Task not found." };

            task.Title = request.Title ?? task.Title;
            task.DueDate = request.DueDate != default ? request.DueDate : task.DueDate;
            task.Status = request.Status ?? task.Status;

            await _context.SaveChangesAsync();
            return new ResponseJson<string> { Data = "Task updated successfully." };
        }

        public async Task<ResponseJson<string>> DeleteTaskAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
                return new ResponseJson<string> { Success = false, Message = "Task not found." };

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return new ResponseJson<string> { Data = "Task deleted successfully." };
        }
    }
}
