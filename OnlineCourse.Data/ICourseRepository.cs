using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineCourse.Data.Model;

namespace OnlineCourse.Data
{
    public   interface ICourseRepository
    {
         Task<List<CourseModel>>GetAllCourseAsync(int ?categoryId=null);
         Task<CourseDetailModel> GetCourseDetailAsync(int courseId);

    }
}
