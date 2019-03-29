using System;
using System.Collections.Generic;
using System.Text;

namespace FundaAgencyViewer.Models
{
	public class Agency
	{
		public int Id { get; private set; }

		public string Name { get; private set; }

		public Agency(int id, string name)
		{
			Id = id;
			Name = name;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is Agency agency))
				return false;

			return Id == agency.Id;
		}

		public override int GetHashCode()
		{
			return Id;
		}
	}
}
