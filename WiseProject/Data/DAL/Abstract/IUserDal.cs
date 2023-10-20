using WiseProject.Data.DAL.EntityFramework;
using WiseProject.Models;

namespace WiseProject.Data.DAL.Abstract
{
    public interface IUserDal : IEntityRepository<User>
    {
        List<Role> GetClaims(User userInfo);
    }
}
