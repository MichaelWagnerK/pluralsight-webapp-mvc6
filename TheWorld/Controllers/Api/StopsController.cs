using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TheWorld.Models;
using TheWorld.Services;
using TheWorld.ViewModels;

namespace TheWorld.Controllers.Api
{
    [Route("/api/trips/{tripName}/stops")]
    public class StopsController : Controller
    {
        private IWorldRepository _repository;
        private ILogger<StopsController> _logger;
        private GeoCoordsService _coordsService;

        public StopsController(IWorldRepository repository, 
            ILogger<StopsController> logger,
            GeoCoordsService coordsService)
        {
            _repository = repository;
            _logger = logger;
            _coordsService = coordsService;
        }

        [HttpGet("")]
        public IActionResult Get(string tripName)
        {
            try
            {
                var trip = _repository.GetTripByName(tripName);

                var stops = trip.Stops.OrderBy(s => s.Order).ToList();
                var stopViewModels = Mapper.Map<IEnumerable<StopViewModel>>(stops);

                return Ok(stopViewModels);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get stops: {ex}");
            }

            return BadRequest("Failed to get stops");
        }

        [HttpPost("")]
        public async Task<IActionResult> Post(string tripName, [FromBody]StopViewModel stopViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newStop = Mapper.Map<Stop>(stopViewModel);

                    var result = await _coordsService.GetCoordsAsync(newStop.Name);
                    if (!result.Success)
                    {
                        _logger.LogError(result.Message);
                    }
                    else
                    {

                        newStop.Latitude = result.Latitude;
                        newStop.Longitude = result.Longitude;
                    }


                    _repository.AddStop(tripName, newStop);

                    if (await _repository.SaveChangesAsync())
                    {
                        var returnStopViewModel = Mapper.Map<StopViewModel>(newStop);
                        return Created($"/api/trips/{tripName}/stops/{newStop.Name}", returnStopViewModel);
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to save new stop {ex}");
            }

            return BadRequest("Failed to save new stop");
        }
    }
}
