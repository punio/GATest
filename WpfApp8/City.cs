using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp8
{
	public class City
	{
		public int X { get; set; }
		public int Y { get; set; }


		public double DistanceTo(City to)
		{
			return Math.Sqrt(Math.Pow(X - to.X, 2) + Math.Pow(Y - to.Y, 2));
		}
	}

	public class CityPath
	{
		public City From { get; set; }
		public City To { get; set; }
	}
}
