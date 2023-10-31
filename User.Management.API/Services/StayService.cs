using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using User.Management.API.Models;
using User.Management.API.Repositories;

namespace User.Management.API.Services
{
    public class StayService : IStayService
    {
        private readonly IStayRepository _stayRepository;

        public StayService(IStayRepository stayRepository)
        {
            _stayRepository = stayRepository;
        }

        public async Task<IEnumerable<Stay>> GetAllStays()
        {
            return await _stayRepository.GetAllStays();
        }

        public async Task<IEnumerable<Stay>> GetStaysByCountry(string country)
        {
            return await _stayRepository.GetStaysByCountry(country);
        }

        public async Task<IEnumerable<Stay>> GetStaysByCity(string city)
        {
            return await _stayRepository.GetStaysByCity(city);
        }

        public async Task<IEnumerable<Stay>> GetStaysByMaxGuests(int maxGuests)
        {
            return await _stayRepository.GetStaysByMaxGuests(maxGuests);
        }

        public async Task<IEnumerable<Stay>> GetStaysByName(string name)
        {
            return await _stayRepository.GetStaysByName(name);
        }

        public async Task<Stay> GetStayById(int stayId)
        {
            return await _stayRepository.GetStayById(stayId);
        }

        public async Task CreateStay(Stay stay)
        {
            await _stayRepository.CreateStay(stay);
        }

        public async Task UpdateStay(Stay stay)
        {
            await _stayRepository.UpdateStay(stay);
        }

        public async Task DeleteStay(int stayId)
        {
            await _stayRepository.DeleteStay(stayId);
        }
    }
}
