using CityInfo.API.Models;

namespace CityInfo.API
{
    public class CititesDataStore
    {
        public List<CityDto> Citites { get; set; }

        //public static CititesDataStore current { get; } = new CititesDataStore();

        public CititesDataStore()
        {
            Citites = new List<CityDto>()
            {
                new CityDto()
                {
                    Id=1,
                    Name="New York City",
                    Description = "New York City",
                    PointOfInterests = new List<PointOfInterestDto>()
                        {
                            new PointOfInterestDto()
                            {
                                Id=1,
                                Name="TimeSquare",
                                Description = "TimeSquare"
                            },
                            new PointOfInterestDto()
                            {
                                Id=2,
                                Name="Central Park",
                                Description="Central Park"
                            }
                        }
                },
                new CityDto()
                {
                    Id=2,
                    Name="Wilmington",
                    Description="Wilmington",
                    PointOfInterests = new List<PointOfInterestDto>()
                        {
                            new PointOfInterestDto()
                            {
                                Id=1,
                                Name="Down Town",
                                Description = "Down Town"
                            },
                            new PointOfInterestDto()
                            {
                                Id=2,
                                Name="Beach",
                                Description="Beach"
                            }
                        }
                },
                new CityDto()
                {
                    Id=3,
                    Name="Chennai",
                    Description="Chennai",
                    PointOfInterests = new List<PointOfInterestDto>()
                        {
                            new PointOfInterestDto()
                            {
                                Id=1,
                                Name="Anna Salai",
                                Description = "Anna Salai"
                            },
                            new PointOfInterestDto()
                            {
                                Id=2,
                                Name="T Nagar",
                                Description="T Nagar"
                            }
                        }
                }
            };
        }
    }
}