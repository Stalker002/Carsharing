using AutoMapper;
using Carsharing.Application.DTOs;
using Carsharing.DataAccess.Entites;

namespace Carsharing.Application.Mappings;

public class CarMappingProfile : Profile
{
    public CarMappingProfile()
    {
        CreateMap<CarEntity, CarWithInfoDto>()
            .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.CarStatus.Name))
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.PricePerMinute, opt => opt.MapFrom(src => src.Tariff.PricePerMinute))
            .ForMember(dest => dest.PricePerKm, opt => opt.MapFrom(src => src.Tariff.PricePerKm))
            .ForMember(dest => dest.PricePerDay, opt => opt.MapFrom(src => src.Tariff.PricePerDay))
            .ForMember(dest => dest.FuelType, opt => opt.MapFrom(src => src.SpecificationCar.FuelType))
            .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.SpecificationCar.Brand))
            .ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.SpecificationCar.Model))
            .ForMember(dest => dest.Transmission, opt => opt.MapFrom(src => src.SpecificationCar.Transmission))
            .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.SpecificationCar.Year))
            .ForMember(dest => dest.StateNumber, opt => opt.MapFrom(src => src.SpecificationCar.StateNumber))
            .ForMember(dest => dest.MaxFuel, opt => opt.MapFrom(src => src.SpecificationCar.MaxFuel));

        CreateMap<CarEntity, CarWithMinInfoDto>()
            .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.CarStatus.Name))
            .ForMember(dest => dest.PricePerDay, opt => opt.MapFrom(src => src.Tariff.PricePerDay))
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.FuelType, opt => opt.MapFrom(src => src.SpecificationCar.FuelType))
            .ForMember(dest => dest.MaxFuel, opt => opt.MapFrom(src => src.SpecificationCar.MaxFuel))
            .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.SpecificationCar.Brand))
            .ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.SpecificationCar.Model))
            .ForMember(dest => dest.Transmission, opt => opt.MapFrom(src => src.SpecificationCar.Transmission));
    }
}