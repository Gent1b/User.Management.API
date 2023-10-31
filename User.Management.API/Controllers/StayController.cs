using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using User.Management.API.Models;
using User.Management.API.Models.DTO;
using User.Management.API.Services;

namespace User.Management.API.Controllers
{
    [Route("api/stays")]
    [ApiController]
    public class StayController : ControllerBase
    {
        private readonly IStayService _stayService;
        private readonly IUserProfileService _userProfileService;

        public StayController(IStayService stayService, IUserProfileService userProfileService)
        {
            _stayService = stayService;
            _userProfileService = userProfileService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Stay>>> GetAllStays()
        {
            var stays = await _stayService.GetAllStays();
            return Ok(stays);
        }

        [HttpGet("country/{country}")]
        public async Task<ActionResult<IEnumerable<Stay>>> GetStaysByCountry(string country)
        {
            var stays = await _stayService.GetStaysByCountry(country);
            return Ok(stays);
        }

        [HttpGet("city/{city}")]
        public async Task<ActionResult<IEnumerable<Stay>>> GetStaysByCity(string city)
        {
            var stays = await _stayService.GetStaysByCity(city);
            return Ok(stays);
        }

        [HttpGet("maxguests/{maxGuests}")]
        public async Task<ActionResult<IEnumerable<Stay>>> GetStaysByMaxGuests(int maxGuests)
        {
            var stays = await _stayService.GetStaysByMaxGuests(maxGuests);
            return Ok(stays);
        }

        [HttpGet("name/{name}")]
        public async Task<ActionResult<IEnumerable<Stay>>> GetStaysByName(string name)
        {
            var stays = await _stayService.GetStaysByName(name);
            return Ok(stays);
        }

        [HttpGet("{stayId}")]
        public async Task<ActionResult<Stay>> GetStayById(int stayId)
        {
            var stay = await _stayService.GetStayById(stayId);
            if (stay == null)
            {
                return NotFound();
            }
            return Ok(stay);
        }

        [HttpPost]
        public async Task<ActionResult> CreateStay([FromBody] StayDTO stayDTO)
        {
            // Check if the UserProfile with the specified UserProfileId exists
            var userProfile = await _userProfileService.GetUserProfileById(stayDTO.UserProfileId);
            if (userProfile == null)
            {
                return BadRequest("User Profile not found.");
            }

            // Map the DTO to the entity
            var stay = new Stay
            {
                Name = stayDTO.Name,
                Country = stayDTO.Country,
                City = stayDTO.City,
                ImageUrl = stayDTO.ImageUrl,
                MaxGuests = stayDTO.MaxGuests,
                UserProfileId = stayDTO.UserProfileId
            };

            await _stayService.CreateStay(stay);
            return CreatedAtAction("GetStayById", new { stayId = stay.StayId }, stay);
        }


        [HttpPut("{stayId}")]
        public async Task<IActionResult> UpdateStay(int stayId, [FromBody] Stay stay)
        {
            if (stayId != stay.StayId)
            {
                return BadRequest();
            }

            await _stayService.UpdateStay(stay);
            return NoContent();
        }

        [HttpDelete("{stayId}")]
        public async Task<ActionResult> DeleteStay(int stayId)
        {
            var stay = await _stayService.GetStayById(stayId);
            if (stay == null)
            {
                return NotFound();
            }

            await _stayService.DeleteStay(stayId);
            return Ok();
        }
    }
}
