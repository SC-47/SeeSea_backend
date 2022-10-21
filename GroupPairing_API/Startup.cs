//-----------------------------------------------------------------------
// <copyright file="Startup.cs" company="Cmoney">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace GroupPairing_API
{
    using GroupPairing_API.DataCenter;
    using GroupPairing_API.Interface;
    using GroupPairing_API.Models.Db;
    using GroupPairing_API.Module;
    using GroupPairing_API.Repository;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.FileProviders;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;
    using System.IO;

    /// <summary>
    /// The settings of the GroupPairingController.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup" /> class.
        /// </summary>
        /// <param name="configuration">The setting information.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Gets the application configuration properties.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"> Services to the container.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SeeSea_Backend", Version = "v1" });
            });

            // ���UILogger���O�A�����O�t�d�B�z���~Log�g�J
            services.AddSingleton<ILogger>(new Logger(Configuration.GetSection("LogPath").Value));

            // �[�JSeeSeaTestContext�γs�u�r��]�w
            services.AddDbContext<SeeSeaTestContext>(option => option.UseSqlServer(Configuration.GetConnectionString("SeeSeaTest")));

            // ���UISeeSeaTestRepository���O�A�����O�t�d�B�z�PSeeSeaTest��Ʈw��Ʀs��
            services.AddScoped<ISeeSeaTestRepository>(sp => new SeeSeaTestRepository(sp.GetRequiredService<SeeSeaTestContext>()));

            // ���UDivingPointDataCenter���O�A�����O�t�d�B�zDivingPoint��������ƹB���޿�
            services.AddScoped<IDivingPointDataCenter>(sp => new DivingPointDataCenter(sp.GetService<ISeeSeaTestRepository>()));

            // ���URoomDataCenter���O�A�����O�t�d�B�zRoom��������ƹB���޿�
            services.AddScoped<RoomDataCenter>();

            // ���UUserDataCenter���O�A�����O�t�d�B�zUser��������ƹB���޿�
            services.AddScoped<UserDataCenter>();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">Configure the application's request pipeline.</param>
        /// <param name="env">Provides the information about the web hosting environment an application is running in.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles(); //for the wwwroot folder
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), @"image")),
                RequestPath = new PathString("/image")
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GroupPairing_API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseMiddleware<RequestLogMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}