using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineCourse.Data.Entities;
using OnlineCourse.Data.Model;

namespace OnlineCourse.Service
{
    public interface ICourseCategoryService
    {
        Task<CourseCategoryModel?> GetById(int id);
        Task<List<CourseCategoryModel>> GetCourseCategories();
    }
}
