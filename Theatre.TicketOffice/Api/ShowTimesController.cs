using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Theatre.TicketOffice.Services;
using Theatre.TicketOffice.ViewModels;

namespace Theatre.TicketOffice.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowTimesController : ControllerBase
    {
        private readonly IShowTimesService _showTimesService;
        private readonly IBookingsService _bookingsService;
        private readonly UserManager<IdentityUser> _userManager;

        public ShowTimesController(IShowTimesService showTimesService, IBookingsService bookingsService, UserManager<IdentityUser> userManager)
        {
            _showTimesService = showTimesService;
            _bookingsService = bookingsService;
            _userManager = userManager;
        }

        // GET: api/ShowTimes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetShowTime([FromRoute] int id)
        {
            var showTime = await _showTimesService.GetById(id);

            if (showTime == null)
            {
                return NotFound();
            }

            return Ok(showTime);
        }

        // PUT: api/ShowTimes/5
        [HttpPut("{id}")]
        [Authorize(Roles = Constants.Roles.Administrator)]
        public async Task<IActionResult> UpdateShowTime([FromRoute] int id, [FromBody] ShowTimeViewModel showTime)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != showTime.ID)
            {
                return BadRequest();
            }

            try
            {
                var updated = await _showTimesService.Update(showTime);
                return Ok(updated);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (_showTimesService.GetById(id) == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // POST: api/ShowTimes
        [HttpPost]
        [Authorize(Roles = Constants.Roles.Administrator)]
        public async Task<IActionResult> CreateShowTime([FromBody] AddShowTimesParams addParams)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var created = await _showTimesService.Create(addParams);

            return StatusCode((int)HttpStatusCode.Created, created);
        }

        // POST: api/ShowTimes/filtered
        // Chose to create this as a POST because if filter grows a GET request would become really ugly
        [HttpPost("filtered")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllShowTimes([FromBody] ShowTimeFilter filter)
        {
            var showTimes = await _showTimesService.GetAllFiltered(filter);
            return Ok(showTimes);
        }

        // POST: api/ShowTimes/5/book/10
        [HttpPost("{id}/book/{ticketsToBook}")]
        public async Task<IActionResult> BookTickets([FromRoute] int id, [FromRoute] int ticketsToBook)
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                throw new Exception("Can't find logged user");
            }

            await _bookingsService.BookTickets(id, ticketsToBook, userId);

            return Ok();
        }

        // DELETE: api/ShowTimes/5
        [HttpDelete("{id}")]
        [Authorize(Roles = Constants.Roles.Administrator)]
        public async Task<IActionResult> DeleteShowTime([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var showTime = await _showTimesService.GetById(id);
            if (showTime == null)
            {
                return NotFound();
            }

            await _showTimesService.Delete(id);

            return Ok();
        }
    }
}
