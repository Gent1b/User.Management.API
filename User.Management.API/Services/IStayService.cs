using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using User.Management.API.Models;

namespace User.Management.API.Services
{
    public interface IStayService
    {
        Task<IEnumerable<Stay>> GetAllStays();
        Task<IEnumerable<Stay>> GetStaysByCountry(string country);
        Task<IEnumerable<Stay>> GetStaysByCity(string city);
        Task<IEnumerable<Stay>> GetStaysByMaxGuests(int maxGuests);
        Task<IEnumerable<Stay>> GetStaysByName(string name);
        Task<Stay> GetStayById(int stayId);
        Task CreateStay(Stay stay);
        Task UpdateStay(Stay stay);
        Task DeleteStay(int stayId);
    }
}
