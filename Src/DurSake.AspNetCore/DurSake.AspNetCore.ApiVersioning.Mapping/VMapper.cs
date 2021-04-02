using DurSake.AspNetCore.ApiVersioning.Mapping.MapperFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DurSake.AspNetCore.ApiVersioning.Mapping
{
    public class VMapper : IVMapper
    {
        private readonly IMapperFactory mapperFactory;

        public VMapper(IMapperFactory mapperFactory)
        {
            this.mapperFactory = mapperFactory ?? throw new ArgumentNullException(nameof(mapperFactory));
        }

        public IEnumerable<T> Map<T>(IEnumerable<object> dtoObjects)
        {
            var dtoObjectType = dtoObjects.GetType().GetGenericArguments()[0];

            var iMapperType = typeof(IMapper<>).MakeGenericType(new[] { dtoObjectType });

            MethodInfo getMapperMethod = mapperFactory.GetType().GetMethod(nameof(MapperFactory.MapperFactory.GetMapper));
            getMapperMethod =
                getMapperMethod.MakeGenericMethod(typeof(T), dtoObjectType);

            MethodInfo iMapperMapMethod =
                iMapperType.GetMethods().SingleOrDefault(m => m.ReturnType.IsInterface);
            iMapperMapMethod = iMapperMapMethod.MakeGenericMethod(typeof(T));

            var vMapper = getMapperMethod.Invoke(mapperFactory, null);
            return (IEnumerable<T>)iMapperMapMethod.Invoke(vMapper, new[] { dtoObjects });
        }

        public T Map<T>(object dtoObject)
        {
            var dtoObjectType = dtoObject.GetType();
            var iMapperType = typeof(IMapper<>).MakeGenericType(new[] { dtoObjectType });

            MethodInfo getMapperMethod = mapperFactory.GetType().GetMethod(nameof(MapperFactory.MapperFactory.GetMapper));
            getMapperMethod =
                getMapperMethod.MakeGenericMethod(typeof(T), dtoObjectType);

            MethodInfo iMapperMapMethod =
                iMapperType.GetMethods().SingleOrDefault(m => !m.ReturnType.IsInterface);
            iMapperMapMethod = iMapperMapMethod.MakeGenericMethod(typeof(T));

            var vMapper = getMapperMethod.Invoke(mapperFactory, null);
            return (T)iMapperMapMethod.Invoke(vMapper, new[] { dtoObject });
        }
    }
}
