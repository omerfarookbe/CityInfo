using CityInfo.API.Entities;

namespace CityInfo.API.Services
{
    public interface ICityInfoRepository
    {
        Task<IEnumerable<City>> GetCitiesAsync();

        Task<(IEnumerable<City>, PaginationMetaData)> GetCitiesAsync(string? name, string? searchquery, int pagenumber, int pagesize);

        Task<City?> GetCityAsync(int cityId, bool includepointofinterest);

        Task<IEnumerable<PointOfInterest>> GetPointOfInterestsForCityAsync(int cityid);

        Task<bool> CityExistsAsync(int cityid);

        Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityid, int pointofinterestid);

        Task AddPointOfInterestForCityAsync(int cityid, PointOfInterest pointofinterest);
        
        void DeletePointOfInterest(PointOfInterest pointofinterest);

        Task<bool> SaveChangesAsync();
    }
}
