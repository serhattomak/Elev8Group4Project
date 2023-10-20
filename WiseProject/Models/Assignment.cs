using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WiseProject.Models
{
    public class Assignment : IEntity
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("CourseId")]
        public int CourseId { get; set; }
        [Required]
        [MaxLength(150)]
        public string Title { get; set; }
        [Required]
        [MaxLength(150)]
        public string Description { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime DueDate { get; set; }
    }
}
