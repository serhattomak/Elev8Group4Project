using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WiseProject.Models
{
    public class Assignment : IEntity
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Course")]
        public int CourseId { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        [Required]
        [MaxLength(150)]
        public string Title { get; set; }
        public string CourseTitle { get; set; }
        public Course Course { get; set;}

        [Required]
        [MaxLength(150)]
        public string Description { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime DueDate { get; set; }
    }
}
