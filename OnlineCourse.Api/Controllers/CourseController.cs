//using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineCourse.Data.Model;
using OnlineCourse.Service;

namespace OnlineCourse.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService) 
        {
            _courseService = courseService;
        }


        [HttpGet]
        public async Task<ActionResult<List<CourseModel>>> GetAllCourseAsync()
        {
          var courses= await _courseService.GetAllCourseAsync();
            return Ok(courses);
        }

        [HttpGet("Details/{categoryId}")]
        [AllowAnonymous]

        public async Task<ActionResult<CourseDetailModel>> GetCourseDetailAsync(int courseId)
        {
            var courseDetail = await _courseService.GetCourseDetailAsync(courseId);
            if (courseDetail == null)
            {
                return NotFound();
            }
            return Ok(courseDetail);
        }

    }
}
