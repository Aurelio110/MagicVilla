namespace MagicVilla_Web
{
    public class MappingConfig : AutoMapper.Profile
    {
        public MappingConfig()
        {
            CreateMap<Models.Villa, Models.Dto.VillaDTO>().ReverseMap();
            CreateMap<Models.Dto.VillaDTO, Models.Villa>().ReverseMap();
        }
    }
}
