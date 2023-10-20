using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WiseProject.Models
{
    public class Enrollment : IEntity
    {
        public int Id { get; set; }

        [ForeignKey("UserId")]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        [ForeignKey("CourseId")]
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }

        public DateTime EnrollmentDate { get; set; }
    }
}
