namespace DurSake.AspNetCore.ApiVersioning.Mapping.Tests.Mapping.Contracts.v2
{
    public class Coin
    {
        public Country? Country { get; set; }

        public CoinType? Type { get; set; }

        public int? Year { get; set; }

        public double? Value { get; set; }

        public string Description { get; set; }
    }
}
