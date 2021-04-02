using System.Collections.Generic;

namespace DurSake.AspNetCore.ApiVersioning.Mapping
{
    public interface IVMapper
    {
        IEnumerable<T> Map<T>(IEnumerable<object> dtoObjects);

        T Map<T>(object dtoObject);
    }
}