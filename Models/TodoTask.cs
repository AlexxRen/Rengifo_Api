namespace Rengifo_Api.Models
{
    public class TodoTask
    {
        public int Id { get; set; }                  // Identificador único
        public string Title { get; set; } = null!;   // Título de la tarea
        public string? Description { get; set; }     // Descripción opcional
        public bool IsCompleted { get; set; }        // Estado (completada o no)
        public DateTime? DueDate { get; set; }       // Fecha límite opcional
    }
}