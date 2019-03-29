using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using FundaAgencyViewer.Models;

namespace FundaAgencyViewer.Core
{
	public interface IEstateClient
	{
		Task<IList<House>> GetHousesAsync(string city, bool? hasGarden);
	}
}
