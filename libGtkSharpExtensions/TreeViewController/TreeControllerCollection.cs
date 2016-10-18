using System;
using System.Collections.Generic;
using System.Linq;

using Gtk;

namespace GtkSharp.Extensions
{
	/// <summary>
	/// Basic TreeView Controller, where the model is a single-column set of objects of type T, built from a given collection. 
	/// It can optionally Automatically load the TreeView, based on the properties of the object type T
	/// </summary>
	public class TreeControllerCollection<T>
	{
		/// <summary>
		/// Gets or sets the TreeView widget.
		/// This must be assigned during, or immediately after, the constructor call for this object
		/// </summary>
		/// <value>The view.</value>
		public TreeView View { get; set; }

		/// <summary>
		/// Gets or sets the TreeView model object, which stores the data for the TreeView.
		/// </summary>
		/// <value>The model.</value>
		public ListStore Model { get; set; }

		/// <summary>
		/// Gets or sets the field includer.
		/// </summary>
		/// <value>The field includer.</value>
		public IFieldInclusion FieldIncluder { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="libGtkSharpTreeView.TreeViewManagerBasic`1"/> class.
		/// IMPORTANT: Make sure to assign the View property with an instantiated TreeView widget.
		/// </summary>
		public TreeControllerCollection ()
		{
			this.FieldIncluder = new FieldInclusion ();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="libGtkSharpTreeView.TreeViewManagerBasic`1"/> class.
		/// </summary>
		/// <param name="view">The TreeView widget (must be instantiated).</param>
		public TreeControllerCollection (TreeView view)
			: this ()
		{
			if (view == null)
				throw new ArgumentNullException ("view");
			
			this.View = view;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="libGtkSharpTreeView.TreeViewManagerBasic`1"/> class.
		/// </summary>
		/// <param name="view">View.</param>
		/// <param name="collection">Collection of items to display in the TreeView.</param>
		/// <param name="autoPopulateColumns">If set to <c>true</c> then automatically populates the TreeView with columns for each property of <c>T</c>.
		/// If <c>false</c> then only creates the model</param>
		public TreeControllerCollection (TreeView view, IEnumerable<T> collection, bool autoPopulateColumns)
			: this (view)
		{
			if (autoPopulateColumns) {
				AutoPopulate (collection);
			} else {
				CreateAndAssignModel (collection);
			}
		}


		/// <summary>
		/// Automatically populate the TreeView by creating a new model, and assigning a text column for each property of the object.
		/// Uses Reflection to determine properties.
		/// </summary>
		/// <param name="collection">Collection.</param>
		public void AutoPopulate (IEnumerable<T> collection)
		{
			CreateAndAssignModel (collection);

			// Add a text column for each property
			foreach (var prop in typeof(T).GetProperties ()) {
				if (this.FieldIncluder.Included (prop.Name))
					AddTextColumn (prop.Name, x => prop.GetValue (x, null).ToDefaultableString (String.Empty));
			}
		}

		/// <summary>
		/// Creates the model and assigns it to the TreeView.
		/// </summary>
		/// <returns>The and assign model.</returns>
		/// <param name="collection">Collection.</param>
		public ListStore CreateAndAssignModel (IEnumerable<T> collection)
		{
			var model = CreateModel (collection);

			this.Model = model;
			this.View.Model = model;

			return model;
		}

		/// <summary>
		/// Creates the model, based on the Collection of objects.
		/// </summary>
		/// <returns>The model.</returns>
		/// <param name="collection">Collection.</param>
		public ListStore CreateModel (IEnumerable<T> collection)
		{
			var model = new ListStore (typeof(T));

			foreach (var item in collection) {
				model.AppendValues (item);
			}

			return model;
		}



		/// <summary>
		/// Adds a new text column to the TreeView.
		/// </summary>
		/// <param name="columnTitle">Column title.</param>
		/// <param name="cellTextGetter">Cell text getter.</param>
		public virtual TreeViewColumn AddTextColumn (string columnTitle, Func<T, string> cellTextGetter)
		{
			var newColumn = CreateColumn (columnTitle, cellTextGetter);

			View.AppendColumn (newColumn);

			return newColumn;
		}

		/// <summary>
		/// Gets the column, which displays cell text.
		/// </summary>
		/// <returns>The column.</returns>
		/// <param name="columnTitle">Column title.</param>
		/// <param name="cellTextGetter">Cell text getter.</param>
		public virtual TreeViewColumn CreateColumn (string columnTitle, Func<T, string> cellTextGetter)
		{
			var col = new TreeViewColumn ();
			col.Title = columnTitle;

			var cellText1 = new CellRendererText ();
			col.PackStart (cellText1, true);

			// Set the function for assigning the Cell data, according to the current object
			col.SetCellDataFunc (cellText1
				, new TreeCellDataFunc (
					delegate (TreeViewColumn tree_column, CellRenderer cell, TreeModel tree_model, TreeIter iter) {
						((CellRendererText)cell).Text = cellTextGetter ((T)tree_model.GetValue (iter, 0));
					}
				)
			);

			return col;
		}


		/// <summary>
		/// Tries the get selected item.
		/// </summary>
		/// <returns><c>true</c>, if get selected item was tryed, <c>false</c> otherwise.</returns>
		/// <param name="selectedItem">Selected item.</param>
		public virtual bool TryGetSelectedItem (out T selectedItem)
		{
			selectedItem = default(T);

			TreeIter iter;
			if (View.Selection.GetSelected (out iter)) {
				selectedItem = (T)View.Model.GetValue (iter, 0);
				return true;
			}
			return false;
		}

		/// <summary>
		/// Gets the selected items.
		/// </summary>
		/// <returns>The selected items.</returns>
		public virtual T[] GetSelectedItems ()
		{
			var items = new List<T> ();

			var paths = View.Selection.GetSelectedRows ();
			foreach (var path in paths) {
				TreeIter iter;
				if (View.Model.GetIter (out iter, path)) {
					items.Add ((T)View.Model.GetValue (iter, 0));
				}
			}

			return items.ToArray ();
		}

		public virtual bool FindItem (T item, IEqualityComparer<T> comparer, out TreeIter iter)
		{
			return Model.TryFindIter (0, item, comparer, out iter);
		}

		public virtual bool FindItem (out TreeIter iter, Func<T, bool> isItemFound)
		{
			return Model.TryFindIter (0, isItemFound, out iter);
		}
	}
}

