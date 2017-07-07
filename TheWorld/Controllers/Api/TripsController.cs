using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using TheWorld.Models;
using TheWorld.ViewModels;

namespace TheWorld.Controllers.Api
{
    [Route("api/trips")]
    [Authorize]
    public class TripsController : Controller
    {
        private readonly IWorldRepository _repository;
        private ILogger<TripsController> _logger;

        public TripsController(IWorldRepository repository, ILogger<TripsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet("")]
        public IActionResult Get()
        {
            try
            {
                var trips = _repository.GetTripsByUserName(User.Identity.Name);

                var results = Mapper.Map<IEnumerable<TripViewModel>>(trips);
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get all trips: {ex}");

                return BadRequest("Error ocurred");
            }
        }

        [HttpPost("")]
        public async Task<IActionResult> Post([FromBody]TripViewModel tripViewModel)
        {
            if (ModelState.IsValid)
            {
                var newTrip = Mapper.Map<Trip>(tripViewModel);

                newTrip.UserName = User.Identity.Name;

                _repository.AddTrip(newTrip);

                if (await _repository.SaveChangesAsync())
                {
                    var returnTripViewModel = Mapper.Map<TripViewModel>(newTrip);
                    return Created($"api/trips/{tripViewModel.Name}", returnTripViewModel);
                }
            }

            return BadRequest("Failed to save the trip");


        }
    }
}
