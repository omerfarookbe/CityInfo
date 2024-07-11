using CityInfo.API.DbContexts;
using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Services
{
    public class CityInfoRepository : ICityInfoRepository
    {
        private readonly CityInfoContext cityinfocontext;

        public CityInfoRepository(CityInfoContext _cityinfocontext)
        {
            this.cityinfocontext = _cityinfocontext;
        }
        public async Task<IEnumerable<City>> GetCitiesAsync()
        {
            return await cityinfocontext.Cities.OrderBy(o => o.Name).ToListAsync();
        }
        public async Task<(IEnumerable<City>,PaginationMetaData)> GetCitiesAsync(string? name, string? searchquery, int pagenumber, int pagesize)
        {
            var cities = cityinfocontext.Cities as IQueryable<City>;

            if (!string.IsNullOrEmpty(name))
            {
                name = name.Trim();
                cities = cities.Where(f => f.Name == name);
            }

            if (!string.IsNullOrEmpty(searchquery))
            {
                searchquery = searchquery.Trim();
                cities = cities.Where(f => f.Name.Contains(name) || ( f.Description !=null && f.Description.Contains(searchquery)));
            }

            var totalItemCount = await cities.CountAsync();

            var paginationMetaData = new PaginationMetaData(totalItemCount, pagesize, pagenumber);

            var result = await cities
                .Skip(pagesize * (pagenumber-1))
                .Take(pagesize)
                .OrderBy(o => o.Name)
                .ToListAsync();

            return (result, paginationMetaData);
        }

        public async Task<City?> GetCityAsync(int cityId, bool includepointofinterest)
        {
            if (includepointofinterest)
            {
                return await cityinfocontext.Cities.Include(i => i.PointOfInterests).Where(f => f.Id == cityId).FirstOrDefaultAsync();
            }

            return await cityinfocontext.Cities.Where(f => f.Id == cityId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PointOfInterest>> GetPointOfInterestsForCityAsync(int cityid)
        {
            return await cityinfocontext.PointOfInterests.Where(f => f.CityId == cityid).OrderBy(o => o.Name).ToListAsync();
        }

        public async Task<bool> CityExistsAsync(int cityid)
        {
            return await cityinfocontext.Cities.AnyAsync(f => f.Id == cityid);
        }

        public async Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityid, int pointofinterestid)
        {
            return await cityinfocontext.PointOfInterests.Where(f => f.CityId == cityid && f.Id == pointofinterestid).FirstOrDefaultAsync();
        }

        public async Task AddPointOfInterestForCityAsync(int cityid, PointOfInterest pointofinterest)
        {
            var city = await GetCityAsync(cityid, false);
            if (city != null)
            {
                city.PointOfInterests.Add(pointofinterest);
            }
        }

        public void DeletePointOfInterest(PointOfInterest pointofinterest)
        {
            cityinfocontext.PointOfInterests.Remove(pointofinterest);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await cityinfocontext.SaveChangesAsync() > 0);
        }
    }
}