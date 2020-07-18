using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Theatre.TicketOffice.Services;
using Theatre.TicketOffice.ViewModels;

namespace Theatre.TicketOffice.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Constants.Roles.Administrator)]
    public class ShowsController : ControllerBase
    {
        private readonly IShowsService _showsService;
        private readonly IShowTimesService _showTimesService;

        public ShowsController(IShowsService showsService, IShowTimesService showTimesService)
        {
            _showsService = showsService;
            _showTimesService = showTimesService;
        }

        // GET: api/Shows
        [HttpGet]
        public async Task<IEnumerable<ShowViewModel>> GetAllShows()
        {
            return await _showsService.GetAll();
        }

        // GET: api/Shows/names
        [AllowAnonymous]
        [HttpGet("names")]
        public async Task<IEnumerable<string>> GetAllShowNames()
        {
            return (await _showsService.GetAll()).Select(s => s.Name);
        }

        // GET: api/Shows/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetShow([FromRoute] int id)
        {
            var show = await _showsService.GetById(id);

            if (show == null)
            {
                return NotFound();
            }

            return Ok(show);
        }

        // GET: api/Shows/5/ShowTimes
        [HttpGet("{showId}/ShowTimes")]
        public async Task<IActionResult> GetShowTimes([FromRoute] int showId)
        {
            var show = await _showsService.GetById(showId);

            if (show == null)
            {
                return NotFound();
            }

            var showTimes = await _showTimesService.GetByShowId(showId);

            return Ok(showTimes);
        }

        // PUT: api/Shows/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateShow([FromRoute] int id, [FromBody] ShowViewModel show)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != show.ID)
            {
                return BadRequest();
            }

            try
            {
                var updated = await _showsService.Update(show);
                return Ok(updated);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (_showsService.GetById(id) == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // POST: api/Shows
        [HttpPost]
        public async Task<IActionResult> CreateShow([FromBody] ShowViewModel show)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var created = await _showsService.Create(show);

            return CreatedAtAction("GetShow", new { id = show.ID }, created);
        }

        // DELETE: api/Shows/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShow([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var show = await _showsService.GetById(id);
            if (show == null)
            {
                return NotFound();
            }

            await _showsService.Delete(id);

            return Ok();
        }
    }
}