//using HTP.App.Users;
//using HTP.App.Users.Queries.GetUser;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;

//namespace HPT.Api.Controllers;

//[ApiController]
//[Route("api/[controller]")]
//[Authorize]
//public class UsersController : ControllerBase
//{
//    private readonly GetUserQueryHandler _getUserHandler;
//    private readonly IUserContext _currentUserService;

//    public UsersController(
//        GetUserQueryHandler getUserHandler,
//        IUserContext currentUserService)
//    {
//        _getUserHandler = getUserHandler;
//        _currentUserService = currentUserService;
//    }

//    [HttpGet("{id:guid}")]
//    public async Task<IActionResult> GetUser(Guid id, CancellationToken ct)
//    {
//        var query = new GetUserQuery(id);
//        var result = await _getUserHandler.Handle(query, ct);

//        if (result.IsFailure)
//        {
//            return NotFound(new { error = result.Error.Message });
//        }

//        return Ok(result.Value);
//    }

//    [HttpGet("me")]
//    public IActionResult GetCurrentUser()
//    {
//        if (!_currentUserService.IsAuthenticated)
//        {
//            return Unauthorized();
//        }

//        var currentUserResult = _currentUserService.GetCurrentUser();
        
//        if (currentUserResult.IsFailure)
//        {
//            return Unauthorized(new { error = currentUserResult.Error.Message });
//        }

//        return Ok(currentUserResult.Value);
//    }
//}
