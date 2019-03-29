using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FundaAgencyViewer.Models
{
	public class AgencyStatViewModel
	{
		[Required]
		public string City { get; set; }

		[Required]
		public int Max { get; set; }

		public bool HasGarden { get; set; }

		public IList<AgencyStat> Agencies { get; set; }
	}
}
