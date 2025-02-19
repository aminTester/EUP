using Microsoft.EntityFrameworkCore;
using BlazorWasmShared.Models;

namespace BlazorWasmAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Professor> Professors { get; set; }
    }
}
