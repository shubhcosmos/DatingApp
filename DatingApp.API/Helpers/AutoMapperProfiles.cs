using System.Linq;
using AutoMapper;
using DatingApp.API.Dtos;
using DatingApp.API.Models;

namespace DatingApp.API.Helpers
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
             CreateMap<User,UserForListDto>().ForMember(destinationMember=>destinationMember.PhotoUrl,IMappingOperationOptions=>{
                 IMappingOperationOptions.MapFrom(sourceMember=> sourceMember.Photos.FirstOrDefault(p=>p.IsMain).Url);
             }).ForMember(Propertytomanuallychange=>Propertytomanuallychange.Age ,IMappingOperationOptions=>{
                 IMappingOperationOptions.ResolveUsing(source=>source.DateOfBirth.CalculateAge());
             });
             CreateMap<User,UserForDetailedDto>().ForMember(destinationMember=>destinationMember.PhotoUrl,IMappingOperationOptions=>{
                 IMappingOperationOptions.MapFrom(sourceMember=> sourceMember.Photos.FirstOrDefault(p=>p.IsMain).Url);
             }).ForMember(Propertytomanuallychange=>Propertytomanuallychange.Age ,IMappingOperationOptions=>{
                 IMappingOperationOptions.ResolveUsing(source=>source.DateOfBirth.CalculateAge());
             });
             CreateMap<Photo,PhotosForDetailedDto>() ;
             CreateMap<UserForUpdateDto,User>();
             CreateMap<Photo,PhotoForReturnDto>();
             CreateMap<PhotoForCreationDto,Photo>();

             CreateMap<UserForRegisterDto,User>();

             CreateMap<MessageForCreationDto,Message>().ReverseMap();

             CreateMap<Message,MessageToReturnDto>()
             .ForMember(m=>m.SenderPhotoUrl ,opt =>opt.MapFrom(m=>m.Sender.Photos.FirstOrDefault(p=>p.IsMain).Url))
              .ForMember(m=>m.RecipientPhotoUrl ,opt =>opt.MapFrom(m=>m.Recipient.Photos.FirstOrDefault(p=>p.IsMain).Url));

        }
    }
}