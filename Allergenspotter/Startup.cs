using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Allergenspotter.Models;
using Allergenspotter.Services;
using Allergenspotter.Repositories;
using System;

namespace Allergenspotter
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AllergyContext>(opt =>
              opt.UseSqlServer(Configuration.GetConnectionString("AllergenSpotterData"), opt => 
              {
                  opt.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
              }));
            services.AddScoped<IAllergySpotterService, AllergySpotterService>();
            services.AddScoped<IAllergyDataRepository, AllergyDataRepository>();
            //:TODO make the above services singleton and see how dbcontext scope can be accesed in singletons.
            services.AddControllers();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
