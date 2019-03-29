using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

using FundaAgencyViewer.Core;

namespace FundaAgencyViewer.Tests
{
	public class EstateRequestBuilderTests
	{
		private readonly Uri _baseUri = new Uri("http://partnerapi.funda.nl/feeds/Aanbod.svc");
		private readonly string _key = "123";

		[Test]
		public void When_city_provided_request_should_contain_city()
		{
			var builder = new EstateRequestBuilder(_baseUri, _key);
			var requestUri = builder.
					WithCity("amsterdam").
					Build();

			Assert.AreEqual($"{_baseUri}/{_key}/?type=koop&zo=/amsterdam/", requestUri.ToString());
		}

		[Test]
		public void When_garden_provided_request_should_contain_garden()
		{
			var builder = new EstateRequestBuilder(_baseUri, _key);
			var requestUri = builder.
					WithGarden().
					Build();

			Assert.AreEqual($"{_baseUri}/{_key}/?type=koop&zo=/tuin/", requestUri.ToString());
		}

		[Test]
		public void When_city_and_garden_provided_request_should_contain_city_and_garden()
		{
			var builder = new EstateRequestBuilder(_baseUri, _key);
			var requestUri = builder.
					WithCity("amsterdam").
					WithGarden().
					Build();

			Assert.AreEqual($"{_baseUri}/{_key}/?type=koop&zo=/amsterdam/tuin/", requestUri.ToString());
		}

		[Test]
		public void When_all_parameters_provided_request_should_contain_all()
		{
			var builder = new EstateRequestBuilder(_baseUri, _key);
			var requestUri = builder.
					WithCity("amsterdam").
					WithGarden().
					WithJsonOutput().
					WithPaging(1, 25).
					Build();

			Assert.AreEqual($"{_baseUri}/json/{_key}/?type=koop&zo=/amsterdam/tuin/&page=1&pagesize=25", requestUri.ToString());
		}
	}
}
