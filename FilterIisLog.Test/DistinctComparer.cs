using System;
using System.Collections.Generic;

namespace FilterIisLog.Test
{
	class DistinctComparer : IEqualityComparer<string>
	{
		public bool Equals(string x, string y)
		{
			return x.Equals(y);
		}

		public int GetHashCode(string obj)
		{
			return obj.GetHashCode();
		}
	}
}