using System;
using System.Collections.Generic;
using System.Collections;

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
		public void AddRange (IEnumerable<AutoModel> items)
		{
			foreach (var item in items) {
				Add (item);
			}
		}

		IEqualityComparer _comparer;

		public static AutoModelComparer GetDefaultComparer ()
		{
			return new AutoModelComparer (new StringEqualityComparer ());
		}

		public AutoModelCollection ()
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

	public class AutoModelComparer : IEqualityComparer<AutoModel>, IEqualityComparer
	{
		IEqualityComparer<string> _nameComparer;

		public AutoModelComparer (IEqualityComparer<string> nameComparer)
		{
			if (nameComparer == null)
				throw new ArgumentNullException ("nameComparer");
			this._nameComparer = nameComparer;
		}

		#region IEqualityComparer implementation

		public bool Equals (AutoModel x, AutoModel y)
		{
			if (x == null || y == null)
				return false;
			return _nameComparer.Equals (x.ModelName, y.ModelName);
		}

		public int GetHashCode (AutoModel obj)
		{
			return obj == null ? 0 : obj.ModelName.GetHashCode ();
		}

		#endregion

		bool IEqualityComparer.Equals (object x, object y)
		{
			return this.Equals (x as AutoModel, y as AutoModel);
		}

		int IEqualityComparer.GetHashCode (object obj)
		{
			return this.GetHashCode (obj as AutoModel);
		}
	}
}

