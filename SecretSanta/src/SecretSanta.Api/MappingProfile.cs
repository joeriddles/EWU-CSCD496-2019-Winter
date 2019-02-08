using AutoMapper;
using SecretSanta.Api.ViewModels;
using SecretSanta.Domain.Models;

namespace SecretSanta.Api
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			RecognizeDestinationPostfixes("ViewModel");
			CreateMap<User, UserViewModel>().ReverseMap();
			CreateMap<User, UserInputViewModel>().ReverseMap();
			CreateMap<Gift, GiftViewModel>().ReverseMap();
			CreateMap<Group, GroupViewModel>().ReverseMap();
			CreateMap<Group, GroupInputViewModel>().ReverseMap();
		}
	}
}
