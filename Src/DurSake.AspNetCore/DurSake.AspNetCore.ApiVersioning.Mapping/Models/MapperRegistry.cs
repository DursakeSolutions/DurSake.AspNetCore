using System;

namespace DurSake.AspNetCore.ApiVersioning.Mapping.Models
{
    public class MapperRegistry
    {
        public MapperRegistry()
        {
        }

        public MapperRegistry(Type tService, Type tImplementation)
        {
            TService = tService;
            TImplementation = tImplementation;
        }

        public Type TService { get; set; }

        public Type TImplementation { get; set; }

        public static MapperRegistry Create<TService, TImplementation>() =>
            new MapperRegistry(typeof(TService), typeof(TImplementation));

        public static MapperRegistry Create(Type tService, Type tImplementation) =>
            new MapperRegistry(tService, tImplementation);
    }
}
