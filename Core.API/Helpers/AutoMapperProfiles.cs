using System.Linq;
using AutoMapper;
using Core.API.Dtos;
using Core.API.Models;

namespace Core.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserListDto>()
                .ForMember(dest => dest.PhotoUrl, opt => 
                {
                    opt.MapFrom(src => src.Photos.FirstOrDefault(ph => ph.IsMain).Url);
                })
                .ForMember(dest => dest.Age, opt => 
                {
                    opt.MapFrom(d => d.DateOfBirth.CalculateAge());
                });

            CreateMap<User, UserDetailsDto>()
                .ForMember(dest => dest.PhotoUrl, opt => 
                {
                    opt.MapFrom(src => src.Photos.FirstOrDefault(ph => ph.IsMain).Url);
                })
                .ForMember(dest => dest.Age, opt => 
                {
                    opt.MapFrom(d => d.DateOfBirth.CalculateAge());
                });
                
            CreateMap<Photo, PhotoDetailsDto>();
            CreateMap<UserEditDto, User>();
            CreateMap<Photo, PhotoReturnDto>();
            CreateMap<PhotoCreationDto, Photo>();
            CreateMap<UserRegistrationDto, User>(); 
            CreateMap<MessageCreationDto, Message>().ReverseMap();       
            CreateMap<Message, MessageDetailDto>()
                .ForMember(m => m.SenderPhotoUrl, 
                    opt => opt.MapFrom(u => u.Sender.Photos.FirstOrDefault(ph => ph.IsMain).Url))
                .ForMember(m => m.RecipientPhotoUrl, 
                    opt => opt.MapFrom(u => u.Recipient.Photos.FirstOrDefault(ph => ph.IsMain).Url));
        }
    }
}