using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VakaxaIDServer.Models;
using VakaxaIDServer.Quickstart.Setting;

namespace VakaxaIDServer.Data
{
    public class ApplicationDbContext :  IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }

        public DbSet<LogActionModel> LogActions { get; set; }
        public DbSet<WebSessionModel> WebSessions { get; set; }
        public DbSet<SettingActivityModel> SettingActivities { get; set; }
        public DbSet<EmailQueueModel> EmailContext { get; set; }
        public DbSet<SmsQueueModel> SmsContext { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
