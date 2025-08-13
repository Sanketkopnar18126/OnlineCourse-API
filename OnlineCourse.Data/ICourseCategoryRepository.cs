using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineCourse.Data.Entities;

namespace OnlineCourse.Data
{
    public interface ICourseCategoryRepository
    {
        Task<CourseCategory?> GetById(int id);
        Task<List<CourseCategory>> GetCourseCategories();
    }
}
 