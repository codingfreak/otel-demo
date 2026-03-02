namespace codingfreaks.OtelDemo.Services.CoreApi.Controllers
{
    using Logic.Models;

    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class PeopleController : ControllerBase
    {
        #region methods

        [HttpGet]
        public async ValueTask<IActionResult> GetAsync()
        {
            var result = Array.Empty<Person>();
            await Task.Yield();
            return Ok(result);
        }

        #endregion
    }
}