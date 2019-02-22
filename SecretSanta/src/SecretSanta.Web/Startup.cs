using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SecretSanta.Web
{
	public class Startup
	{
		private IConfiguration Configuration { get; }
		public Startup(IConfiguration configuration) => Configuration = configuration;

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc();
			services.AddHttpClient("SecretSantaApi", config =>
			{
				config.BaseAddress = new Uri(Configuration.GetSection("SecretSantaApi").GetValue<String>("BaseUri"));
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();

			app.UseStaticFiles();

			app.UseMvcWithDefaultRoute();
		}
	}
}
