using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User.Management.API.Models;

namespace User.Management.API.Repositories
{
    public class StayRepository : IStayRepository
    {
        private readonly ApplicationDbContext _context;

        public StayRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Stay>> GetAllStays()
        {
            return await _context.Stays.ToListAsync();
        }

        public async Task<IEnumerable<Stay>> GetStaysByCountry(string country)
        {
            return await _context.Stays.Where(s => s.Country == country).ToListAsync();
        }

        public async Task<IEnumerable<Stay>> GetStaysByCity(string city)
        {
            return await _context.Stays.Where(s => s.City == city).ToListAsync();
        }

        public async Task<IEnumerable<Stay>> GetStaysByMaxGuests(int maxGuests)
        {
            return await _context.Stays.Where(s => s.MaxGuests >= maxGuests).ToListAsync();
        }

        public async Task<IEnumerable<Stay>> GetStaysByName(string name)
        {
            return await _context.Stays.Where(s => s.Name.Contains(name)).ToListAsync();
        }

        public async Task<Stay> GetStayById(int stayId)
        {
            return await _context.Stays.FindAsync(stayId);
        }

        public async Task CreateStay(Stay stay)
        {
            _context.Stays.Add(stay);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateStay(Stay stay)
        {
            _context.Entry(stay).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteStay(int stayId)
        {
            var stay = await _context.Stays.FindAsync(stayId);
            if (stay != null)
            {
                _context.Stays.Remove(stay);
                await _context.SaveChangesAsync();
            }
        }
    }
}
