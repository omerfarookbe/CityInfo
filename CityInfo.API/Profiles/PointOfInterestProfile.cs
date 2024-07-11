using AutoMapper;

namespace CityInfo.API.Profiles
{
    public class PointOfInterestProfile : Profile
    {
        public PointOfInterestProfile()
        {
            CreateMap<Entities.PointOfInterest,Models.PointOfInterestDto>();
            CreateMap<Models.PointOfInterestCreateDto, Entities.PointOfInterest>();
            CreateMap<Models.PointOfInterestUpdateDto, Entities.PointOfInterest>();
            CreateMap<Entities.PointOfInterest, Models.PointOfInterestUpdateDto>();
        }
    }
}