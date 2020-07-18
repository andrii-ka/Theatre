using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Theatre.TicketOffice.Controllers
{
    [Route("[controller]/[action]")]
    [Authorize(Roles = Constants.Roles.Administrator)]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        // GET: Admin/ShowTimes/{showId}
        [Route("{showId}")]
        public ActionResult ShowTimes([FromRoute] int showId)
        {
            ViewData["ShowId"] = showId;

            return View();
        }
    }
}