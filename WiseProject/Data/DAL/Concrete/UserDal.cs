using WiseProject.Data.DAL.Concrete;
using WiseProject.Data;
using WiseProject.Data.DAL.Abstract;
using WiseProject.Data.DAL.EntityFramework;
using WiseProject.Models;

namespace WiseProject.Data.DAL.Concrete
{
    public class UserDal : EfEntityRepositoryBase<User, ApplicationDbContext>, IUserDal
    {
        public List<Role> GetClaims(User user)
        {
            using (var context = new ApplicationDbContext())
            {
               var result = from roles in context.Roles
                            join userRoles in context.UserRoles
                                on roles.Id equals userRoles.RoleId
                            where userRoles.UserId == user.Id
                            select new Role { Id = roles.Id, Name = roles.Name };

                return result.ToList();
            }
        }
    }
}
