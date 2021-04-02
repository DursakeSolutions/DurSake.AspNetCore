namespace DurSake.AspNetCore.ApiVersioning.Mapping.MapperFactory
{
    public interface IMapperFactory
    {
        IMapper<TDto> GetMapper<T, TDto>();
    }
}