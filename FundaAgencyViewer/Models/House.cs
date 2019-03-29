using System;
using System.Collections.Generic;
using System.Text;

using FundaAgencyViewer.Core;

namespace FundaAgencyViewer.Models
{
	public class House
	{
		public string Id { get; set; }

		public int? Price { get; set; }

		public string City { get; set; }

		public Agency Agency { get; set; }

		public override bool Equals(object obj)
		{
			if (!(obj is House house))
				return false;

			return Id == house.Id;
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}

		public static House FromDto(HouseDto dto)
		{
			return new House()
			{
				Id = dto.Id,
				Price = dto.Price,
				City = dto.City,
				Agency = new Agency(dto.AgencyId, dto.AgencyName)
			};
		}
	}
}
