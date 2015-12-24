using System;

namespace GtkSharpExtensions.Automotive
{
	/// <summary>
	/// Stores information about an Automobile Type
	/// </summary>
	public class Automobile
	{
		public Manufacturer Make { get; set; }

		public AutoModel Model { get; set; }

		public int Year { get; set; }

		public Automobile ()
		{
		}
	}

	public class AutomobileCollection : System.Collections.ObjectModel.Collection<Automobile>
	{

	}
}

