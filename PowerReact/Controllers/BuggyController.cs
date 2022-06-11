using Microsoft.AspNetCore.Mvc;

namespace PowerReact.Controllers;

public class BuggyController : BaseApiController
{
    [HttpGet("not-found")]
    public ActionResult GetNotFound()
    {
        return NotFound(); // returns 404
    }

    [HttpGet("bad-request")]
    public ActionResult GetBadRequest()
    {
        return BadRequest(new ProblemDetails{Title = "This is a bad request"});
    }

    [HttpGet("unauthorized")]
    public ActionResult GetUnauthorized()
    {
        return Unauthorized(); // returns 404
    }

    [HttpGet("validation-error")]
    public ActionResult GetValidationError()
    {
        ModelState.AddModelError("Problem1", "This is the first error"); // returns 404
        ModelState.AddModelError("Problem2", "This is the second error"); // returns 404
        return ValidationProblem(); // returns all errors in ModelState (related to validations)
    }

    [HttpGet("server-error")]
    public ActionResult GetServerError()
    {
        throw new Exception("This is a server error");
    }
}