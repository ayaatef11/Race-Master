using AutoMapper;

namespace RunGroop.Application.Mapping
{
	internal interface IMapFrom<T>
	{
		void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType());
	}
}
