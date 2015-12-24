using System;

namespace GtkSharp.Extensions
{
	public static class ComboBoxExtensions
	{
		/// <summary>
		/// Sets the Selected item in the ComboBox to the first item in the model.
		/// </summary>
		/// <param name="widget">ComboBox Widget.</param>
		public static void SetActiveFirst (this Gtk.ComboBox widget)
		{
			if (widget == null)
				throw new ArgumentNullException ("widget");
			
			if (widget.Model == null)
				return;
			
			Gtk.TreeIter iter;
			if (widget.Model.GetIterFirst (out iter)) {
				widget.SetActiveIter (iter);
			}
		}

		/// <summary>
		/// Packs the cell to the start, and adds the given attribute to the cell.
		/// </summary>
		/// <param name="widget">Widget.</param>
		/// <param name="cell">Cell.</param>
		/// <param name="expand">If set to <c>true</c> expand.</param>
		/// <param name="attribute">Attribute.</param>
		/// <param name="columnIndex">Column index.</param>
		public static void PackCellStartAttributed (this Gtk.ComboBox widget, Gtk.CellRenderer cell, bool expand, string attribute, int columnIndex)
		{
			widget.PackStart (cell, expand);
			widget.AddAttribute (cell, attribute, columnIndex);
		}

		public static void PackStartCell (this Gtk.ComboBox combo, Gtk.CellRenderer cellRenderer, bool expand, Action<Gtk.CellRenderer, Gtk.TreeModel, Gtk.TreeIter> assignment)
		{
			combo.PackStart (cellRenderer, expand);
			combo.SetCellDataAction (cellRenderer, assignment);
		}

		public static void PackStartCell<TCell> (this Gtk.ComboBox combo, TCell cellRenderer, bool expand, Action<TCell, Gtk.TreeModel, Gtk.TreeIter> assignment) where TCell : Gtk.CellRenderer
		{
			combo.PackStart (cellRenderer, expand);
			combo.SetCellDataAction (cellRenderer, assignment);
		}

		public static void PackStartCell<TCell, TValue> (this Gtk.ComboBox combo, TCell cellRenderer, bool expand, int modelColumnIndex, Action<TCell, TValue> assignment) where TCell : Gtk.CellRenderer
		{
			combo.PackStart (cellRenderer, expand);
			combo.SetCellDataAction (cellRenderer, modelColumnIndex, assignment);
		}

		public static void PackStartCellText<TValue> (this Gtk.ComboBox combo, bool expandCell, int modelColumnIndex, Func<TValue, string> valueDisplay)
		{
			PackStartCellText (combo, new Gtk.CellRendererText (), expandCell, modelColumnIndex, valueDisplay);
		}

		public static void PackStartCellText<TValue> (this Gtk.ComboBox combo, Gtk.CellRendererText cellRenderer, bool expand, int modelColumnIndex, Func<TValue, string> valueDisplay)
		{
			combo.PackStart (cellRenderer, expand);
			combo.SetCellDataAction (cellRenderer, modelColumnIndex, (Gtk.CellRendererText cell, TValue value) => cell.Text = valueDisplay (value));
		}

		public static void SetCellDataAction (this Gtk.ComboBox combo, Gtk.CellRenderer cellRenderer, Action<Gtk.CellRenderer, Gtk.TreeModel,Gtk.TreeIter> assignment)
		{
			combo.SetCellDataFunc (cellRenderer, new Gtk.CellLayoutDataFunc (delegate(Gtk.CellLayout cell_layout, Gtk.CellRenderer cell, Gtk.TreeModel tree_model, Gtk.TreeIter iter) {
						assignment (cell, tree_model, iter);
					}));
		}

		public static void SetCellDataAction<TCell> (this Gtk.ComboBox combo, TCell cellRenderer, Action<TCell, Gtk.TreeModel,Gtk.TreeIter> assignment) where TCell : Gtk.CellRenderer
		{
			combo.SetCellDataFunc (cellRenderer, new Gtk.CellLayoutDataFunc (delegate(Gtk.CellLayout cell_layout, Gtk.CellRenderer cell, Gtk.TreeModel tree_model, Gtk.TreeIter iter) {
						assignment ((TCell)cell, tree_model, iter);
					}));
		}

		public static void SetCellDataAction<TCell, TValue> (this Gtk.ComboBox combo, TCell cellRenderer, int modelColumnIndex, Action<TCell, TValue> assignment) where TCell : Gtk.CellRenderer
		{
			combo.SetCellDataFunc (cellRenderer, new Gtk.CellLayoutDataFunc (delegate(Gtk.CellLayout cell_layout, Gtk.CellRenderer cell, Gtk.TreeModel tree_model, Gtk.TreeIter iter) {
						assignment ((TCell)cell, (TValue)tree_model.GetValue (iter, modelColumnIndex));
					}));
		}

		/// <summary>
		/// Sets the model as the given collection, and sets it to display text using the given function.
		/// </summary>
		/// <param name="combo">Combo.</param>
		/// <param name="collection">Collection.</param>
		/// <param name="displayTextFunction">Display text function.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static void SetModelTextDisplay<T> (this Gtk.ComboBox combo, System.Collections.Generic.IEnumerable<T> collection, Func<T, string> displayTextFunction)
		{
			var textCell = new Gtk.CellRendererText ();

			combo.PackStart (textCell, true);
			combo.SetCellDataAction (textCell, delegate(Gtk.CellRenderer cell, Gtk.TreeModel tree_model, Gtk.TreeIter iter) {
					((Gtk.CellRendererText)cell).Text = displayTextFunction ((T)tree_model.GetValue (iter, 0));
				});

			var store = new Gtk.ListStore (typeof(T));
			foreach (var item in collection) {
				store.AppendValues (item);
			}

			combo.Model = store;
		}

		/// <summary>
		/// Gets the selected value of the ComboBox (assumes model column index of zero).
		/// </summary>
		/// <returns><c>true</c>, if selected value was gotten, <c>false</c> otherwise.</returns>
		/// <param name="combo">Combo.</param>
		/// <param name="value">Value.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static bool GetSelectedValue<T> (this Gtk.ComboBox combo, out T value)
		{
			return GetSelectedValue (combo, 0, out value);
		}

		public static bool GetSelectedValue<T> (this Gtk.ComboBox combo, int modelIndex, out T value)
		{
			Gtk.TreeIter iter;
			if (combo.GetActiveIter (out iter)) {
				value = (T)combo.Model.GetValue (iter, modelIndex);
				return true;
			}
			value = default(T);
			return false;
		}
	}
}

