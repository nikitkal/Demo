using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Demo_WebAPI.Data
{

    public class AppDbContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public AppDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseNpgsql(_configuration.GetConnectionString("WebApiDatabase"));
        }
       public virtual DbSet<Employee> Employee { get; set; }
    }
}
