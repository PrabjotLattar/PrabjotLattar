using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerDbModel
{
    /// <summary>
    /// Db Context class to set up the database and set up communication to the database
    /// </summary>
    public class TrackerDbContext : DbContext
    {
        /// <summary>
        /// Assigning TimeTrackDB name to the database
        /// </summary>
        public TrackerDbContext() : base("TimeTrackDb")
        {
            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<TrackerDbContext>());
        }


        public DbSet<Absent> Absents { get; set; }
        public DbSet<Campus> Campuses { get; set; }
        public DbSet<Description> Descriptions { get; set; }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Message> Messages { get; set; }

        public DbSet<Workhour> Workhours { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //fluent api
            //
           

        }



    }
}
