namespace DurSake.AspNetCore.ApiVersioning.Mapping.Tests.Mapping.Contracts.v1
{
    public class Coin
    {
        public CoinType? Type { get; set; }

        public int? Year { get; set; }

        public double? Value { get; set; }

        public string Description { get; set; }
    }
}
