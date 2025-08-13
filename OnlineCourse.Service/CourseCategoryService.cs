using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineCourse.Data;
using OnlineCourse.Data.Model;

namespace OnlineCourse.Service
{
    public class CourseCategoryService : ICourseCategoryService
    {
        private readonly ICourseCategoryRepository courseCategoryService;
        public CourseCategoryService(ICourseCategoryRepository _courseCategoryRepo) 
        {
          courseCategoryService = _courseCategoryRepo;
        }
        public async Task<CourseCategoryModel?> GetById(int id)
        {
           var data= await courseCategoryService.GetById(id);
            return new CourseCategoryModel()
            {
                CategoryName = data.CategoryName,
                CategoryId = data.CategoryId,
                Description = data.Description

            };
            return null;
        }

        public async Task<List<CourseCategoryModel>> GetCourseCategories()
        {
          var categories= await  courseCategoryService.GetCourseCategories();
          return  categories.Select(c=>new CourseCategoryModel()
            {
                CategoryName = c.CategoryName,
                CategoryId = c.CategoryId,
                Description = c.Description
            }).ToList();
        }
    }
}
