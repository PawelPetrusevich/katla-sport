using AutoMapper;
using DataAccessHive = KatlaSport.DataAccess.ProductStoreHive.StoreHive;
using DataAccessHiveSection = KatlaSport.DataAccess.ProductStoreHive.StoreHiveSection;

namespace KatlaSport.Services.HiveManagement
{
    public sealed class HiveManagementMappingProfile : Profile
    {
        public HiveManagementMappingProfile()
        {
            CreateMap<DataAccessHive, HiveListItem>().ReverseMap();
            CreateMap<DataAccessHive, Hive>().ReverseMap();
            CreateMap<DataAccessHiveSection, HiveSectionListItem>().ReverseMap();
            CreateMap<DataAccessHiveSection, HiveSection>().ReverseMap();
            CreateMap<UpdateHiveRequest, DataAccessHive>().ReverseMap();
        }
    }
}
