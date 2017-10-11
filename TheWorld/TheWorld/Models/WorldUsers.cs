using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace TheWorld.Models
{
  public class WorldUsers : IdentityUser
  {
    public DateTime FirstTrip { get; set; }
  }
}