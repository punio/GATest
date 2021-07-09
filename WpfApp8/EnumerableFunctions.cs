using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp8
{
	public static class EnumerableFunctions
	{
		public static IEnumerable<TResult> Pairwise<T, TResult>(this IEnumerable<T> e, Func<T, T, TResult> resultSelector)
		{
			return e.Zip(e.Skip(1), resultSelector);
		}
		public static IEnumerable<int> To(this int from, int to)
		{
			while (from <= to)
			{
				yield return from++;
			}
		}

	}
}
