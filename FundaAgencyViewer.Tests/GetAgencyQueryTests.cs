using System.Linq;
using NUnit.Framework;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

using FundaAgencyViewer.Models;
using FundaAgencyViewer.Core;

namespace FundaAgencyViewer.Tests
{
	public class GetAgencyQueryTests
	{
		[Test]
		public async Task When_city_provided_return_results_only_for_given_city()
		{
			var client = new Mock<IEstateClient>();
			client.Setup(c => c.GetHousesAsync("Amsterdam", null)).ReturnsAsync(CreateTestHouses().Where(c => c.City == "Amsterdam").ToList());
			var query = new GetAgencyQuery(client.Object);
			var results = await query.GetByTopPurchaseAsync(10, "Amsterdam");

			Assert.AreEqual(2, results.Count);
			Assert.AreEqual(new Agency(1, "Agency 1"), results[0].Agency);
			Assert.AreEqual(2, results[0].NrHouses);
			Assert.AreEqual(new Agency(2, "Agency 2"), results[1].Agency);
			Assert.AreEqual(1, results[1].NrHouses);
		}

		private static IList<House> CreateTestHouses()
		{
			return new List<House>()
			{
				new House()
				{
					Price = 100,
					City = "Amsterdam",
					Agency = new Agency(1, "Agency 1")
				},
				new House()
				{
					Price = 120,
					City = "Amsterdam",
					Agency = new Agency(2, "Agency 2")
				},
				new House()
				{
					Price = 90,
					City = "Amsterdam",
					Agency = new Agency(1, "Agency 1")
				},
				new House()
				{
					Price = 180,
					City = "Rotterdam",
					Agency = new Agency(3, "Agency 3")
				}
			};
		}
	}
}