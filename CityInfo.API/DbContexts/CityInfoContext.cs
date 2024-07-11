using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.DbContexts
{
    public class CityInfoContext : DbContext
    {

        public DbSet<City> Cities { get; set; }

        public DbSet<PointOfInterest> PointOfInterests { get; set; }

        public CityInfoContext(DbContextOptions<CityInfoContext> options)
            : base(options)
        {


        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>().HasData(
                new City("NY City")
                {
                    Id = 1,
                    Description = "NY City"
                },
                new City("Antwerp")
                {
                    Id = 2,
                    Description = "Antwerp"
                },
                new City("Paris")
                {
                    Id = 3,
                    Description = "Paris"
                });

            modelBuilder.Entity<PointOfInterest>().HasData(
                new PointOfInterest("Central Park")
                {
                    Id = 1,
                    CityId = 1,
                    Description = "Central Park"
                },
                new PointOfInterest("Empire State")
                {
                    Id = 2,
                    CityId = 1,
                    Description = "Empire State"
                },
                new PointOfInterest("Cathedral")
                {
                    Id = 3,
                    CityId = 2,
                    Description = "Cathedral"
                },
                new PointOfInterest("Effile Tower")
                {
                    Id = 4,
                    CityId = 3,
                    Description = "Effile Tower"
                });

            base.OnModelCreating(modelBuilder);
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlite("connectionstring");
        //    base.OnConfiguring(optionsBuilder);
        //}
    }
}