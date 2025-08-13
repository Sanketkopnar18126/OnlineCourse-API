using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using AutoMapper;
using OnlineCourse.Data;
using OnlineCourse.Data.Model;

namespace OnlineCourse.Service
{
    public interface ICourseService
    {
        Task<List<CourseModel>> GetAllCourseAsync(int? categoryId = null);
        Task<CourseDetailModel> GetCourseDetailAsync(int courseId);
    }

    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        //private readonly IMapper _mapper;


        public CourseService(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
            //_mapper = mapper;
        }
        public Task<List<CourseModel>> GetAllCourseAsync(int? categoryId = null)
        {
            return _courseRepository.GetAllCourseAsync(categoryId);
        }

        public Task<CourseDetailModel> GetCourseDetailAsync(int courseId)
        {
            return _courseRepository.GetCourseDetailAsync(courseId);

        }
    }
}
