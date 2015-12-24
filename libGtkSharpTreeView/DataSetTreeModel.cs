using System;

namespace GtkSharp.Extensions
{
	// Source: http://www.mono-project.com/docs/gui/gtksharp/implementing-ginterfaces/
	// GitHub source code example: https://github.com/mono/gtk-sharp/blob/master/sample/TreeModelDemo.cs

	public class DataSetTreeModel : GLib.Object, Gtk.TreeModelImplementor
	{
		public DataSetTreeModel ()
		{
		}


		#region implementation for Gtk.TreeModelImplementor

		public GLib.GType GetColumnType (int index_)
		{
			throw new NotImplementedException ();
		}

		public bool GetIter (out Gtk.TreeIter iter, Gtk.TreePath path)
		{
			throw new NotImplementedException ();
		}

		public Gtk.TreePath GetPath (Gtk.TreeIter iter)
		{
			throw new NotImplementedException ();
		}

		public void GetValue (Gtk.TreeIter iter, int column, ref GLib.Value value)
		{
			throw new NotImplementedException ();
		}

		public bool IterNext (ref Gtk.TreeIter iter)
		{
			throw new NotImplementedException ();
		}

		public bool IterChildren (out Gtk.TreeIter iter, Gtk.TreeIter parent)
		{
			throw new NotImplementedException ();
		}

		public bool IterHasChild (Gtk.TreeIter iter)
		{
			throw new NotImplementedException ();
		}

		public int IterNChildren (Gtk.TreeIter iter)
		{
			throw new NotImplementedException ();
		}

		public bool IterNthChild (out Gtk.TreeIter iter, Gtk.TreeIter parent, int n)
		{
			throw new NotImplementedException ();
		}

		public bool IterParent (out Gtk.TreeIter iter, Gtk.TreeIter child)
		{
			throw new NotImplementedException ();
		}

		public void RefNode (Gtk.TreeIter iter)
		{
			throw new NotImplementedException ();
		}

		public void UnrefNode (Gtk.TreeIter iter)
		{
			throw new NotImplementedException ();
		}

		public Gtk.TreeModelFlags Flags {
			get { throw new NotImplementedException (); }
		}

		public int NColumns {
			get { throw new NotImplementedException (); }
		}

		#endregion
	}
}

