using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using FundaAgencyViewer.Models;

namespace FundaAgencyViewer.Core
{
	public static class SearchExecutor
	{
		public static async Task<IList<House>> Run(Func<int, Task<SearchResultDto>> search, int maxTry, int maxSimultaneousSearch, TimeSpan delayBeforeRetry)
		{
			var allResults = new List<House>();

			var firstPageSearch = await GetPagedSearchResults(Enumerable.Range(1, 1), search, maxTry, delayBeforeRetry);
			var firstPageResult = firstPageSearch.Single().Value;
			if (firstPageResult == null)
			{
				// instead of returning empty list, maybe we should return another object which contains status and list of houses fetched so far
				// it is also possible the execution might fail later and we return partial list of houses
				return new List<House>();
			}
			allResults.AddRange(GetHouses(firstPageSearch));

			var currentPage = 2;
			var totalPages = firstPageResult.Paging.TotalPages;
			while (currentPage <= totalPages)
			{
				var pages = Enumerable.Range(currentPage, Math.Min(maxSimultaneousSearch, totalPages - currentPage + 1));
				currentPage = pages.Last() + 1;

				var results = await GetPagedSearchResults(pages, search, maxTry, delayBeforeRetry);
				allResults.AddRange(GetHouses(results));
			}

			return allResults;
		}

		private static async Task<Dictionary<int, SearchResultDto>> GetPagedSearchResults(IEnumerable<int> pages, Func<int, Task<SearchResultDto>> search, int maxTry, TimeSpan delayBeforeRetry)
		{
			int tryCount = 1;
			var searches = new Dictionary<int, Task<SearchResultDto>>();
			var pagesToSearch = pages.ToList();
			do
			{
				if (tryCount > 1)
				{
					await Task.Delay(delayBeforeRetry);
				}
				foreach (var page in pagesToSearch)
				{
					searches[page] = search(page);
				}
				await Task.WhenAll(searches.Values);
				pagesToSearch = searches.Where(s => s.Value.Result == null).Select(s => s.Key).ToList();
			}
			while (pagesToSearch.Any() && tryCount++ < maxTry);
			return searches.ToDictionary(s => s.Key, s => s.Value.Result);
		}

		private static IEnumerable<House> GetHouses(Dictionary<int, SearchResultDto> pagedSearchResults)
		{
			return pagedSearchResults.Where(s => s.Value != null).SelectMany(s => s.Value.Houses.Select(h => House.FromDto(h)));
		}
	}
}
