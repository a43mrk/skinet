using API.Errors;
using Microsoft.AspNetCore.Mvc;

// 51-1 Controller to handle errors for invalid routes,
// and handle the responses that are not being handled by our controllers directly 
namespace API.Controllers
{
    // 54-. don't forget to ignore the routes that don't have an httpmethod specified with
    // ApiExplorerSettinjgs(IgnoreApi = true)
    [Route("errors/{code}")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : BaseApiController
    {
        public IActionResult Error(int code){
            return new ObjectResult(new ApiResponse(code));
        }
    }
}