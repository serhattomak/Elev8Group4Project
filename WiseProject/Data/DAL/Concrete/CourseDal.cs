﻿using WiseProject.DAL.EntityFramework;
using WiseProject.Data;
using WiseProject.Models;

namespace WiseProject.DAL.Abstract
{
    public class CourseDal : EfEntityRepositoryBase<Course, ApplicationDbContext>, ICourseDal
    {
    }
}
