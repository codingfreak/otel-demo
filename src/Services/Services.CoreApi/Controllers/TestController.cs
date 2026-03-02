namespace codingfreaks.OtelDemo.Services.CoreApi.Controllers
{
    using Logic.Interfaces;

    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        #region constructors and destructors

        public TestController(IWebLogic webLogic)
        {
            WebLogic = webLogic;
        }

        #endregion

        #region methods

        [HttpGet]
        public async ValueTask<IActionResult> CountWebBytesAsync(string url)
        {
            var result = await WebLogic.CountSiteBytesAsync(url);
            return Ok(result);
        }

        #endregion

        #region properties

        private IWebLogic WebLogic { get; }

        #endregion
    }
}