using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperComUserTasks_.Models;
using SuperComUserTasks_.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task = SuperComUserTasks_.Models.Task;

namespace SuperComUserTasks_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly TaskService _taskService;

        public TasksController(TaskService taskService)
        {
            _taskService = taskService;
        }

        // GET: api/Tasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Task>>> GetTasks()
        {
            var tasks = await _taskService.GetTasksAsync();
            return Ok(tasks);
        }

        // GET: api/Tasks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Task>> GetTask(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            return Ok(task);
        }

        // POST: api/Tasks
        [HttpPost]
        public async Task<ActionResult<Task>> PostTask(CreateTaskDto createTaskDto)
        {
            var task = await _taskService.CreateTaskAsync(createTaskDto);
            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
        }

        // PUT: api/Tasks/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTask(int id, Task task)
        {
            var success = await _taskService.UpdateTaskAsync(id, task);
            if (!success)
            {
                return BadRequest();
            }
            return NoContent();
        }

        // DELETE: api/Tasks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var success = await _taskService.DeleteTaskAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
