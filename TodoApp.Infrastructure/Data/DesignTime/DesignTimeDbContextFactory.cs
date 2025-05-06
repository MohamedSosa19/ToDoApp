using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace TodoApp.Infrastructure.Data.DesignTime
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            // Try multiple possible locations for the appsettings.json
            var basePath = Directory.GetCurrentDirectory();
            var applicationPath = Path.Combine(basePath, "..", "TodoApi");

            // Try to find the appsettings.json file
            string settingsPath = null;
            if (File.Exists(Path.Combine(basePath, "appsettings.json")))
            {
                settingsPath = basePath;
            }
            else if (File.Exists(Path.Combine(applicationPath, "appsettings.json")))
            {
                settingsPath = applicationPath;
            }
            else
            {
                throw new FileNotFoundException("Could not find appsettings.json in either location");
            }

            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(settingsPath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
                .Build();

            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var connectionString = configuration.GetConnectionString("todoDB");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception("Could not find connection string 'todoDB'");
            }

            builder.UseSqlServer(connectionString);

            return new ApplicationDbContext(builder.Options);
        }
    }
}