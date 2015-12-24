using System;
using System.Collections.Generic;
using Gtk;

namespace GtkSharp.Extensions
{
	/// <summary>
	/// Tree view selected row enumerator; gets the TreeIter of the selected row.
	/// </summary>
	public class TreeViewSelectedRowEnumerator : IEnumerator<TreeIter>, IEnumerable<TreeIter>
	{
		TreeModel _model;

		/// <summary>
		/// Gets or sets the model.
		/// </summary>
		/// <value>The model.</value>
		public TreeModel Model {
			get { return _model; }
			set { _model = value; }
		}

		TreeIter _current;

		IEnumerator<TreePath> _pathEnumerator;

		/// <summary>
		/// Initializes a new instance of the <see cref="libGtkSharpTreeView.TreeViewSelectedRowEnumerator"/> class.
		/// </summary>
		/// <param name="tree">The TreeView widget; calls tree.Selection.GetSelectedRows.</param>
		public TreeViewSelectedRowEnumerator (TreeView tree)
		{
			if (tree == null)
				throw new ArgumentNullException ("tree");

			TreePath[] paths = tree.Selection.GetSelectedRows (out _model);

			_pathEnumerator = ((IEnumerable<TreePath>)paths).GetEnumerator ();

			_current = TreeIter.Zero;
		}

		#region IEnumerator implementation

		public bool MoveNext ()
		{
			// Try to move to the next path
			if (!_pathEnumerator.MoveNext ()) {
				return false;
			}

			// Continue getting the next item while the "GetIter" function fails
			while (!_model.GetIter (out _current, _pathEnumerator.Current)) {
				if (!_pathEnumerator.MoveNext ())
					return false;
			}
			return true;
		}

		public void Reset ()
		{
			_pathEnumerator.Reset ();
			_current = TreeIter.Zero;
		}

		object System.Collections.IEnumerator.Current { get { return this.Current; } }

		#endregion

		#region IDisposable implementation

		public void Dispose ()
		{
		}

		#endregion

		#region IEnumerator implementation

		public Gtk.TreeIter Current { get { return _current; } }

		#endregion

		#region IEnumerable implementation

		public IEnumerator<TreeIter> GetEnumerator ()
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
	}
}

