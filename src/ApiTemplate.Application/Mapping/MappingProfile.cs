using ApiTemplate.Application.Interfaces;
using ApiTemplate.Domain.Entities;
using AutoMapper;

namespace ApiTemplate.Application.Mapping;

/// <summary>
/// AutoMapper profile for the application
/// </summary>
public class MappingProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the MappingProfile class
    /// </summary>
    public MappingProfile()
    {
        // Sample mappings
        CreateMap<Sample, SampleDto>()
            .ForCtorParam("Id", opt => opt.MapFrom(src => src.Id))
            .ForCtorParam("Name", opt => opt.MapFrom(src => src.Name))
            .ForCtorParam("Description", opt => opt.MapFrom(src => src.Description))
            .ForCtorParam("CreatedAt", opt => opt.MapFrom(src => src.CreatedAt));
    }
}