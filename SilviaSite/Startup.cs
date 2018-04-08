using System.Globalization;
using SilviaSite.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ESmartAdminPanel
{
	public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
			services.AddLocalization(options => options.ResourcesPath = "Resources");
			services.AddMvc()
			  .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
			  .AddDataAnnotationsLocalization();

			//https://dotnetcoretutorials.com/2017/11/02/using-mailkit-send-receive-email-asp-net-core/
			services.AddSingleton<IEmailConfiguration>(Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>());
			services.AddTransient<IEmailService, EmailService>();
		}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

			var supportedCultures = new[]
			{
				new CultureInfo("en-US"),
				new CultureInfo("fr-FR"),
				new CultureInfo("es-US")
			};
			app.UseRequestLocalization(new RequestLocalizationOptions
			{
				DefaultRequestCulture = new RequestCulture("en-US"),
				SupportedCultures = supportedCultures,
				SupportedUICultures = supportedCultures
			});

			app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
