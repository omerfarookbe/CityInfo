using Asp.Versioning;
using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/cities")]
    [ApiVersion(1)]
    [ApiVersion(2)]
    public class CitiesController : ControllerBase
    {
        public ICityInfoRepository cityinforepository { get; }
        private readonly IMapper mapper;

        const int maxPagesize = 20;

        public CitiesController(ICityInfoRepository _cityinforepository, IMapper _mapper)
        {
            cityinforepository = _cityinforepository;
            mapper = _mapper;
        }

        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<CityWithoutPointOfInterestDto>>> GetCities(string? name, string? searchquery, int pagenumber = 1, int pagesize = 10)
        {
            if (pagesize > maxPagesize)
            {
                pagesize = maxPagesize;
            }

            var (cities, paginationMetaData) = await cityinforepository.GetCitiesAsync(name, searchquery, pagenumber, pagesize);

            Response.Headers.Add("X-Pagination",JsonSerializer.Serialize(paginationMetaData));

            return Ok(mapper.Map<IEnumerable<CityWithoutPointOfInterestDto>>(cities));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCity(int id, bool includepointofinterest = false)
        {
            var city = await cityinforepository.GetCityAsync(id, includepointofinterest);
            if (city == null)
                return NotFound("City not found");

            if (includepointofinterest)
            {
                return Ok(mapper.Map<CityDto>(city));
            }

            return Ok(mapper.Map<CityWithoutPointOfInterestDto>(city));
        }
    }
}