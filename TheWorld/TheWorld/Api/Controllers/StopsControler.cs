using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TheWorld.Api.Controllers.Services;
using TheWorld.Models;
using TheWorld.ViewModels;
using static AutoMapper.Mapper;

namespace TheWorld.Api.Controllers
{
  [Route("/api/trips/{tripName}/stops")]
  public class StopsControler : Controller
  {
    private readonly IWorldRepository _repository;
    private readonly ILogger<StopsControler> _logger;
    private readonly GeoCoordService _coordsService;

    public StopsControler(
      IWorldRepository repository,
      ILogger<StopsControler> logger, 
      GeoCoordService coordsService)
    {
      _repository = repository;
      _logger = logger;
      _coordsService = coordsService;
    }

    [HttpGet]
    public IActionResult Get(string tripName)
    {
      try
      {
        var trip = _repository.GetUserTripByName(tripName, User.Identity.Name);

        var result = Map<IEnumerable<StopViewModel>>(trip.Stops.OrderBy(s => s.Order)
          .ToList());

        return Ok(result);
      }
      catch (Exception e)
      {
        _logger.LogError("Failed to get stops: {0}", e);
      }

      return BadRequest("Failed to get stops");
    }

    [HttpPost]
    public async Task<IActionResult> Post(
      string tripName, 
      [FromBody] StopViewModel vm)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      var newStop = Map<Stop>(vm);

      var result = await _coordsService.GetCoordsAsync(newStop.Name);

      if (!result.Success)
      {
        _logger.LogError(result.Message);
        return BadRequest("Was not able to get coordinates");
      }

      newStop.Latitude = result.Latitude;
      newStop.Longitude = result.Longitude;

      _repository.AddStop(tripName, newStop, User.Identity.Name);

      if (!await _repository.SaveChangesAsync())
        return BadRequest("Failed saving stop");

      return Created($"/api/trips/{tripName}/stops/{newStop.Name}", 
        Map<StopViewModel>(newStop));
    }
  }
}