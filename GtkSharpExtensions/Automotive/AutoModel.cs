using System;

namespace GtkSharpExtensions.Automotive
{
	/// <summary>
	/// Stores a model for an Automobile
	/// </summary>
	public class AutoModel
	{
		public string ModelName { get; set; }

		public string ModelType { get; set; }

		public string ModelSubtype { get; set; }

		public Manufacturer Make { get; set; }

		public AutoModel ()
		{
		}

		public override string ToString ()
		{
			return string.Format ("{0}, {1}", ModelName, ModelSubtype);
		}
	}

	public class AutoModelCollection : System.Collections.ObjectModel.Collection<AutoModel>
	{
		
	}
}

