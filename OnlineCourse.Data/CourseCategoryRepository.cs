using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineCourse.Data.Entities;

namespace OnlineCourse.Data
{
    public class CourseCategoryRepository: ICourseCategoryRepository
    {
        private readonly OnlineCourseDbContext dbContext;

        public CourseCategoryRepository(OnlineCourseDbContext _dbContext)
        {
          dbContext= _dbContext;
        }
        public async Task <CourseCategory> GetById(int id)
        {
            var data= await dbContext.CourseCategories.FindAsync(id);
            return data;
        }

        public async Task<List<CourseCategory>> GetCourseCategories()
        {
            return await dbContext.CourseCategories.ToListAsync();
        }
    }
}
