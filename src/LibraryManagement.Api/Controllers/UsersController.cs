using LibraryManagement.Application.Services.Interfaces;
using LibraryManagement.Application.Services;
using LibraryManagement.Contracts.Requests;
using LibraryManagement.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;
using LibraryManagement.Api.Mappings;
using Microsoft.Extensions.Configuration.UserSecrets;
using Asp.Versioning;

namespace LibraryManagement.Api.Controllers
{
    [ApiController]
    [ApiVersion(1.0)]
    public class UsersController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        [HttpPost(ApiEndpoints.Users.Create)]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
        {
            var user = request.MapToUser();

            var created = await _userService.CreateAsync(user);

            if (!created)
            {
                return BadRequest();
            }

            var response = user.MapToResponse();

            return CreatedAtAction(nameof(Get), new { id = response.Id }, response);
        }

        [HttpGet(ApiEndpoints.Users.Get)]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var user = await _userService.GetAsync(id);

            if (user is null)
            {
                return NotFound();
            }

            var response = user.MapToResponse();

            return Ok(response);
        }

        [HttpPut(ApiEndpoints.Users.Update)]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateUserRequest request)
        {
            var user = request.MapToUser(id);

            var updated = await _userService.UpdateAsync(user);

            if (!updated)
            {
                return NotFound();
            }

            var response = user.MapToResponse();

            return Ok(response);
        }

        [HttpDelete(ApiEndpoints.Users.Delete)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deleted = await _userService.DeleteByIdAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpPost(ApiEndpoints.Users.LendBook)]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> LendBook([FromRoute] Guid userId, [FromRoute] Guid bookId)
        {
            var result = await _userService.LendBookAsync(userId, bookId);

            if (result is null)
            {
                return StatusCode(500);
            }

            if (!result.Success)
            {
                return BadRequest(result.Error);
            }

            var response = result.MapToResponse();

            return Ok(response);
        }

        [HttpPost(ApiEndpoints.Users.ReturnBook)]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> ReturnBook([FromRoute] Guid userId, [FromRoute] Guid bookId)
        {
            var result = await _userService.ReturnBookAsync(userId, bookId);

            if (result is null)
            {
                return StatusCode(500);
            }

            if (!result.Success)
            {
                return BadRequest(result.Error);
            }

            var response = result.MapToResponse();

            return Ok(response);
        }

    }
}
