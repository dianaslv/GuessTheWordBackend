using Exam.Web.Core.Services.Interfaces;
using Exam.Web.Infrastructure.Data.Context;
using Exam.Web.Infrastructure.IOC;
using Exam.Web.Infrastructure.Network.Hubs.Implementation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Exam.Web.Presentation
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        private const string CONNECTION_STRING_PATH = "ConnectionStrings:Exam";
        private const string ALLOWED_HOSTS = "TestEndpoints:Allowed";
        private const string MIGRATION_ASSEMBLY = "Exam.Web.Presentation";
        private const string REPOSITORIES_NAMESPACE = "Exam.Web.Infrastructure.Data.Repositories.Implementations";
        private const string SERVICES_NAMESPACE = "Exam.Web.Core.Services.Implementations";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<KestrelServerOptions>(options => { options.AllowSynchronousIO = true; });
            services.Configure<IISServerOptions>(options => { options.AllowSynchronousIO = true; });
            services.AddSignalR();
            services.InjectMySqlDbContext<DataContext>(Configuration[CONNECTION_STRING_PATH], MIGRATION_ASSEMBLY);
            services.InjectForNamespace(REPOSITORIES_NAMESPACE);
            services.InjectForNamespace(SERVICES_NAMESPACE); 
            services.AddCors(options =>
            {
                options.AddPolicy("_allow", builder =>
                {
                    builder
                        .WithOrigins(Configuration[ALLOWED_HOSTS].Split(","))
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app)
        {
            
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("_allow");

            app.UseEndpoints(endpoints => { 
                endpoints.MapControllers();
                endpoints.MapHub<SignalRHub>("/hub");
            });
        }
    }
}