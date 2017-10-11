using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using TheWorld.Api.Controllers.Services;
using TheWorld.Models;
using TheWorld.Services;
using TheWorld.ViewModels;

namespace TheWorld
{
  public class Startup
  {
    private readonly IHostingEnvironment _env;

    private readonly IConfigurationRoot _config;

    public Startup(IHostingEnvironment env)
    {
      _env = env;

      var builder = new ConfigurationBuilder()
        .SetBasePath(_env.ContentRootPath)
        .AddJsonFile("config.json")
        .AddEnvironmentVariables();

      _config = builder.Build();
    }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddSingleton(_config);

      if (_env.IsDevelopment())
        services.AddScoped<IMailService, DebugMailService>();

      services.AddDbContext<WorldContext>();

      services.AddScoped<IWorldRepository, WorldRepository>();

      services.AddTransient<GeoCoordService>();

      services.AddTransient<WorldContextSeedData>();

      services.AddLogging();

      services
        .AddIdentity<WorldUsers, IdentityRole>(config =>
        {
          config.User.RequireUniqueEmail = true;
          config.Password.RequiredLength = 8;
          config.Cookies.ApplicationCookie.LoginPath = "/Auth/Login";
          config.Cookies.ApplicationCookie.Events = new CookieAuthenticationEvents
          {
            OnRedirectToLogin = async context =>
            {
              if (context.Request.Path.StartsWithSegments("/api") 
                  && context.Response.StatusCode == 200)
              {
                context.Response.StatusCode = 401;
              }
              else
              {
                context.Response.Redirect(context.RedirectUri);
              }
              await Task.Yield();
            }
          };
        })
        .AddEntityFrameworkStores<WorldContext>();

      services
        .AddMvc(options =>
        {
          if (_env.IsProduction())
            options.Filters.Add(new RequireHttpsAttribute());
        })
        .AddJsonOptions(config =>
        {
          config.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, WorldContextSeedData seeder, ILoggerFactory factory)
    {
      if (_env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        factory.AddDebug(LogLevel.Information);
      }
      else
        factory.AddDebug(LogLevel.Error);

      app.UseStaticFiles();

      app.UseIdentity();

      Mapper.Initialize(config =>
      {
        config.CreateMap<TripViewModel, Trip>().ReverseMap();
        config.CreateMap<StopViewModel, Stop>().ReverseMap();
      });

      app.UseMvc(config =>
      {
        config.MapRoute(
          name: "Default",
          template: "{controller}/{action}/{id?}",
          defaults: new
          {
            controller = "App",
            action = "Index"
          });
      });

      seeder.EnsureSeedData().Wait();
    }
  }
}
