using System.ComponentModel.DataAnnotations;
using TodoApp.Core.Enums;

namespace TodoApi.DTOS
{
    public class UpdateTodoDto
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public TodoStatus Status { get; set; }

        public PriorityLevel Priority { get; set; }

        public DateTime? DueDate { get; set; }
    }
}
