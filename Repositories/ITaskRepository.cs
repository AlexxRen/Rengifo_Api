using Rengifo_Api.Models;

namespace Rengifo_Api.Repositories
{
    public interface ITaskRepository
    {
        IEnumerable<TodoTask> GetAll();
        TodoTask? GetById(int id);
        TodoTask Add(TodoTask task);
        void Update(TodoTask task);
        void Delete(int id);
        bool Exists(int id);
    }
}