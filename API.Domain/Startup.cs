using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DATA.Domain.Models;
using DATA.Domain.Seeds; 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions; 
using Microsoft.OpenApi.Models; 

namespace API.Domain
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
            services.AddControllers();
            services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(Configuration["Conexao"])); 
            services.AddSingleton<IConfiguration>(Configuration); 
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "CRUD API",
                    Version = "v1",
                    Description = "Backend-API para teste de desenvolvimento",
                    Contact = new OpenApiContact
                    {
                        Name = "Rafael Rodrigues da Silva",
                        Url = new System.Uri("https://github.com/rafaelrodriguesdasilva/wpf-teste"),
                        Email = "rafaa.cfc@gmail.com"
                    }

                });
                string swaggerXml = PlatformServices.Default.Application.ApplicationBasePath;
                swaggerXml = Path.Combine(swaggerXml, "swagger.xml");
                c.IncludeXmlComments(swaggerXml);
            }); 
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json",
                    "Wpf teste API - Backend");
                c.RoutePrefix = string.Empty;
            });
            //UpdateDatabase(app);
            //Como nesta aplicacao estou usando um BD local
            //Foi automatizada a aplicacao das migrations
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        private static void UpdateDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<AppDbContext>())
                {
                    context.Database.Migrate();
                    new InitialDebugSeeder(context).SeedData();
                }
            }
        }
    }
}
