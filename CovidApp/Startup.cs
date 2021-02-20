using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CovidApp.Persistence;
using CovidApp.Persistence.ModelDb;
using CovidApp.Services.Interface;
using CovidApp.Services.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace CovidApp
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
            services.Configure<CovidDatabaseSettings>(Configuration.GetSection(nameof(CovidDatabaseSettings)));

            services.AddSingleton<ICovidDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<CovidDatabaseSettings>>().Value);

            services.AddHttpClient<IHttpClientService, HttpClientService>("covid", client =>
            {
                client.BaseAddress = new Uri("https://api.covid19api.com");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            services.AddSingleton<IDatabaseService, DatabaseService>();

            services.AddSingleton<ICovidService, CovidService>();

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
