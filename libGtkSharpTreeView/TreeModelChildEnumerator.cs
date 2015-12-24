using System;
using System.Collections.Generic;

namespace GtkSharp.Extensions
{
	public class TreeModelChildEnumerator : IEnumerator<Gtk.TreeIter>, IEnumerable<Gtk.TreeIter>
	{
		private Gtk.TreeModel _model;

		private Gtk.TreeIter _current;

		private Gtk.TreeIter _parent;

		private int _index;

		private int _count;

		public TreeModelChildEnumerator (Gtk.TreeModel model, Gtk.TreeIter parent)
		{
			if (model == null)
				throw new ArgumentNullException ("model");
			
			this._model = model;
			this._parent = parent;

			_count = model.IterNChildren (parent);

			Reset ();
		}

		#region IEnumerable implementation

		public IEnumerator<Gtk.TreeIter> GetEnumerator ()
		{
			return this;
		}

		#endregion

		#region IEnumerable implementation

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
		{
			return GetEnumerator ();
		}

		#endregion

		#region IEnumerator implementation

		public bool MoveNext ()
		{
			if (++_index < _count) {
				if (_model.IterNthChild (out _current, _parent, _index)) {
					return true;
				}
			}
			return false;
		}

		public void Reset ()
		{
			_index = -1;
			_current = new Gtk.TreeIter ();
		}

		object System.Collections.IEnumerator.Current {
			get { return this.Current; }
		}

		#endregion

		#region IDisposable implementation

		public void Dispose ()
		{
			// Nothing to dispose of
		}

		#endregion

		#region IEnumerator implementation

		public Gtk.TreeIter Current {
			get { return _current; }
		}

		#endregion
	}
}
