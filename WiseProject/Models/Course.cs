using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WiseProject.Models
{
    public class Course : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        //[ForeignKey("AspNetUsers")]
        //public string UserId { get; set; }
        //public User User { get; set; }
        [Required]
        [MaxLength(150)]
        public string Title { get; set; }
        [Required]
        [MaxLength(150)]
        public string Description { get; set; }
        [Required]
        [MaxLength(50)]
        public string Category { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        public virtual List<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public virtual List<Assignment> Assignment { get; set; } = new List<Assignment>();
    }
}
