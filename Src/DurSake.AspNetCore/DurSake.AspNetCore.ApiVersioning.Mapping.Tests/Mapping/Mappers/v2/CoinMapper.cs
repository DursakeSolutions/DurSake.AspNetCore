using System;
using System.Collections.Generic;
using System.Linq;

namespace DurSake.AspNetCore.ApiVersioning.Mapping.Tests.Mapping.Mappers.v2
{
    public class CoinMapper : ICoinMapper
    {
        public IEnumerable<T> Map<T>(IEnumerable<DTOs.CoinDTO> coins)
        {
            if (coins == null)
            {
                throw new ArgumentNullException(nameof(coins));
            }

            return coins.Select(coin => Map<T>(coin));
        }

        public T Map<T>(DTOs.CoinDTO coin)
        {
            if (coin == null)
            {
                throw new ArgumentNullException(nameof(coin));
            }

            return (T)(object)new Contracts.v2.Coin
            {
                Country = Enum.Parse<Contracts.v2.Country>(coin.Country.ToString()),
                Type = Enum.Parse<Contracts.v2.CoinType>(coin.Type.ToString()),
                Year = coin.Year,
                Value = coin.Value,
                Description = coin.Description
            };
        }
    }
}
