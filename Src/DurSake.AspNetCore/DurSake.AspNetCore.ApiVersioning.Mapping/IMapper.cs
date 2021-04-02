using System.Collections.Generic;

namespace DurSake.AspNetCore.ApiVersioning.Mapping
{
    public interface IMapper<TDto>
    {
        IEnumerable<T> Map<T>(IEnumerable<TDto> dtoObjects);

        T Map<T>(TDto dtoObject);
    }
}