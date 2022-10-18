//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Cmoney">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace GroupPairing_API
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;

    /// <summary>
    /// The Main program of this project.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The main function of the project.
        /// </summary>
        /// <param name="args">The Regular expression.</param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Construct and initialize the host.
        /// </summary>
        /// <param name="args">The Regular expression.</param>
        /// <returns>The host of the Web API project.</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
