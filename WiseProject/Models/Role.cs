using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace WiseProject.Models
{
    public class Role : IdentityRole<int>, IEntity
    {

    }
}
