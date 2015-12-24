using System;
using System.Collections.Generic;

namespace GtkSharp.Extensions
{
	public class TreeModelItemEnumerator : IEnumerator<Gtk.TreeIter>, IEnumerable<Gtk.TreeIter>
	{
		private Gtk.TreeModel _model;

		private Gtk.TreeIter _current;

		private bool _isMoveFirst = true;

		public TreeModelItemEnumerator (Gtk.TreeModel model)
		{
			if (model == null)
				throw new ArgumentNullException ("model");
			
			this._model = model;

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
			if (_isMoveFirst) {
				_isMoveFirst = false;
				return _model.GetIterFirst (out _current);
			}
			return _model.IterNext(ref _current);
		}

		public void Reset ()
		{
			_isMoveFirst = true;
		}

		object System.Collections.IEnumerator.Current {
			get { return this.Current; }
		}

		#endregion

		#region IDisposable implementation

		public void Dispose ()
		{
			// Nothing to do
		}

		#endregion

		#region IEnumerator implementation

		public Gtk.TreeIter Current {
			get { return _current; }
		}

		#endregion
	}
}
