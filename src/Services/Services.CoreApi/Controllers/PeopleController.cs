namespace codingfreaks.OtelDemo.Services.CoreApi.Controllers
{
    using Logic.Interfaces;
    using Logic.Models;

    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class PeopleController : ControllerBase
    {
        #region constructors and destructors

        public PeopleController(IPeopleLogic logic)
        {
            Logic = logic;
        }

        #endregion

        #region methods

        [HttpPost]
        public async ValueTask<IActionResult> AddAsync(PersonCreateModel data)
        {
            var result = await Logic.AddAsync(data);
            return Ok(result);
        }

        [HttpGet]
        public async ValueTask<IActionResult> GetAsync()
        {
            var result = await Logic.GetAsync();
            return Ok(result);
        }

        #endregion

        #region properties

        private IPeopleLogic Logic { get; }

        #endregion
    }
}