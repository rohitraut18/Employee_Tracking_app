namespace EmployeeTrackingWebApp.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
            : base("name=ApplicationDbContext")
        {
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }

        public DbSet<AppRole> AppRoles { get; set; } 
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Tracking> Trackings { get; set; }
    }
}
