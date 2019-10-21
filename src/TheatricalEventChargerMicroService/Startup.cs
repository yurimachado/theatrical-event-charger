using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Raven.Client.Documents;
using TheatricalEventChargerRepository.DocumentStore;
using TheatricalEventChargerMicroService.Extensions;

namespace TheatricalEventChargerMicroService
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile((env.IsDevelopment() ? $"appsettings.Development.json" : $"appsettings.json"), optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
            this.Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            // Configure RavenDB options and store holder
            services.AddSingleton<IDocumentStore, DocumentStore>(provider => {
                var settings = provider.GetService<IOptions<RavenSettings>>().Value;
                return new DocumentStore { Database = settings.Database, Urls = new[] { settings.Url } };
            });

            services.Configure<RavenSettings>(Configuration.GetSection("Raven"));

            services.AddSingleton<IDocumentStoreHolder, DocumentStoreHolder>();

            services.AddBillCalculationStrategies();

            services.AddRavenDBStores();

            services.AddMediator();

            services.AddMvc()
                .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
