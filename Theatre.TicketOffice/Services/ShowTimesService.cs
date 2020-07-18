using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Theatre.TicketOffice.Data;
using Theatre.TicketOffice.Models;
using Theatre.TicketOffice.ViewModels;

namespace Theatre.TicketOffice.Services
{
    public interface IShowTimesService
    {
        Task<IEnumerable<ShowTimeViewModel>> Create(AddShowTimesParams addParams);
        Task Delete(int id);
        Task<IEnumerable<ShowTimeViewModel>> GetAllFiltered(ShowTimeFilter filter);
        Task<IEnumerable<ShowTimeViewModel>> GetByShowId(int showId);
        Task<ShowTimeViewModel> GetById(int id);
        Task<ShowTimeViewModel> Update(ShowTimeViewModel showTime);
    }

    public class ShowTimesService : IShowTimesService
    {
        private ApplicationDbContext _context;

        public ShowTimesService(ApplicationDbContext context)
        {
            _context = context;
        }

        // if EndDate is provided in params then a serie of weekly events is created
        // otherwise a single event is created
        public async Task<IEnumerable<ShowTimeViewModel>> Create(AddShowTimesParams addParams)
        {
            var entityEntries = new List<EntityEntry<ShowTime>>();
            if (addParams.EndDate.HasValue)
            {
                for (DateTime date = addParams.StartDate; date.Date <= addParams.EndDate.Value.Date; date = date.AddDays(7))
                {
                    var entry = _context.ShowTimes.Add(new ShowTime { ShowID = addParams.ShowID, StartDateUtc = date.ToUniversalTime(), TicketsTotal = addParams.TicketsTotal });
                    entityEntries.Add(entry);
                }
            }
            else
            {
                var entry = _context.ShowTimes.Add(new ShowTime { ShowID = addParams.ShowID, StartDateUtc = addParams.StartDate.ToUniversalTime(), TicketsTotal = addParams.TicketsTotal });
                entityEntries.Add(entry);
            }

            await _context.SaveChangesAsync();

            return entityEntries.Select(e => ToShowTimeViewModel(e.Entity)).ToList();
        }

        public async Task Delete(int id)
        {
            var dbModel = await _context.ShowTimes.FindAsync(id);
            if (dbModel != null)
            {
                _context.ShowTimes.Remove(dbModel); // cascade delete should take care of child records
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<ShowTimeViewModel>> GetAllFiltered(ShowTimeFilter filter)
        {
            // todo: optimize query performance
            return (await _context.ShowTimes
                .Include(s => s.Show)
                .Where(s => s.StartDateUtc.Date == filter.ShowDate.Date)
                .ToListAsync())
                    .Select(ToShowTimeViewModel)
                    .ToList();
        }

        // todo: maybe implement paging here
        public async Task<IEnumerable<ShowTimeViewModel>> GetByShowId(int showId)
        {
            return (await _context.ShowTimes.Where(s => s.ShowID == showId).ToListAsync()).Select(ToShowTimeViewModel).ToList();
        }

        public async Task<ShowTimeViewModel> GetById(int id)
        {
            var dbModel = await _context.ShowTimes.FindAsync(id);
            return ToShowTimeViewModel(dbModel);
        }

        public async Task<ShowTimeViewModel> Update(ShowTimeViewModel showTime)
        {
            var dbModel = ToShowTimeModel(showTime);
            var updated = _context.ShowTimes.Update(dbModel);
            await _context.SaveChangesAsync();

            return ToShowTimeViewModel(updated.Entity);
        }

        // todo: use AutoMapper instead of this
        private ShowTimeViewModel ToShowTimeViewModel(ShowTime s)
        {
            return s == null ?
                null :
                new ShowTimeViewModel { ID = s.ID, ShowID = s.ShowID, ShowName = s.Show?.Name, StartDate = s.StartDateUtc.ToLocalTime(), TicketsTotal = s.TicketsTotal };
        }

        private ShowTime ToShowTimeModel(ShowTimeViewModel s)
        {
            return new ShowTime { ID = s.ID, ShowID = s.ShowID, StartDateUtc = s.StartDate.ToUniversalTime(), TicketsTotal = s.TicketsTotal };
        }
    }
}
