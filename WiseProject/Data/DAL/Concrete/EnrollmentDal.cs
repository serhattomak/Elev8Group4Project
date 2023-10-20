using WiseProject.Data.DAL.Concrete;
using WiseProject.Data;
using WiseProject.Data.DAL.Abstract;
using WiseProject.Data.DAL.EntityFramework;
using WiseProject.Models;

namespace WiseProject.Data.DAL.Concrete
{
    public class EnrollmentDal : EfEntityRepositoryBase<Enrollment, ApplicationDbContext>, IEnrollmentDal
    {
    }
}
