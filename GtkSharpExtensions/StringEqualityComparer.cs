using System;
using System.Collections.Generic;

namespace GtkSharpExtensions
{
	public class StringEqualityComparer : IEqualityComparer<string>
	{
		StringComparison _comparison;

		public StringEqualityComparer ()
			: this (StringComparison.OrdinalIgnoreCase)
		{
		}

		public StringEqualityComparer (StringComparison comparison)
		{
			this._comparison = comparison;
		}

		#region IEqualityComparer implementation

		public bool Equals (string x, string y)
		{
			return String.Equals (x, y, _comparison);
		}

		public int GetHashCode (string obj)
		{
			return obj.GetHashCode ();
		}

		#endregion
	}
}

