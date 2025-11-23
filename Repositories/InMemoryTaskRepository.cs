using Rengifo_Api.Models;
namespace Rengifo_Api.Repositories
{
    public class InMemoryTaskRepository : ITaskRepository
    {
        private readonly List<TodoTask> _tasks = new();
        private int _nextId = 1;
        public InMemoryTaskRepository()
        {
            // Datos iniciales
            _tasks.Add(new TodoTask
            {
                Id = _nextId++,
                Title = "Tarea de ejemplo 1",
                Description = "Revisar correos",
                IsCompleted = false,
                DueDate = DateTime.Today.AddDays(1)
            });

            _tasks.Add(new TodoTask
            {
                Id = _nextId++,
                Title = "Tarea de ejemplo 2",
                Description = "Preparar presentación",
                IsCompleted = false,
                DueDate = DateTime.Today.AddDays(2)
            });
        }

        public IEnumerable<TodoTask> GetAll() => _tasks;

        public TodoTask? GetById(int id) =>
            _tasks.FirstOrDefault(t => t.Id == id);

        public TodoTask Add(TodoTask task)
        {
            task.Id = _nextId++;
            _tasks.Add(task);
            return task;
        }

        public void Update(TodoTask task)
        {
            var existing = GetById(task.Id);
            if (existing is null) return;

            existing.Title = task.Title;
            existing.Description = task.Description;
            existing.IsCompleted = task.IsCompleted;
            existing.DueDate = task.DueDate;
        }

        public void Delete(int id)
        {
            var existing = GetById(id);
            if (existing is not null)
            {
                _tasks.Remove(existing);
            }
        }

        public bool Exists(int id) =>
            _tasks.Any(t => t.Id == id);
    }
}