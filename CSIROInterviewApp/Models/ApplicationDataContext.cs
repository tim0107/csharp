namespace CSIROInterviewApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CSIROInterviewApp.Models;


public class ApplicationDataContext : DbContext
{
    

    // DbSets for all the tables
    public DbSet<User> Users { get; set; }
    public DbSet<Admin> Admins { get; set; }
    public DbSet<Application> Applications { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<University> Universities { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<GPAFilter> GPAFilters { get; set; }
    public DbSet<Invitation> Invitations { get; set; }
    public DbSet<AdminApplication> AdminApplications { get; set; }
    public DbSet<ActionLog> ActionLogs { get; set; }

    public ApplicationDataContext(DbContextOptions<ApplicationDataContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        /* // Relationships

         // User -> Role (One-to-Many)
         modelBuilder.Entity<User>()
             .HasOne(u => u.Role)
             .WithMany(r => r.Users)
             .HasForeignKey(u => u.RoleId);

         // User -> University (One-to-Many)
         modelBuilder.Entity<User>()
             .HasOne(u => u.University)
             .WithMany(uni => uni.Users)
             .HasForeignKey(u => u.UniversityId);

         // User -> Course (One-to-Many)
         modelBuilder.Entity<User>()
             .HasOne(u => u.Course)
             .WithMany(c => c.Applications)
             .HasForeignKey(u => u.CourseId); */

        // Application -> User (One-to-Many)
        modelBuilder.Entity<Application>()
            .HasOne(a => a.User)
            .WithMany(u => u.Applications)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Restrict);


        /*// AdminApplication (Many-to-Many between Admin and Application)
        modelBuilder.Entity<AdminApplication>()
            .HasKey(aa => new { aa.AdminId, aa.ApplicationId });

        modelBuilder.Entity<AdminApplication>()
            .HasOne(aa => aa.Admin)
            .WithMany(a => a.AdminApplications)
            .HasForeignKey(aa => aa.AdminId);

        modelBuilder.Entity<AdminApplication>()
            .HasOne(aa => aa.Application)
            .WithMany(a => a.AdminApplications)
            .HasForeignKey(aa => aa.ApplicationId);

        // Invitation -> User (One-to-Many)
        modelBuilder.Entity<Invitation>()
            .HasOne(i => i.User)
            .WithMany(u => u.Invitations)
            .HasForeignKey(i => i.UserId);

        // ActionLog -> User or Admin 
        modelBuilder.Entity<ActionLog>()
            .HasOne(al => al.User)
            .WithMany()
            .HasForeignKey(al => al.UserId)
            .OnDelete(DeleteBehavior.Restrict); //  this prevents cascade delete

        modelBuilder.Entity<ActionLog>()
            .HasOne(al => al.Admin)
            .WithMany()
            .HasForeignKey(al => al.AdminId)
            .OnDelete(DeleteBehavior.Restrict); */
    }

}


