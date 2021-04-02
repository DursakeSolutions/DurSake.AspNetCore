namespace DurSake.AspNetCore.ApiVersioning.Mapping.Tests.Mapping.DTOs
{
    public class CoinDTO
    {
        public CountryDTO? Country { get; set; }

        public CoinTypeDTO? Type { get; set; }

        public int? Year { get; set; }

        public double? Value { get; set; }

        public string Description { get; set; }
    }
}
