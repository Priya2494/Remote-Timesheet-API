using Microsoft.EntityFrameworkCore;
using TimesheetAPI.Models;

namespace TimesheetAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<TimesheetEntry> TimesheetEntries { get; set; }
    }
}
