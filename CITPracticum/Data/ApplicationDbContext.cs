using CITPracticum.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CITPracticum.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder builder)
        {
            builder.Properties<DateOnly>()
                .HaveConversion<DateOnlyConverter>()
                .HaveColumnType("date");
        }

        /// <summary>
        /// Converts <see cref="DateOnly" /> to <see cref="DateTime"/> and vice versa.
        /// </summary>
        public class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
        {
            /// <summary>
            /// Creates a new instance of this converter.
            /// </summary>
            public DateOnlyConverter() : base(
                d => d.ToDateTime(TimeOnly.MinValue),
                d => DateOnly.FromDateTime(d))
            { }
        }

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Employer> Employers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<JobPosting> JobPostings { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<PracticumForms> PracticumForms { get; set; }
        public DbSet<FormA> FormAs { get; set; }
        public DbSet<FormB> FormBs { get; set; }
        public DbSet<FormC> FormCs { get; set; }
        public DbSet<FormD> FormDs { get; set; }
        public DbSet<FormFOIP> FormFOIPs { get; set; }
        public DbSet<FormStuInfo> FormStuInfos { get; set; }
        public DbSet<FormExitInterview> FormExitInterviews { get; set; }
        public DbSet<Placement> Placements { get; set; }
        public DbSet<Timesheet> Timesheets { get; set; }
        public DbSet<TimeEntry> TimeEntries { get; set; }
        public DbSet<Application> Applications { get; set; }
    }
}