using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineCourse.Service;

namespace OnlineCourse.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseCategoryController : ControllerBase
    {
        private readonly ILogger<CourseCategoryController> _logger;
        private readonly ICourseCategoryService categoryService;

        public CourseCategoryController(ILogger<CourseCategoryController> logger, ICourseCategoryService _categoryService)
        {
            _logger = logger;
            categoryService = _categoryService;
        }

        [HttpGet ("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var category = await categoryService.GetById(id);
            //what if the id is not present?
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await categoryService.GetCourseCategories();
            return Ok(categories);
        }
    }
}
