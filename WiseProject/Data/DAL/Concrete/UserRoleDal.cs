using WiseProject.Data.DAL.Concrete;
using WiseProject.Data;
using WiseProject.Data.DAL.Abstract;
using WiseProject.Data.DAL.EntityFramework;
using WiseProject.Models;

namespace WiseProject.Data.DAL.Concrete
{
    public class UserRoleDal : EfEntityRepositoryBase<UserRole, ApplicationDbContext>, IUserRoleDal
    {
    }
}
