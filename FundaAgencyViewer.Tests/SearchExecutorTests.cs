using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

using FundaAgencyViewer.Core;
using FundaAgencyViewer.Models;

namespace FundaAgencyViewer.Tests
{
	public class SearchExecutorTests
	{
		[Test]
		public async Task When_execution_always_fails_it_should_try_till_max_try_count()
		{
			var tryCount = 0;
			var maxTry = 5;
			var result = await SearchExecutor.Run((page) =>
			{
				tryCount++;
				SearchResultDto searchResult = null;
				return Task.FromResult(searchResult);
			},
			maxTry,
			5,
			TimeSpan.FromMilliseconds(5));

			Assert.AreEqual(maxTry, tryCount);
			Assert.IsEmpty(result);
		}

		[Test]
		public async Task When_execution_always_succeeds_it_should_try_only_once()
		{
			var housesPerPage = GenerateHouses();

			SearchResultDto Search(int page)
			{
				return new SearchResultDto
				{
					Paging = new PagingDto()
					{
						CurrentPage = page,
						TotalPages = 3
					},
					Houses = housesPerPage[page]
				};
			}

			var tryCount = 0;
			var maxTry = 1;
			var result = await SearchExecutor.Run((page) =>
			{
				tryCount++;
				return Task.FromResult(Search(page));
			},
			maxTry,
			1,
			TimeSpan.FromMilliseconds(5));

			Assert.AreEqual(3, tryCount);
			Assert.AreEqual(housesPerPage.SelectMany(p => p.Value.Select(h => House.FromDto(h))), result);
		}

		[Test]
		public async Task When_execution_fails_it_should_continue_after_sucessful_try()
		{
			var housesPerPage = GenerateHouses();

			var tryCountForFailingPage = 0;
			var failCount = 2;
			var failingPage = 2;
			SearchResultDto Search(int page)
			{
				if (page == failingPage)
				{
					tryCountForFailingPage++;
					if (tryCountForFailingPage < failCount)
					{
						return null;
					}
				}

				return new SearchResultDto
				{
					Paging = new PagingDto()
					{
						CurrentPage = page,
						TotalPages = housesPerPage.Count
					},
					Houses = housesPerPage[page]
				};
			}

			var tryCount = 0;
			var maxTry = failCount;
			var result = await SearchExecutor.Run((page) =>
			{
				tryCount++;
				return Task.FromResult(Search(page));
			},
			maxTry,
			3,
			TimeSpan.FromMilliseconds(5));

			Assert.AreEqual(housesPerPage.Count + failCount - 1, tryCount);
			Assert.AreEqual(housesPerPage.SelectMany(p => p.Value.Select(h => House.FromDto(h))), result);
		}

		private static Dictionary<int, List<HouseDto>> GenerateHouses()
		{
			return new Dictionary<int, List<HouseDto>>
			{
				[1] = new List<HouseDto>()
				{
					new HouseDto()
					{
						City = "Amsterdam",
						Price = 100
					},
					new HouseDto()
					{
						City = "Amsterdam",
						Price = 110
					}
				},
				[2] = new List<HouseDto>()
				{
					new HouseDto()
					{
						City = "Amsterdam",
						Price = 125
					},
					new HouseDto()
					{
						City = "Amsterdam",
						Price = 78
					},
					new HouseDto()
					{
						City = "Amsterdam",
						Price = 230
					}
				},
				[3] = new List<HouseDto>()
				{
					new HouseDto()
					{
						City = "Amsterdam",
						Price = 88
					}
				}
			};
		}
	}
}
