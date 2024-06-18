using Microsoft.EntityFrameworkCore;
using SuperComUserTasks_.Models;
using Task = SuperComUserTasks_.Models.Task;


namespace SuperComUserTasks_.Services
{
    public class TaskService
    {
        private readonly AppDbContext _context;

        public TaskService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Task>> GetTasksAsync()
        {
            return await _context.Tasks.ToListAsync();
        }

        public async Task<Task> GetTaskByIdAsync(int id)
        {
            return await _context.Tasks.FindAsync(id);
        }

        public async Task<Task> CreateTaskAsync(CreateTaskDto createTaskDto)
        {
            var task = new Task
            {
                UserId = createTaskDto.UserId,
                Name = createTaskDto.Name,
                Description = createTaskDto.Description
            };
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return task;
        }

        public async Task<bool> UpdateTaskAsync(int id, Task task)
        {
            if (id != task.Id)
            {
                return false;
            }

            _context.Entry(task).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return false;
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return true;
        }

        private bool TaskExists(int id)
        {
            return _context.Tasks.Any(e => e.Id == id);
        }
    }
}