using CommandsService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace CommandsService.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options):base(options) 
        {
        }
        public DbSet<Models.Platform> Platforms { get; set; }
        public DbSet<Models.Command> Commands { get; set; }
    }
}
