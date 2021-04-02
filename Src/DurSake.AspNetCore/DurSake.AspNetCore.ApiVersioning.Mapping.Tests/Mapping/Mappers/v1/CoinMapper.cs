using DurSake.AspNetCore.ApiVersioning.Mapping.Tests.Mapping.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DurSake.AspNetCore.ApiVersioning.Mapping.Tests.Mapping.Mappers.v1
{
    public class CoinMapper : ICoinMapper
    {
        public IEnumerable<T> Map<T>(IEnumerable<CoinDTO> coins)
        {
            if (coins == null)
            {
                throw new ArgumentNullException(nameof(coins));
            }

            return coins.Select(coin => Map<T>(coin));
        }

        public T Map<T>(CoinDTO coin)
        {
            if (coin == null)
            {
                throw new ArgumentNullException(nameof(coin));
            }

            return (T)(object)new Contracts.v1.Coin
            {
                Type = Enum.Parse<Contracts.v1.CoinType>(coin.Type.ToString()),
                Year = coin.Year,
                Value = coin.Value,
                Description = coin.Description
            };
        }
    }
}
