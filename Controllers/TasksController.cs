using Microsoft.AspNetCore.Mvc;
using Rengifo_Api.Models;
using Rengifo_Api.Repositories;
namespace Rengifo_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskRepository _repository;

        public TasksController(ITaskRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Lista todas las tareas.
        /// </summary>
        [HttpGet]
        public ActionResult<IEnumerable<TodoTask>> GetAll()
        {
            var tasks = _repository.GetAll();
            return Ok(tasks);
        }

        /// <summary>
        /// Obtiene una tarea por su ID.
        /// </summary>
        /// <param name="id">Identificador de la tarea</param>
        [HttpGet("{id:int}")]
        public ActionResult<TodoTask> GetById(int id)
        {
            var task = _repository.GetById(id);
            if (task is null)
                return NotFound(new { message = $"No se encontró la tarea con id {id}" });

            return Ok(task);
        }

        /// <summary>
        /// Crea una nueva tarea.
        /// </summary>
        /// <param name="task">Datos de la tarea</param>
        [HttpPost]
        public ActionResult<TodoTask> Create([FromBody] TodoTask task)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = _repository.Add(task);

            // Devuelve 201 Created con la ruta del recurso
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        /// <summary>
        /// Actualiza una tarea existente.
        /// </summary>
        /// <param name="id">Id de la tarea a actualizar</param>
        /// <param name="task">Datos actualizados</param>
        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] TodoTask task)
        {
            if (id != task.Id)
                return BadRequest(new { message = "El id de la ruta no coincide con el id del cuerpo." });

            if (!_repository.Exists(id))
                return NotFound(new { message = $"No se encontró la tarea con id {id}" });

            _repository.Update(task);
            return NoContent(); // 204
        }

        /// <summary>
        /// Elimina una tarea por ID.
        /// </summary>
        /// <param name="id">Id de la tarea a eliminar</param>
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            if (!_repository.Exists(id))
                return NotFound(new { message = $"No se encontró la tarea con id {id}" });

            _repository.Delete(id);
            return NoContent();
        }
    }
}