using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Mudanza.Api.Aplication.Main;
using Mudanza.Api.Aplication.Interface;
using Mudanza.Api.Infraestructure;
using Mudanza.Api.Infraestructure.Interface;
using Mudanza.Api.Infraestructure.Repository;
using Swashbuckle.Swagger;
using Microsoft.OpenApi.Models;

namespace Mudanza.Api.Core
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
            services.AddCors();            
            services.AddControllers();
            services.AddDbContext<DbContextMudanza>(options => options.UseSqlServer(Configuration.GetConnectionString("Conexion")));
            services.AddTransient<IMudanzaAplication, MudanzaAplication>();
            services.AddTransient<ILogRespository, LogRespository>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1.0",
                    Title = "Documentación API",
                    Contact = new OpenApiContact
                    {
                        Name = "Carlos Arturo Roca Muñoz",
                        Email = "charlesrock96@gmail.com"
                    },
                });
            });
            
        }
        
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(options =>
            {
                options.WithOrigins("http://localhost:3000");
                options.AllowAnyMethod();
                options.AllowAnyHeader();
            });

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

            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Documentación API");
            });
        }
    }
}
