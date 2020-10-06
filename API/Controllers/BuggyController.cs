using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using API.Errors;
using Microsoft.AspNetCore.Authorization;

// 49-1 Controller for testing errors
namespace API.Controllers
{
    public class BuggyController : BaseApiController
    {
        // 49-2 inject StoreContext at constructor
        // alert: this controller is only for testing purposes!
        private readonly StoreContext _context;
        public BuggyController(StoreContext context)
        {
            _context = context;
        }

        [HttpGet("testauth")]
        [Authorize]
        public ActionResult<string> GetSecretText(){
            return "secret stuff";
        }

        [HttpGet("Notfound")]
        public ActionResult GetNotFoundRequest()
        {
            var thing = _context.Products.Find(42); // Null is returned from EF if item do not exists

            // Always guard yourself from null.
            if(thing == null){
                // avoid null reference exceptions are relly bad :(
                // 50-2 Use the custom error class we made.
                return NotFound(new ApiResponse(404));
            }

            return Ok();
        }
        [HttpGet("servererror")]
        public ActionResult GetServerError()
        {
            var thing = _context.Products.Find(42);
            var thingToReturn = thing.ToString();

            return Ok();
        }

        [HttpGet("badrequest")]
        public ActionResult GetBadRequest()
        {
            // return default BadRequest method.
            // 50-2 Use the custom error class we made.
            return BadRequest(new ApiResponse(400));
        }

        [HttpGet("badrequest/{id}")]
        public ActionResult GetNotFoundRequest(int id)
        {
            return Ok();
        }
    }
}