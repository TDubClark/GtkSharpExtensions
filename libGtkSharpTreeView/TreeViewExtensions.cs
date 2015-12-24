using System;
using Gtk;
using System.Collections.Generic;

namespace GtkSharp.Extensions
{
	/// <summary>
	/// Extensions for the Gtk# TreeView widget.
	/// </summary>
	public static class TreeViewExtensions
	{
		/// <summary>
		/// Gets the model object of the selected item (the value at model index zero).
		/// </summary>
		/// <returns><c>true</c>, if selected model object was gotten, <c>false</c> otherwise.</returns>
		/// <param name="treeView">Tree view.</param>
		/// <param name="selectedObject">Selected object.</param>
		public static bool GetSelectedModelObject (this Gtk.TreeView treeView, out object selectedObject)
		{
			Gtk.TreeIter iter;
			if (treeView.Selection.GetSelected (out iter)) {
				selectedObject = treeView.Model.GetValue (iter, 0);
				return true;
			}
			selectedObject = null;
			return false;
		}

		/// <summary>
		/// Gets the selected model object at the given column index.
		/// </summary>
		/// <returns><c>true</c>, if selected model object was gotten, <c>false</c> otherwise.</returns>
		/// <param name="treeView">Tree view.</param>
		/// <param name="selectedObject">Selected object.</param>
		/// <param name="modelColumnIndex">Model column index.</param>
		public static bool GetSelectedModelObject (this Gtk.TreeView treeView, out object selectedObject, int modelColumnIndex)
		{
			Gtk.TreeIter iter;
			if (treeView.Selection.GetSelected (out iter)) {
				selectedObject = treeView.Model.GetValue (iter, modelColumnIndex);
				return true;
			}
			selectedObject = null;
			return false;
		}

		/// <summary>
		/// Sets the selected item in this combobox, based on the given function.
		/// </summary>
		/// <param name="combo">Combo.</param>
		/// <param name="isSelectedFunc">Gets whether the item in the given model, at the given iterator, should be selected.</param>
		public static void SetSelectedItem (this Gtk.ComboBox combo, Func<Gtk.TreeModel, Gtk.TreeIter, bool> isSelectedFunc)
		{
			combo.Model.FindItem (delegate(Gtk.TreeModel argModel, Gtk.TreeIter argIter) {
					if (isSelectedFunc (argModel, argIter)) {
						combo.SetActiveIter (argIter);
						return true;
					}
					return false;
				});
		}


		#region TreeView - AppendColumn methods

		/// <summary>
		/// Appends the given collection of columns to this TreeView.
		/// </summary>
		/// <param name="tree">Tree.</param>
		/// <param name="columnCollection">Column collection.</param>
		public static void AppendColumns (this TreeView tree, IEnumerable<TreeViewColumn> columnCollection)
		{
			if (columnCollection == null)
				throw new ArgumentNullException ("columnCollection");
			foreach (var column in columnCollection) {
				tree.AppendColumn (column);
			}
		}

		/// <summary>
		/// Appends a new column to the tree.
		/// </summary>
		/// <param name="tree">Tree.</param>
		/// <param name="title">The column Title.</param>
		/// <param name="modelColumn">The Model column index from which to get the value.</param>
		/// <param name="setCellValue">An action which sets the cell value from the given object (retrieved from the model at column index [modelColumn]).</param>
		/// <typeparam name="CR">The 1st type parameter.</typeparam>
		public static TreeViewColumn AppendColumn<CR> (this TreeView tree, string title, int modelColumn, Action<CR, object> setCellValue) where CR : CellRenderer, new()
		{
			return AppendColumn (tree, title, new CR (), modelColumn, setCellValue);
		}

		/// <summary>
		/// Appends a new column to the tree.
		/// </summary>
		/// <param name="tree">Tree.</param>
		/// <param name="title">The column Title.</param>
		/// <param name="cellRenderer">Cell renderer.</param>
		/// <param name="modelColumn">The Model column index from which to get the value.</param>
		/// <param name="setCellValue">An action which sets the cell value from the given object (retrieved from the model at column index [modelColumn]).</param>
		/// <typeparam name="CR">The type of CellRenderer.</typeparam>
		public static TreeViewColumn AppendColumn<CR> (this TreeView tree, string title, CR cellRenderer, int modelColumn, Action<CR, object> setCellValue) where CR : CellRenderer
		{
			return tree.AppendColumn (title, cellRenderer, new TreeCellDataFunc (
					delegate(TreeViewColumn tree_column, CellRenderer cell, TreeModel tree_model, TreeIter iter) {
						setCellValue (((CR)cell), tree_model.GetValue (iter, modelColumn));
					}
				));
		}

		/// <summary>
		/// Appends a new column to the tree.
		/// </summary>
		/// <param name="tree">Tree.</param>
		/// <param name="title">The column Title.</param>
		/// <param name="modelColumn">The Model column index from which to get the value.</param>
		/// <param name="setCellValue">An action which sets the cell value from the given object (retrieved from the model at column index [modelColumn]).</param>
		/// <typeparam name="CR">The type of CellRenderer.</typeparam>
		/// <typeparam name="VAL">The type of value retrieved from the model at index [modelColumn].</typeparam>
		public static TreeViewColumn AppendColumn<CR, VAL> (this TreeView tree, string title, int modelColumn, Action<CR, VAL> setCellValue) where CR : CellRenderer, new()
		{
			return AppendColumn (tree, title, new CR (), modelColumn, setCellValue);
		}

		/// <summary>
		/// Appends a new column to the tree.
		/// </summary>
		/// <param name="tree">Tree.</param>
		/// <param name="title">The column Title.</param>
		/// <param name="cellRenderer">Cell renderer.</param>
		/// <param name="modelColumn">The Model column index from which to get the value.</param>
		/// <param name="setCellValue">An action which sets the cell value from the given object (retrieved from the model at column index [modelColumn]).</param>
		/// <typeparam name="CR">The type of CellRenderer.</typeparam>
		/// <typeparam name="VAL">The type of value retrieved from the model at index [modelColumn].</typeparam>
		public static TreeViewColumn AppendColumn<CR, VAL> (this TreeView tree, string title, CR cellRenderer, int modelColumn, Action<CR, VAL> setCellValue) where CR : CellRenderer
		{
			return tree.AppendColumn (title, cellRenderer, new TreeCellDataFunc (
					delegate(TreeViewColumn tree_column, CellRenderer cell, TreeModel tree_model, TreeIter iter) {
						setCellValue (((CR)cell), (VAL)tree_model.GetValue (iter, modelColumn));
					}
				));
		}

		/// <summary>
		/// Appends a new column to the tree, with the given Toggle CellRenderer.
		/// </summary>
		/// <param name="tree">Tree.</param>
		/// <param name="title">The column Title.</param>
		/// <param name="cellRenderer">Cell renderer.</param>
		/// <param name="modelColumn">The Model column index from which to get the value.</param>
		/// <param name="setCellValue">An action which sets the cell value from the given object (retrieved from the model at column index [modelColumn]).</param>
		/// <param name="handleCellToggled">An action which occurs when the cell is toggled; parameter is the model value at the edited iter for the given modelColumn.</param>
		/// <typeparam name="CR">The type of CellRenderer.</typeparam>
		/// <typeparam name="VAL">The type of value retrieved from the model at index [modelColumn].</typeparam>
		public static TreeViewColumn AppendColumnEditableToggle<VAL> (this TreeView tree, string title, Gtk.CellRendererToggle cellRenderer, int modelColumn
			, Action<CellRendererToggle, VAL> setCellValue
			, Action<VAL> handleCellToggled
		)
		{
			cellRenderer.Activatable = true;
			var column = AppendColumn (tree, title, cellRenderer, modelColumn, setCellValue);

			cellRenderer.Toggled += (object o, ToggledArgs args) => {
				TreeIter iter;
				if (tree.Model.GetIter (out iter, new TreePath (args.Path))) {
					handleCellToggled ((VAL)tree.Model.GetValue (iter, modelColumn));
				}
			};

			return column;
		}

		/// <summary>
		/// Appends a new column to the tree, with the given Text CellRenderer.
		/// </summary>
		/// <param name="tree">Tree.</param>
		/// <param name="title">The column Title.</param>
		/// <param name="cellRenderer">Cell renderer.</param>
		/// <param name="modelColumn">The Model column index from which to get the value.</param>
		/// <param name="setCellValue">An action which sets the cell value from the given object (retrieved from the model at column index [modelColumn]).</param>
		/// <param name="handleCellEdited">An action which occurs when the text of the cell is edited; first parameter is the model value at the edited iter for the given modelColumn, second parameter is the new text.</param>
		/// <typeparam name="CR">The type of CellRenderer.</typeparam>
		/// <typeparam name="VAL">The type of value retrieved from the model at index [modelColumn].</typeparam>
		public static TreeViewColumn AppendColumnEditableText<VAL> (this TreeView tree, string title, CellRendererText cellRenderer, int modelColumn
			, Action<CellRendererText, VAL> setCellValue
			, Action<VAL, string> handleCellEdited
		)
		{
			cellRenderer.Editable = true;
			var column = AppendColumn (tree, title, cellRenderer, modelColumn, setCellValue);

			cellRenderer.Edited += (object o, EditedArgs args) => {
				TreeIter iter;
				if (tree.Model.GetIter (out iter, new TreePath (args.Path))) {
					handleCellEdited ((VAL)tree.Model.GetValue (iter, modelColumn), args.NewText);
				}
			};

			return column;
		}

		/// <summary>
		/// Appends a new column to the tree, with the given Spin CellRenderer.
		/// </summary>
		/// <param name="tree">Tree.</param>
		/// <param name="title">The column Title.</param>
		/// <param name="cellRenderer">Cell renderer.</param>
		/// <param name="modelColumn">The Model column index from which to get the value.</param>
		/// <param name="setCellValue">An action which sets the cell value from the given object (retrieved from the model at column index [modelColumn]).</param>
		/// <param name="handleCellEdited">An action which occurs when the text of the cell is edited; first parameter is the model value at the edited iter for the given modelColumn, second parameter is the new text.</param>
		/// <typeparam name="CR">The type of CellRenderer.</typeparam>
		/// <typeparam name="VAL">The type of value retrieved from the model at index [modelColumn].</typeparam>
		public static TreeViewColumn AppendColumnEditableSpin<VAL> (this TreeView tree, string title, CellRendererSpin cellRenderer, int modelColumn
			, Action<CellRendererSpin, VAL> setCellValue
			, Action<VAL, string> handleCellEdited
		)
		{
			// CellRendererSpin inherits from CellRendererText
			return AppendColumnEditableText<VAL> (tree, title, cellRenderer, modelColumn, (CellRendererText arg1, VAL arg2) => setCellValue ((CellRendererSpin)arg1, arg2), handleCellEdited);
		}

		/// <summary>
		/// Appends a new column to the tree, with the given Text CellRenderer.
		/// </summary>
		/// <param name="tree">Tree.</param>
		/// <param name="title">The column Title.</param>
		/// <param name="cellRenderer">Cell renderer.</param>
		/// <param name="modelColumn">The Model column index from which to get the value.</param>
		/// <param name="setCellValue">An action which sets the cell value from the given object (retrieved from the model at column index [modelColumn]).</param>
		/// <param name="handleCellEdited">An action which occurs when the text of the cell is edited; first parameter is the model value at the edited iter for the given modelColumn, second parameter is the new text.</param>
		/// <typeparam name="CR">The type of CellRenderer.</typeparam>
		/// <typeparam name="VAL">The type of value retrieved from the model at index [modelColumn].</typeparam>
		public static TreeViewColumn AppendColumnEditableCombo<VAL> (this TreeView tree, string title, CellRendererCombo cellRenderer, int modelColumn
			, Action<CellRendererCombo, VAL> setCellValue
			, Action<VAL, string> handleCellEdited
		)
		{
			// CellRendererCombo inherits from CellRendererText
			return AppendColumnEditableText<VAL> (tree, title, cellRenderer, modelColumn, (CellRendererText arg1, VAL arg2) => setCellValue ((CellRendererCombo)arg1, arg2), handleCellEdited);
		}

		#endregion


		/// <summary>
		/// Gets the selected items from column at index zero.
		/// </summary>
		/// <returns>The selected items.</returns>
		/// <param name="tree">Tree.</param>
		public static object[] GetSelectedItems (this TreeView tree)
		{
			return tree.GetSelectedItems (0);
		}

		/// <summary>
		/// Gets the selected items from the given column index.
		/// </summary>
		/// <returns>The selected items.</returns>
		/// <param name="tree">Tree.</param>
		/// <param name="valueColumn">Value column index.</param>
		public static object[] GetSelectedItems (this TreeView tree, int valueColumn)
		{
			TreeModel model1;
			TreeIter iter;

			var lstSelected = new List<object> ();

			var paths = tree.Selection.GetSelectedRows (out model1);
			foreach (var path in paths) {
				if (model1.GetIter (out iter, path)) {
					lstSelected.Add (model1.GetValue (iter, valueColumn));
				}
			}

			return lstSelected.ToArray ();
		}

		/// <summary>
		/// Gets the selected rows as an enumerable object (implements IEnumerable<TreeIter>).
		/// </summary>
		/// <returns>The selected rows.</returns>
		/// <param name="tree">Tree.</param>
		/// <param name="model">The TreeView Model.</param>
		public static IEnumerable<TreeIter> GetSelectedRows (this TreeView tree, out TreeModel model)
		{
			return GetSelectedRowsEnumerator (tree, out model);
		}

		/// <summary>
		/// Gets the selected rows as an enumerable object (implements IEnumerator<TreeIter>).
		/// </summary>
		/// <returns>The selected rows enumerator.</returns>
		/// <param name="tree">Tree.</param>
		/// <param name="model">The TreeView Model.</param>
		public static TreeViewSelectedRowEnumerator GetSelectedRowsEnumerator (this TreeView tree, out TreeModel model)
		{
			var enumerator = new TreeViewSelectedRowEnumerator (tree);
			model = enumerator.Model;
			return enumerator;
		}
	}
}

