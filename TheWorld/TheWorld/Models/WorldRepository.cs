using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TheWorld.Models
{
  public class WorldRepository : IWorldRepository
  {
    readonly WorldContext _db;

    public WorldRepository(WorldContext db)
    {
      _db = db;
    }

    public IEnumerable<Trip> GetAllTrips() => _db.Trips.ToList();

    public IEnumerable<Trip> GetTripsByUsername(string username) => 
      _db.Trips
        .Include(t => t.Stops)
        .Where(t => t.UserName == username);

    public void AddTrip(Trip trip) => _db.Add(trip);

    public async Task<bool> SaveChangesAsync() => 
      await _db.SaveChangesAsync() > 0;

    public Trip GetTripByName(string tripName) => 
      _db.Trips
        .Include(t => t.Stops)
        .FirstOrDefault(t => t.Name == tripName);

    public Trip GetUserTripByName(string tripName, string username) => 
      _db
        .Trips
        .Include(t => t.Stops)
        .FirstOrDefault(t => t.Name == tripName && t.UserName == username);

    public void AddStop(string tripName, Stop newStop, string username)
    {
      var trip = GetUserTripByName(tripName, username);

      if (trip == null) return;

      trip.Stops.Add(newStop);
      _db.Stops.Add(newStop);
    }
  }
}
