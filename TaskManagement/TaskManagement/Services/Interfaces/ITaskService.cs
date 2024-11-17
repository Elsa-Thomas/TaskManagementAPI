using TaskManagement.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskManagement.Services.Interfaces
{
    public interface ITaskService
    {
        Task<ResponseJson<TaskEntry>> CreateTaskAsync(TaskCreateDto request);
        Task<ResponseJson<List<TaskEntry>>> GetTasksByUserIdAsync(int userId);
        Task<ResponseJson<string>> UpdateTaskAsync(int id, TaskUpdateDto request);
        Task<ResponseJson<string>> DeleteTaskAsync(int id);
    }
}
