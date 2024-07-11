using Asp.Versioning;
using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using System.Reflection.Metadata.Ecma335;

namespace CityInfo.API.Controllers
{
    [Route("api/v{version:apiVersion}/cities/{cityid}/pointofinterest")]
    [ApiController]
    [ApiVersion(2)]
    //[Authorize]
    public class PointOfInterestsConroller : ControllerBase
    {
        private readonly ILogger<PointOfInterestsConroller> logger;
        private readonly IMailService mailservice;
        private readonly ICityInfoRepository cityinforepository;
        private readonly IMapper mapper;

        public PointOfInterestsConroller(ILogger<PointOfInterestsConroller> _logger, IMailService _mailservice, ICityInfoRepository _cityinforepository, IMapper _mapper)
        {
            logger = _logger;
            mailservice = _mailservice;
            cityinforepository = _cityinforepository;
            mapper = _mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PointOfInterestDto>>> GetPointOfInterests(int cityid)
        {
            bool cityexist = await cityinforepository.CityExistsAsync(cityid);

            if (!cityexist)
            {
                logger.LogInformation($"City with id {cityid} wasn't found when accessing point of interests");
                return NotFound("City not found");
            }

            var pointofinterestforcity = await cityinforepository.GetPointOfInterestsForCityAsync(cityid);

            return Ok(mapper.Map<IEnumerable<PointOfInterestDto>>(pointofinterestforcity));

        }

        [HttpGet("{pointofinterestid}", Name = "GetPointOfInterest")]
        public async Task<ActionResult<PointOfInterestDto>> GetPointOfInterest(int cityid, int pointofinterestid)
        {
            bool cityexist = await cityinforepository.CityExistsAsync(cityid);

            if (!cityexist)
            {
                return NotFound("City not found");
            }

            var pointofinterestforcity = await cityinforepository.GetPointOfInterestForCityAsync(cityid, pointofinterestid);
            if (pointofinterestforcity == null)
            {
                return NotFound("Point of interest not found");
            }

            return Ok(mapper.Map<PointOfInterestDto>(pointofinterestforcity));
        }

        [HttpPost]
        public async Task<ActionResult<PointOfInterestDto>> CreatePointOfInterest(int cityid, [FromBody] PointOfInterestCreateDto pointOfInterestCreationDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            bool cityexist = await cityinforepository.CityExistsAsync(cityid);

            if (!cityexist)
            {
                return NotFound("City not found");
            }


            var finalPointOfInterest = mapper.Map<Entities.PointOfInterest>(pointOfInterestCreationDto);

            await cityinforepository.AddPointOfInterestForCityAsync(cityid, finalPointOfInterest);
            await cityinforepository.SaveChangesAsync();

            var createdpointofintertesttoreturn = mapper.Map<Models.PointOfInterestDto>(finalPointOfInterest);
            return CreatedAtRoute("GetPointOfInterest",
                new
                {
                    cityid = cityid,
                    pointofinterestid = createdpointofintertesttoreturn.Id
                }, createdpointofintertesttoreturn);
        }

        [HttpPut("{pointofinterestid}")]
        public async Task<ActionResult> UpdatePointOfInterest(int cityid, int pointofinterestid, PointOfInterestUpdateDto pointOfInterestUpdateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            bool cityexist = await cityinforepository.CityExistsAsync(cityid);

            if (!cityexist)
            {
                return NotFound("City not found");
            }

            var pointofinterestexisting = await cityinforepository.GetPointOfInterestForCityAsync(cityid, pointofinterestid);
            if (pointofinterestexisting == null)
                return NotFound("Point of intertest not found");

            mapper.Map(pointOfInterestUpdateDto, pointofinterestexisting);

            await cityinforepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{pointofinterestid}")]
        public async Task<ActionResult> PatchPointOfInterest(int cityid, int pointofinterestid, JsonPatchDocument<PointOfInterestUpdateDto> patchDocument)
        {
            bool cityexist = await cityinforepository.CityExistsAsync(cityid);

            if (!cityexist)
            {
                return NotFound("City not found");
            }

            var pointofinterestexisting = await cityinforepository.GetPointOfInterestForCityAsync(cityid, pointofinterestid);
            if (pointofinterestexisting == null)
                return NotFound("Point of intertest not found");

            var pointofintertestToPatch = mapper.Map<PointOfInterestUpdateDto>(pointofinterestexisting);

            patchDocument.ApplyTo(pointofintertestToPatch, ModelState);

            if (!ModelState.IsValid)
                return BadRequest();
            mapper.Map(pointofintertestToPatch, pointofinterestexisting);

            await cityinforepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{pointofinterestid}")]
        public async Task<ActionResult> DeletePointOfInterest(int cityid, int pointofinterestid)
        {
            bool cityexist = await cityinforepository.CityExistsAsync(cityid);

            if (!cityexist)
            {
                return NotFound("City not found");
            }

            var pointofinterestexisting = await cityinforepository.GetPointOfInterestForCityAsync(cityid, pointofinterestid);
            if (pointofinterestexisting == null)
                return NotFound("Point of intertest not found");

            cityinforepository.DeletePointOfInterest(pointofinterestexisting);
            await cityinforepository.SaveChangesAsync();

            mailservice.SendMail("Point of interest deleted", $"Point of interetest {pointofinterestexisting.Name} with id {pointofinterestexisting.Id} is deleted");

            return Ok();

        }
    }
}