namespace KatlaSport.Services.Tests
{
    using System;

    using AutoMapper;

    using KatlaSport.Services.HiveManagement;
    using KatlaSport.Services.ProductManagement;

    public class MapperInitialize
    {
        private static readonly Lazy<MapperInitialize> _instanseInitialize = new Lazy<MapperInitialize>(() => new MapperInitialize());

        private MapperInitialize()
        {
            Mapper.Reset();
            Mapper.Initialize(x =>
            {
                x.AddProfile(new HiveManagementMappingProfile());
                x.AddProfile(new ProductManagementMappingProfile());
            });
        }

        public static MapperInitialize Initialize()
        {
            return _instanseInitialize.Value;
        }
    }
}
