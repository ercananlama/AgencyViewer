using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using FundaAgencyViewer.Models;

namespace FundaAgencyViewer.Core
{
	public interface IGetAgencyQuery
	{
		Task<IList<AgencyStat>> GetByTopPurchaseAsync(int max, string city, bool? hasGarden = null);
	}
}
