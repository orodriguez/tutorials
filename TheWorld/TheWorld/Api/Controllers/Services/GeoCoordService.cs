﻿using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using TheWorld.Services;

namespace TheWorld.Api.Controllers.Services
{
  public class GeoCoordService
  {
    private ILogger<GeoCoordService> _logger;

    private readonly IConfigurationRoot _config;

    public GeoCoordService(
      ILogger<GeoCoordService> logger, 
      IConfigurationRoot config)
    {
      _logger = logger;
      _config = config;
    }

    public async Task<GeoCoordResults> GetCoordsAsync(string name)
    {
      var result = new GeoCoordResults
      {
        Success = false,
        Message = "Failed to get coordinates"
      };

      var apiKey = _config["Keys:BingKey"];
      var encodedName = WebUtility.UrlEncode(name);
      var url = $"http://dev.virtualearth.net/REST/v1/Locations?q={encodedName}&Key={apiKey}";

      var client = new HttpClient();

      var json = await client.GetStringAsync(url);

      var results = JObject.Parse(json);
      var resources = results["resourceSets"][0]["resources"];
      if (!results["resourceSets"][0]["resources"].HasValues)
      {
        result.Message = $"Could not find '{name}' as a location";
      }
      else
      {
        var confidence = (string)resources[0]["confidence"];
        if (confidence != "High")
        {
          result.Message = $"Could not find a confident match for '{name}' as a location";
        }
        else
        {
          var coords = resources[0]["geocodePoints"][0]["coordinates"];
          result.Latitude = (double) coords[0];
          result.Longitude = (double) coords[1];
          result.Success = true;
          result.Message = "Success";
        }
      }
       
      return result;
    }
  }
}

