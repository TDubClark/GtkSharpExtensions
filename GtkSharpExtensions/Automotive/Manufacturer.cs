using System;
using System.Collections;
using System.Collections.Generic;

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
		public void AddRange (IEnumerable<Manufacturer> items)
		{
			foreach (var item in items) {
				Add (item);
			}
		}

		IEqualityComparer _comparer;

		public static ManufacturerComparer GetDefaultComparer ()
		{
			return new ManufacturerComparer (new StringEqualityComparer ());
		}

		public ManufacturerCollection ()
		{
			_comparer = GetDefaultComparer ();
		}

		public ManufacturerCollection (IList<Manufacturer> list)
			: base (list)
		{
			_comparer = GetDefaultComparer ();
		}

		/// <summary>
		/// Gets or sets the equality comparer.
		/// </summary>
		/// <value>The comparer.</value>
		public IEqualityComparer Comparer {
			get {
				return _comparer;
			}
			set {
				_comparer = value;
			}
		}
	}

	public class ManufacturerComparer : IEqualityComparer<Manufacturer>, IEqualityComparer
	{
		IEqualityComparer<string> _nameComparer;

		public ManufacturerComparer (IEqualityComparer<string> nameComparer)
		{
			if (nameComparer == null)
				throw new ArgumentNullException ("nameComparer");
			this._nameComparer = nameComparer;
		}

		#region IEqualityComparer implementation

		public bool Equals (Manufacturer x, Manufacturer y)
		{
			if (x == null || y == null)
				return false;
			return _nameComparer.Equals (x.CompanyName, y.CompanyName);
		}

		public int GetHashCode (Manufacturer obj)
		{
			return obj == null ? 0 : obj.CompanyName.GetHashCode ();
		}

		#endregion

		bool IEqualityComparer.Equals (object x, object y)
		{
			return Equals (x as Manufacturer, y as Manufacturer);
		}

		int IEqualityComparer.GetHashCode (object obj)
		{
			return GetHashCode (obj as Manufacturer);
		}
	}
}

