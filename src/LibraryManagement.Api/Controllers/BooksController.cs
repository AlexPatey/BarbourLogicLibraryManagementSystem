using Asp.Versioning;
using LibraryManagement.Api.Mappings;
using LibraryManagement.Application.Services.Interfaces;
using LibraryManagement.Contracts.Requests;
using LibraryManagement.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Api.Controllers
{
    [ApiController]
    [ApiVersion(1.0)]
    public class BooksController(IBookService bookService) : ControllerBase
    {
        private readonly IBookService _bookService = bookService;

        [HttpPost(ApiEndpoints.Books.Create)]
        [ProducesResponseType(typeof(BookResponse), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] CreateBookRequest request)
        {
            var book = request.MapToBook();

            var created = await _bookService.CreateAsync(book);

            if (!created)
            {
                return BadRequest();
            }

            var response = book.MapToResponse();

            return CreatedAtAction(nameof(Get), new { id = response.Id }, response);
        }

        [HttpGet(ApiEndpoints.Books.Get)]
        [ProducesResponseType(typeof(BookResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var book = await _bookService.GetAsync(id);

            if (book is null)
            {
                return NotFound();
            }

            var response = book.MapToResponse();

            return Ok(response);
        }

        [HttpGet(ApiEndpoints.Books.GetAll)]
        [ProducesResponseType(typeof(BooksResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var books = await _bookService.GetAllAsync();

            var response = books.MapToResponse();

            return Ok(response);
        }

        [HttpPut(ApiEndpoints.Books.Update)]
        [ProducesResponseType(typeof(BookResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateBookRequest request)
        {
            var book = request.MapToBook(id);

            var updated = await _bookService.UpdateAsync(book);

            if (!updated)
            {
                return NotFound();
            }

            var response = book.MapToResponse();

            return Ok(response);
        }

        [HttpDelete(ApiEndpoints.Books.Delete)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deleted = await _bookService.DeleteByIdAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return Ok();
        }

    }
}
