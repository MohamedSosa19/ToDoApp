using System.ComponentModel.DataAnnotations;
using TodoApp.Core.Enums;

namespace TodoApi.DTOS
{
    public class CreateTodoDto
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public PriorityLevel Priority { get; set; } = PriorityLevel.Medium;

        public DateTime? DueDate { get; set; }
    }
}
