﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Dto;
using Application.Dto.QueryParams;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookCrossingBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly IWishListService _wishListService;

        public BooksController(IBookService bookService, IWishListService wishListService)
        {
            _bookService = bookService;
            _wishListService = wishListService;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<PaginationDto<BookGetDto>>> GetAllBooksAsync([FromQuery]BookQueryParams parameters)
        {
            return await _bookService.GetAllAsync(parameters);
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookGetDto>> GetBook([FromRoute] int id)
        {
            var book = await _bookService.GetByIdAsync(id);
            if (book == null)
                return NotFound();
            return Ok(book);
        }

        // POST: api/Books
        [HttpPost]
        public async Task<ActionResult<BookPutDto>> PostBookAsync([FromForm] BookPostDto bookDto)
        {
            var insertedBook = await _bookService.AddAsync(bookDto);
            return CreatedAtAction("GetBook", new { id = insertedBook.Id }, insertedBook);
        }

        // PUT: api/Books/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBookAsync([FromRoute] int id, [FromForm] BookPutDto bookDto)
        {
            if (id != bookDto.Id)
            {
                return BadRequest();
            }

            var isBookUpdated = await _bookService.UpdateAsync(bookDto);
            if (!isBookUpdated)
            {
                return BadRequest();
            }

            return NoContent();
        }
        
        [HttpPut("{id}/deactivate")]
        public async Task<IActionResult> DeactivateBookAsync([FromRoute] int id)
        {
            var isBookDeactivated = await _bookService.DeactivateAsync(id);
            if (!isBookDeactivated)
            {
                return BadRequest();
            }
            return NoContent();
        }

        [HttpPut("{id}/activate")]
        public async Task<IActionResult> ActivateBookAsync([FromRoute] int id)
        {
            var isBookActivated = await _bookService.ActivateAsync(id);
            if (!isBookActivated)
            {
                return BadRequest();
            }
            return NoContent();
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]

        public async Task<ActionResult> DeleteBookAsync([FromRoute] int id)
        {
            var isBookRemoved = await _bookService.RemoveAsync(id);
            if (!isBookRemoved)
            {
                return NotFound();
            }
            return Ok();
        }

        [HttpGet("registered")]
        public async Task<ActionResult<PaginationDto<BookGetDto>>> GetRegisteredBooksAsync([FromQuery]BookQueryParams parameters)
        {
            return await _bookService.GetRegisteredAsync(parameters);
        }

        [HttpGet("current")]
        public async Task<ActionResult<PaginationDto<BookGetDto>>> GetCurrentOwnedBooksAsync([FromQuery]BookQueryParams parameters)
        {
            return await _bookService.GetCurrentOwned(parameters);
        }

        [HttpGet("current/{id}")]
        public async Task<ActionResult<List<BookGetDto>>> GetCurrentOwnedBooksByIdAsync(int id)
        {
            return await _bookService.GetCurrentOwnedById(id);
        }

        [HttpGet("read")]
        public async Task<ActionResult<PaginationDto<BookGetDto>>> GetReadBooksAsync([FromQuery]BookQueryParams parameters)
        {
            return await _bookService.GetReadBooksAsync(parameters);
        }

        // POST: api/Books
        [HttpPost("fromcompany")]
        public async Task<ActionResult<BookPutDto>> PostBookFromCompany([FromForm] BookPostDto bookDto)
        {
            var insertedBook = await _bookService.AddAsync(bookDto);
            await _wishListService.AddWishAsync(insertedBook.Id);
            return CreatedAtAction("GetBook", new { id = insertedBook.Id }, insertedBook);
        }
        
        
        [HttpPost("rating")]
        public async Task<ActionResult<bool>> SetRating([FromQuery]BookRatingQueryParams ratingQueryParams)
        {
            return await _bookService.SetRating(ratingQueryParams);
        }

        [HttpGet("rating/{bookId}/user/{userId}")]
        public async Task<ActionResult<double>> GetRating(int bookId, int userId)
        {
            return await _bookService.GetRating(bookId, userId);
        }
    }
}
