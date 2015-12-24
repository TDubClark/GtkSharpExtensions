using System;

namespace GtkSharpExtensions.Automotive
{
	/// <summary>
	/// An Automotive Manufacturer.
	/// </summary>
	public class Manufacturer
	{
		public string CompanyName { get; set; }

		public string CompanyInitials { get; set; }

		public Manufacturer ()
		{
		}

		public override string ToString ()
		{
			return string.Format ("{0}", CompanyName);
		}
	}

	public class ManufacturerCollection : System.Collections.ObjectModel.Collection<Manufacturer>
	{
		public ManufacturerCollection ()
		{
		}

		public ManufacturerCollection (System.Collections.Generic.IList<Manufacturer> list)
			: base (list)
		{
		}
	}
}

