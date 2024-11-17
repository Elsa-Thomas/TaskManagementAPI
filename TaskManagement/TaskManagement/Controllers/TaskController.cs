using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Model;
using TaskManagement.Services.Interfaces;

namespace TaskManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask(TaskCreateDto request)
        {
            var response = await _taskService.CreateTaskAsync(request);
            if (!response.Success)
                return BadRequest(response.Message);

            return Ok(response.Data);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetTasksByUserId(int userId)
        {
            var response = await _taskService.GetTasksByUserIdAsync(userId);
            return Ok(response.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, TaskUpdateDto request)
        {
            var response = await _taskService.UpdateTaskAsync(id, request);
            if (!response.Success)
                return BadRequest(response.Message);

            return Ok(response.Data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var response = await _taskService.DeleteTaskAsync(id);
            if (!response.Success)
                return NotFound(response.Message);

            return Ok(response.Data);
        }
    }
}

