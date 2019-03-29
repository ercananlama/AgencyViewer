using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

using FundaAgencyViewer.Models;
using System.Diagnostics;

namespace FundaAgencyViewer.Core
{
	public class FundaEstateClient : IEstateClient
	{
		private const int DefaultPageSize = 25;
		// good to decide by nr of available cores
		private const int MaxSimultaneousSearch = 5;
		private const int MaxTry = 10;
		private static readonly TimeSpan DelayBeforeRetry = TimeSpan.FromSeconds(5);

		private readonly Uri _baseUri;
		private readonly string _key;

		public FundaEstateClient(Uri baseUri, string key)
		{
			_baseUri = baseUri;
			_key = key;
		}

		public async Task<IList<House>> GetHousesAsync(string city, bool? hasGarden)
		{
			return await SearchExecutor.Run((page) => 
				{
					return Search(city, hasGarden, page, DefaultPageSize);
				}, 
				MaxTry, 
				MaxSimultaneousSearch, 
				DelayBeforeRetry);
		}

		private Uri CreateRequest(string city, bool? hasGarden, int page, int pageSize)
		{
			var requestBuilder = new EstateRequestBuilder(_baseUri, _key);
			requestBuilder.WithJsonOutput();
			requestBuilder.WithPaging(page, pageSize);
			requestBuilder = requestBuilder.WithCity(city);
			if (hasGarden == true)
			{
				requestBuilder = requestBuilder.WithGarden();
			}
			return requestBuilder.Build();
		}

		private async Task<SearchResultDto> Search(string city, bool? hasGarden, int page, int pageSize)
		{
			using (var client = new HttpClient())
			{
				var requestUri = CreateRequest(city, hasGarden, page, pageSize);
				try
				{
					Debug.WriteLine($"Calling {requestUri}");

					var result = await client.GetStringAsync(requestUri);
					return JsonConvert.DeserializeObject<SearchResultDto>(result);
				}
				catch (HttpRequestException e)
				{
					Trace.TraceError(e.StackTrace);
					return null;
				}
			}
		}
	}
}
