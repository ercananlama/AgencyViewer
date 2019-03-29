using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using FundaAgencyViewer.Models;
using FundaAgencyViewer.Core;

namespace FundaAgencyViewer.Controllers
{
	public class HomeController : Controller
	{
		private readonly IGetAgencyQuery _query;

		public HomeController(IGetAgencyQuery query)
		{
			_query = query;
		}

		public IActionResult Index()
		{
			return View(new AgencyStatViewModel()
			{
				City = "Amsterdam",
				Max = 10
			});
		}

		[HttpPost]
		public async Task<IActionResult> Index(AgencyStatViewModel vm)
		{
			if (!ModelState.IsValid)
				return View(vm);

			bool? hasGarden = null;
			if (vm.HasGarden)
				hasGarden = true;

			vm.Agencies = await _query.GetByTopPurchaseAsync(vm.Max, vm.City, hasGarden);
			return View(vm);
		}
	}
}
