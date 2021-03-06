﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Dto;
using Application.Dto.QueryParams;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BookCrossingBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly IGenreService _genreService;
        private readonly ILogger _logger;

        public GenreController(IGenreService genreService, ILogger<GenreController> logger)
        {
            _genreService = genreService;
            _logger = logger;
        }

        [HttpGet("{id:min(1)}")]
        public async Task<ActionResult<GenreDto>> GetGenre(int id)
        {
            _logger.LogInformation("Getting genre {id}", id);
            var genre = await _genreService.GetById(id);
            if (genre == null)
            {
                _logger.LogWarning("GetGenre({Id}) NOT FOUND", id);
                return NotFound();
            }
            return Ok(genre);
        }

        // GET: api/Genres
        [HttpGet]
        public async Task<ActionResult<List<GenreDto>>> GetAllGenres()
        {
            _logger.LogInformation("Getting all genres");
            return Ok(await _genreService.GetAll());
        }


        // PUT: api/Genre
        [HttpPut]
        public async Task<IActionResult> PutGenre(GenreDto genreDto)
        {
            _logger.LogInformation("Update genre {GenreDto}", genreDto);
            var updated = await _genreService.Update(genreDto);
            if (!updated)
            {
                _logger.LogWarning("Update genre ({GenreDto}) NOT FOUND", genreDto);
                return NotFound();
            }
            return NoContent();
        }

        // POST: api/Genre
        [HttpPost]
        public async Task<ActionResult<GenreDto>> PostGenre([FromBody] GenreDto genreDto)
        {
            _logger.LogInformation("Post genre {GenreDto}", genreDto);
            var insertedGenre = await _genreService.Add(genreDto);
            return CreatedAtAction("GetGenre", new { id = insertedGenre.Id }, insertedGenre);
        }

        // DELETE: api/Genre/id
        [HttpDelete("{id:min(1)}")]
        public async Task<IActionResult> DeleteGenre(int id)
        {
            _logger.LogInformation("Delete genre {Id}", id);
            var genre = await _genreService.Remove(id);
            if (genre == false)
            {
                _logger.LogWarning("Delete genre ({Id}) NOT FOUND", id);
                return NotFound();
            }
            return Ok();
        }
        [HttpGet("paginated")]
        public async Task<ActionResult<PaginationDto<GenreDto>>> GetAllGenres([FromQuery] FullPaginationQueryParams fullPaginationQuery)
        {
            _logger.LogInformation("Getting all paginated genres");
            return await _genreService.GetAll(fullPaginationQuery);
        }
    }
}