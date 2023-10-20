using Microsoft.AspNetCore.Identity;

namespace WiseProject.Models
{
    public class User : IdentityUser<int>, IEntity
    {
        public List<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}
