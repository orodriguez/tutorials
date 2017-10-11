using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TheWorld.Models;
using TheWorld.ViewModels;
using static AutoMapper.Mapper;

namespace TheWorld.Controllers.Api
{
  [Route("api/trips")]
  public class TripsController : Controller
  {
    readonly IWorldRepository _repository;
    private readonly ILogger _logger;

    public TripsController(IWorldRepository repository, 
      ILogger<TripsController> logger)
    {
      _repository = repository;
      _logger = logger;
    }

    [Authorize]
    [HttpGet]
    public IActionResult Get()
    {
      try
      {
        var allTrips = _repository.GetTripsByUsername(User.Identity.Name);
        
        return Ok(Map<IEnumerable<TripViewModel>>(allTrips));
      }
      catch (Exception e)
      {
        _logger.LogError($"Failed to get all trips: {e}");
        return BadRequest("Error occured");
      }
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] TripViewModel trip)
    {
      if (!ModelState.IsValid) return BadRequest(ModelState);

      var newTrip = Map<Trip>(trip);

      newTrip.UserName = User.Identity.Name;

      _repository.AddTrip(newTrip);

      if (!await _repository.SaveChangesAsync())
        return BadRequest("Failed to save changes to the database");

      return Created($"api/trips/{trip.Name}", Map<TripViewModel>(newTrip));
    }
  }

}
