using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Theatre.TicketOffice.Data;
using Theatre.TicketOffice.Models;
using Theatre.TicketOffice.ViewModels;

namespace Theatre.TicketOffice.Services
{
    public interface IShowsService
    {
        // todo: we may want to implement paging here if the number of shows is going to be large
        Task<IEnumerable<ShowViewModel>> GetAll();
        Task<ShowViewModel> GetById(int id);
        Task<ShowViewModel> Create(ShowViewModel show);
        Task<ShowViewModel> Update(ShowViewModel show);
        Task Delete(int id);
    }

    public class ShowsService : IShowsService
    {
        private ApplicationDbContext _context;

        public ShowsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ShowViewModel> Create(ShowViewModel show)
        {
            var dbModel = ToShowModel(show);
            var created = _context.Shows.Add(dbModel);
            await _context.SaveChangesAsync();

            return ToShowViewModel(created.Entity);
        }

        public async Task Delete(int id)
        {
            var dbModel = await _context.Shows.FindAsync(id);
            if (dbModel != null)
            {
                _context.Shows.Remove(dbModel); // cascade delete should take care of child records
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<ShowViewModel>> GetAll()
        {
            return (await _context.Shows.ToListAsync()).Select(ToShowViewModel); 
        }

        public async Task<ShowViewModel> GetById(int id)
        {
            var dbModel = await _context.Shows.FindAsync(id);
            return ToShowViewModel(dbModel);
        }

        public async Task<ShowViewModel> Update(ShowViewModel show)
        {
            var dbModel = ToShowModel(show);
            var updated = _context.Shows.Update(dbModel);
            await _context.SaveChangesAsync();

            return ToShowViewModel(updated.Entity);
        }

        // todo: use AutoMapper instead of this
        private ShowViewModel ToShowViewModel(Show s)
        {
            return s == null ? null : new ShowViewModel { ID = s.ID, Name = s.Name };
        }

        private Show ToShowModel(ShowViewModel s)
        {
            return new Show { ID = s.ID, Name = s.Name };
        }
    }
}
