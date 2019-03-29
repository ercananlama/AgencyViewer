using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace FundaAgencyViewer.Core
{
	[DataContract]
	public class SearchResultDto
	{
		[DataMember(Name = "TotaalAantalObjecten")]
		public int TotalNrOfRecords { get; set; }

		[DataMember(Name = "Paging")]
		public PagingDto Paging { get; set; }

		[DataMember(Name = "Objects")]
		public IList<HouseDto> Houses { get; set; }
	}

	[DataContract]
	public class PagingDto
	{
		[DataMember(Name = "AantalPaginas")]
		public int TotalPages { get; set; }

		[DataMember(Name = "HuidigePagina")]
		public int CurrentPage { get; set; }
	}

	[DataContract]
	public class HouseDto
	{
		[DataMember(Name = "Id")]
		public string Id { get; set; }

		[DataMember(Name = "Koopprijs")]
		public int? Price { get; set; }

		[DataMember(Name = "Woonplaats")]
		public string City { get; set; }

		[DataMember(Name = "MakelaarId")]
		public int AgencyId { get; set; }

		[DataMember(Name = "MakelaarNaam")]
		public string AgencyName { get; set; }
	}
}
