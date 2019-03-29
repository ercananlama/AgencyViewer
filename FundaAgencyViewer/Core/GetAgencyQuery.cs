using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

using FundaAgencyViewer.Models;

namespace FundaAgencyViewer.Core
{
	public sealed class GetAgencyQuery : IGetAgencyQuery
	{
		private readonly IEstateClient _client;

		public GetAgencyQuery(IEstateClient client)
		{
			_client = client;
		}

		public async Task<IList<AgencyStat>> GetByTopPurchaseAsync(int max, string city, bool? hasGarden = null)
		{
			if (string.IsNullOrEmpty(city))
				throw new ArgumentNullException(nameof(city));

			var houses = await _client.GetHousesAsync(city, hasGarden);
			return houses.
					GroupBy(c => c.Agency).
					OrderByDescending(c => c.Count()).
					Take(max).
					Select(c => new AgencyStat()
					{
						Agency = c.Key,
						NrHouses = c.Count()
					}).ToList();
		}
	}
}
