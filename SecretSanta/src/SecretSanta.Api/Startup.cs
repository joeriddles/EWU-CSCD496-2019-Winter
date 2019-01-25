using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services;
using Swashbuckle.AspNetCore.Swagger;

namespace SecretSanta.Api
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
			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

			/* https://stackoverflow.com/questions/38138100/what-is-the-difference-between-services-addtransient-service-addscoped-and-serv
				Transient objects are always different; a new instance is provided to every controller and every service.
				Scoped objects are the same within a request, but different across different requests.
				Singleton objects are the same for every object and every request.

				Kevin: if you don't know what to use: generally use `scoped`
				Does my object do something that's gonna matter if 2 people do something at the same time?
			*/

			/*
				Service type: IGiftService
				Service type must match constructor
				Specifies lifetime
			 */
			services.AddScoped<IGiftService, GiftService>();

			// for this assignment use in memory
			var connection = new SqliteConnection("DataSource=:memory:");
			connection.Open();
			services.AddDbContext<ApplicationDbContext>(build =>
			{
				build.UseSqlite(connection);
				// notice how we're not closing the connection... the thing is gonna die when the process dies.
			});

			// Register the Swagger generator, defining 1 or more Swagger documents
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
			});
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

			// Enable middleware to serve generated Swagger as a JSON endpoint.
			app.UseSwagger();

			// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
			// specifying the Swagger JSON endpoint.
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
			});

			app.UseHttpsRedirection();
			app.UseMvc();
		}
	}
}
