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

            // 註冊ILogger類別，此類別負責處理錯誤Log寫入
            services.AddSingleton<ILogger>(new Logger(Configuration.GetSection("LogPath").Value));

            // 加入SeeSeaTestContext及連線字串設定
            services.AddDbContext<SeeSeaTestContext>(option => option.UseSqlServer(Configuration.GetConnectionString("SeeSeaTest")));

            // 註冊ISeeSeaTestRepository類別，此類別負責處理與SeeSeaTest資料庫資料存取
            services.AddScoped<ISeeSeaTestRepository>(sp => new SeeSeaTestRepository(sp.GetRequiredService<SeeSeaTestContext>()));

            // 註冊DivingPointDataCenter類別，此類別負責處理DivingPoint相關的資料運算邏輯
            services.AddScoped<IDivingPointDataCenter>(sp => new DivingPointDataCenter(sp.GetService<ISeeSeaTestRepository>()));

            // 註冊RoomDataCenter類別，此類別負責處理Room相關的資料運算邏輯
            services.AddScoped<RoomDataCenter>();

            // 註冊UserDataCenter類別，此類別負責處理User相關的資料運算邏輯
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