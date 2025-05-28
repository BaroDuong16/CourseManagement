using Microsoft.AspNetCore.Mvc;
using backend.Models;
using backend.Repositories;
namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BackendController : Controller
    {
        private readonly ICourseRepositories _courseRepository;
        public BackendController(ICourseRepositories courseRepositories)
        {
            _courseRepository = courseRepositories;
        }

       
    }
}