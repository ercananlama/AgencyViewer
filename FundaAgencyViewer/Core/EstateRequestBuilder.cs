using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace FundaAgencyViewer.Core
{
	public class EstateRequestBuilder
	{
		private readonly Uri _baseUri;

		private string _city;
		private string _key;
		private bool? _hasGarden;
		private int? _page;
		private int? _pageSize;
		private bool? _hasJsonOutput;

		public EstateRequestBuilder(Uri baseUri, string key)
		{
			_baseUri = baseUri ?? throw new ArgumentNullException(nameof(baseUri));
			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentNullException(nameof(key));
			}
			_key = key;
		}

		public EstateRequestBuilder WithCity(string city)
		{
			_city = city;
			return this;
		}

		public EstateRequestBuilder WithGarden()
		{
			_hasGarden = true;
			return this;
		}

		public EstateRequestBuilder WithPaging(int page, int pageSize)
		{
			_page = page;
			_pageSize = pageSize;
			return this;
		}

		public EstateRequestBuilder WithJsonOutput()
		{
			_hasJsonOutput = true;
			return this;
		}

		public Uri Build()
		{
			var uriBuilder = new UriBuilder(_baseUri);
			if (_hasJsonOutput == true)
			{
				uriBuilder.Path += "/json";
			}
			uriBuilder.Path += $"/{_key}/";

			var queryBuilder = new StringBuilder();
			queryBuilder.Append("type=koop"); // for simplicity, we add sale flag here
			var search = string.Empty;
			if (!string.IsNullOrEmpty(_city))
			{
				search = $"/{_city}/";
			}
			if (_hasGarden == true)
			{
				if (string.IsNullOrEmpty(search))
				{
					search += "/";
				}
				search += "tuin/";
			}
			queryBuilder.AppendFormat("&zo={0}", search);
			if (_page != null && _pageSize != null)
			{
				queryBuilder.AppendFormat("&page={0}&pagesize={1}", _page, _pageSize);
			}
			uriBuilder.Query = queryBuilder.ToString();

			return uriBuilder.Uri;
		}
	}
}
