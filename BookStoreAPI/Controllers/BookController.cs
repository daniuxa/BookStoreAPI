using AutoMapper;
using BookStore_Bll;
using BookStore_Dal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers
{
    [Produces("application/json", "application/xml")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;

        public BookController(IBookService bookService, IMapper mapper)
        {
            this._bookService = bookService ?? throw new ArgumentNullException(nameof(bookService));
            this._mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("api/books/{id}", Name = "GetBook")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await _bookService.GetBookAsync(id);

            return book == null ? NotFound() : Ok(book);
        }

        [HttpGet("api/books")]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            return Ok(await _bookService.GetBooksAsync());
        }

        [HttpPost("api/books")]
        public async Task<ActionResult<Book>> CreateBook(BookDTO book)
        {
            var finalBook = _mapper.Map<Book>(book);
            var addedBook = await _bookService.AddBookAsync(finalBook);
            if (addedBook == null)
            {
                return BadRequest();
            }
            await _bookService.SaveChangesAsync();
            return CreatedAtRoute("GetBook", new { id = addedBook.Id }, addedBook);
        }

        [HttpDelete("api/books/{id}")]
        public async Task<ActionResult> DeleteBook(int id)
        {
            var bookToDelete = await _bookService.GetBookAsync(id);
            if (bookToDelete == null)
            {
                return NotFound();
            }
            _bookService.DeleteBook(bookToDelete);
            await _bookService.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Partially update of book
        /// </summary>
        /// <param name="id">Id of book</param>
        /// <param name="patchDocument">The set of operations to apply to the book</param>
        /// <returns>IActionResult</returns>
        /// <remarks>
        /// Sample request (this request updates the brand's name)
        /// PATCH /books/id
        /// [
        ///     {
        ///         "op": "replace",
        ///         "path": "/title",
        ///         "value": "NewTitle"
        ///     }
        /// ]
        /// </remarks> 
        [HttpPatch("api/books/{id}")]
        public async Task<IActionResult> PartiallyUpdateBook(int id, JsonPatchDocument<BookDTO> patchDocument)
        {
            var bookToUpdate = await _bookService.GetBookAsync(id);
            if (bookToUpdate == null)
            {
                return NotFound();
            }
            var bookToPatch = _mapper.Map<BookDTO>(bookToUpdate);
            patchDocument.ApplyTo(bookToPatch, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!TryValidateModel(bookToPatch))
            {
                return BadRequest(ModelState);
            }
            _mapper.Map(bookToPatch, bookToUpdate);
            await _bookService.SaveChangesAsync();
            return NoContent();
        }
    }
}
