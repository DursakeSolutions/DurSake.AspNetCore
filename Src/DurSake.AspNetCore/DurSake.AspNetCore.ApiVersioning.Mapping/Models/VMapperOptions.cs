using System;
using System.Collections.Generic;

namespace DurSake.AspNetCore.ApiVersioning.Mapping.Models
{
    public class VMapperOptions
    {
        public IDictionary<Type, MapperRegistry> RegisteredMappers { get; set; }
    }
}