using System;
using System.Collections.Generic;
using System.Linq;
using Gtk;

using GtkSharp.Extensions;

namespace GtkSharp.Win
{
	public interface IObjectCreator<T>
	{
		T CreateNew ();
	}

	public class gwinCollectionEditor<T> : Dialog where T : new()
	{
		public IList<T> ValueCollection { get; set; }

		IObjectCreator<T> _creator;

		TreeView _tree;
		ListStore _treeStore;

		public bool Saved { get; set; }

		/// <summary>
		/// Saves the items to the collection.
		/// </summary>
		void Save ()
		{
			ValueCollection.Clear ();
			foreach (var iter in _treeStore.GetParentItems ()) {
				ValueCollection.Add ((T)_treeStore.GetValue (iter, 0));
			}
			Saved = true;
		}

		/// <summary>
		/// Gets or sets the editable property names.
		/// </summary>
		/// <value>The editable property names.</value>
		public List<string> EditablePropertyNames { get; set; }

		/// <summary>
		/// Gets or sets whether columns are editable by default.
		/// </summary>
		/// <value><c>true</c> if editable default; otherwise, <c>false</c>.</value>
		public bool EditableDefault { get; set; }

		/// <summary>
		/// Gets or sets the property name comparer.
		/// </summary>
		/// <value>The property name comparer.</value>
		public IEqualityComparer<string> PropertyNameComparer { get; set; }


		/// <summary>
		/// Determines whether the given property should be editable.
		/// </summary>
		/// <returns><c>true</c> if this instance is editable property the specified propName; otherwise, <c>false</c>.</returns>
		/// <param name="propName">Property name.</param>
		bool IsEditableProp (string propName)
		{
			if (EditablePropertyNames.Count > 0) {
				// At least one item was added to the editable property names.
				if (EditablePropertyNames.Contains (propName, PropertyNameComparer))
					return true;
			}

			// Edit any parsable field
			if (ColumnParsers.ContainsKey (propName))
				return true;
			
			return EditableDefault;
		}

		protected gwinCollectionEditor (string title, Window parent, IEqualityComparer<string> propertyNameComparer)
			: base (title, parent, DialogFlags.Modal, "Save", ResponseType.Accept, "Close", ResponseType.Cancel)
		{
			this.PropertyNameComparer = propertyNameComparer;
			this.ColumnParsers = new Dictionary<string, Func<string, object>> (propertyNameComparer);
			this.ColumnDisplayers = new Dictionary<string, Func<object, string>> (propertyNameComparer);
			this.EditablePropertyNames = new List<string> ();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="libGtkSharpWin.gcwinCollectionEditor`1"/> class.
		/// </summary>
		/// <param name="title">Title.</param>
		/// <param name="parent">Parent.</param>
		/// <param name="propertyNameComparer">Property name comparer.</param>
		/// <param name="collection">Collection.</param>
		public gwinCollectionEditor (string title, Window parent, IEqualityComparer<string> propertyNameComparer, IList<T> collection)
			: this (title, parent, propertyNameComparer, collection, null, false, false)
		{
		}

		public gwinCollectionEditor (string title, Window parent, IEqualityComparer<string> propertyNameComparer, IList<T> collection, bool forceEditable)
			: this (title, parent, propertyNameComparer, collection, null, forceEditable, forceEditable)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="libGtkSharpWin.gcwinCollectionEditor`1"/> class.
		/// </summary>
		/// <param name="title">Title.</param>
		/// <param name="parent">Parent.</param>
		/// <param name="propertyNameComparer">Property name comparer.</param>
		/// <param name="collection">Collection.</param>
		/// <param name="creator">Creator.</param>
		public gwinCollectionEditor (string title, Window parent, IEqualityComparer<string> propertyNameComparer, IList<T> collection, IObjectCreator<T> creator, bool forceEditable, bool editableDefault)
			: this (title, parent, propertyNameComparer)
		{
			this.ValueCollection = collection;
			this._creator = creator;

			this.EditableDefault = editableDefault;

			Build (600, 480, forceEditable);

			foreach (var item in collection) {
				_treeStore.AppendValues (item);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.gcwinCollectionEditor`1"/> class.
		/// </summary>
		/// <param name="title">Title.</param>
		/// <param name="parent">Parent.</param>
		/// <param name="propertyNameComparer">Property name comparer.</param>
		/// <param name="collection">Collection.</param>
		/// <param name="creator">Creator.</param>
		/// <param name="columnParsers">Column parser functions.</param>
		/// <param name="columnDisplayers">Column Displayer functions.</param>
		public gwinCollectionEditor (string title, Window parent, IEqualityComparer<string> propertyNameComparer
			, IList<T> collection
			, IObjectCreator<T> creator
			, Dictionary<string, Func<string, object>> columnParsers
			, Dictionary<string, Func<object, string>> columnDisplayers)
			: this (title, parent, propertyNameComparer, collection, creator, columnParsers, columnDisplayers, new string[] { })
		{
		}

		public gwinCollectionEditor (string title, Window parent, IEqualityComparer<string> propertyNameComparer
			, IList<T> collection
			, IObjectCreator<T> creator
			, Dictionary<string, Func<string, object>> columnParsers
			, Dictionary<string, Func<object, string>> columnDisplayers
			, string[] editablePropNames)
			: this (title, parent, propertyNameComparer)
		{
			if (columnParsers == null)
				throw new ArgumentNullException ("columnParsers");
			if (columnDisplayers == null)
				throw new ArgumentNullException ("columnDisplayers");

			this.ValueCollection = collection;
			this._creator = creator;
			this.ColumnParsers = columnParsers;
			this.ColumnDisplayers = columnDisplayers;

			this.EditablePropertyNames.AddRange (editablePropNames);

			Build (600, 480, false);

			foreach (var item in collection) {
				_treeStore.AppendValues (item);
			}
		}

		Dictionary<int, System.Reflection.PropertyInfo> _columnProperties;


		/// <summary>
		/// Gets or sets the column input parser functions, each according to property name.
		/// </summary>
		/// <value>The column parsers.</value>
		public Dictionary<string, Func<string, object>> ColumnParsers { get; set; }

		/// <summary>
		/// Gets or sets the column displayer functions, each according to property name.
		/// </summary>
		/// <value>The column displayers.</value>
		public Dictionary<string, Func<object, string>> ColumnDisplayers { get; set; }

		void Build (int width, int height, bool forceEditable)
		{
			this.SetSizeRequest (width, height);
			this.Modal = true;

			int uneditableColumnCount = 0;

			_columnProperties = new Dictionary<int, System.Reflection.PropertyInfo> ();


			_treeStore = new ListStore (typeof(T));

			_tree = new TreeView (_treeStore);

			CellRenderer treeCell = null;
			TreeCellDataFunc func = null;

			var props = typeof(T).GetProperties ();
			foreach (var prop in props) {

				if (prop.PropertyType.Equals (typeof(int))) {
					var spinCell = new CellRendererSpin ();
					spinCell.Editable = IsEditableProp (prop.Name);
					spinCell.Digits = 0;
					spinCell.Adjustment = new Adjustment (0.0, Int32.MinValue, Int32.MaxValue, 1.0, 1.0, 1.0);
//					spinCell.Adjustment.StepIncrement = 0.0;
//					spinCell.Adjustment.Upper = Int32.MaxValue;
//					spinCell.Adjustment.Lower = Int32.MinValue;
					spinCell.Edited += SpinCellEdited;

					treeCell = spinCell;

					func = new TreeCellDataFunc (SpinCellFunction);
				} else if (prop.PropertyType.Equals (typeof(bool))) {
					var cellToggle = new CellRendererToggle ();
					cellToggle.Activatable = IsEditableProp (prop.Name);
					cellToggle.Toggled += ToggleCellToggled;

					treeCell = cellToggle;

					func = new TreeCellDataFunc (ToggleCellFunction);
				} else if (prop.PropertyType.Equals (typeof(string))) {
					var cellText = new CellRendererText ();
					cellText.Editable = IsEditableProp (prop.Name);
					cellText.Edited += TextCellEdited;

					treeCell = cellText;

					func = new TreeCellDataFunc (TextCellFunction);
				} else {
					var cellText = new CellRendererText ();
					cellText.Editable = ColumnParsers.ContainsKey (prop.Name);
					if (!cellText.Editable)
						uneditableColumnCount++;
					cellText.Edited += TextCellEdited;

					treeCell = cellText;

					func = new TreeCellDataFunc (TextCellFunction);
				}

				_columnProperties.Add (treeCell.Handle.ToInt32 (), prop);

				treeCell.Sensitive = true;

				// Set background-color to light gray
				const int clrGray = (15 * 16) + 3;
				treeCell.CellBackgroundGdk = new Gdk.Color (clrGray, clrGray, clrGray);

				var treeCol = new TreeViewColumn ();
				treeCol.Reorderable = true;
				treeCol.Resizable = true;
				treeCol.SortIndicator = true;

				treeCol.Title = prop.Name;
				treeCol.PackStart (treeCell, false);
				treeCol.SetCellDataFunc (treeCell, func);

				_tree.AppendColumn (treeCol);
			}



			HButtonBox buttons = new HButtonBox ();
			buttons.Spacing = 6;
			buttons.Layout = ButtonBoxStyle.Start;

			if (uneditableColumnCount == 0 || forceEditable) {
				buttons.PackStartButton ("Add", BtnAdd_Clicked, false, false, 0);
				buttons.PackStartButton ("Insert", BtnInsert_Clicked, false, false, 0);
				buttons.PackStartButton ("Delete", BtnDelete_Clicked, false, false, 0);
			}

			ScrolledWindow scroller = new ScrolledWindow ();
			scroller.Add (_tree);

			this.VBox.PackStart (scroller, true, true, 0);
			this.VBox.PackEnd (buttons.NestInHBox (0, false, false, 6), false, false, 6);

			this.Response += GcwinCollectionEditor_Response;

			if (this.Child != null) {
				this.Child.ShowAll ();
			}
		}

		void GcwinCollectionEditor_Response (object o, ResponseArgs args)
		{
			if (args.ResponseId == ResponseType.Accept) {
				Save ();
			}
		}

		string GetColumnDisplay (object value, string columnName)
		{
			Func<object, string> displayer;
			if (ColumnDisplayers.TryGetValue (columnName, out displayer)) {
				return displayer (value);
			}
			return value == null ? "" : value.ToString ();
		}

		/// <summary>
		/// A cell data function for a Spin cell
		/// </summary>
		/// <param name="tree_column">Tree column.</param>
		/// <param name="cell">Cell.</param>
		/// <param name="tree_model">Tree model.</param>
		/// <param name="iter">Iter.</param>
		void SpinCellFunction (TreeViewColumn tree_column, CellRenderer cell, TreeModel tree_model, TreeIter iter)
		{
			CellDataFunction (cell, tree_model, iter, 0, (CellRenderer oCell, object value) => ((CellRendererSpin)oCell).Adjustment.Value = (int)value);
		}

		/// <summary>
		/// A cell data function for a Text cell
		/// </summary>
		/// <param name="tree_column">Tree column.</param>
		/// <param name="cell">Cell.</param>
		/// <param name="tree_model">Tree model.</param>
		/// <param name="iter">Iter.</param>
		void TextCellFunction (TreeViewColumn tree_column, CellRenderer cell, TreeModel tree_model, TreeIter iter)
		{
			CellDataFunction (cell, tree_model, iter, "", (CellRenderer oCell, object value, string columnName) => ((CellRendererText)oCell).Text = GetColumnDisplay (value, columnName));
		}

		/// <summary>
		/// A cell data function for a Toggle cell
		/// </summary>
		/// <param name="tree_column">Tree column.</param>
		/// <param name="cell">Cell.</param>
		/// <param name="tree_model">Tree model.</param>
		/// <param name="iter">Iter.</param>
		void ToggleCellFunction (TreeViewColumn tree_column, CellRenderer cell, TreeModel tree_model, TreeIter iter)
		{
			CellDataFunction (cell, tree_model, iter, false, (CellRenderer oCell, object value) => ((CellRendererToggle)oCell).Active = (bool)value);
		}

		/// <summary>
		/// The generic function for assigning data to a cell.
		/// </summary>
		/// <param name="cell">Cell.</param>
		/// <param name="tree_model">Tree model.</param>
		/// <param name="iter">Iter.</param>
		/// <param name="cellAssign">Cell assign.</param>
		void CellDataFunction (CellRenderer cell, TreeModel tree_model, TreeIter iter, object defaultValue, Action<CellRenderer, object> cellAssign)
		{
			System.Reflection.PropertyInfo prop;
			if (_columnProperties.TryGetValue (cell.Handle.ToInt32 (), out prop)) {
				var value = prop.GetValue ((T)tree_model.GetValue (iter, 0), null);
				if (value != null)
					cellAssign (cell, value);
				else
					cellAssign (cell, defaultValue);
			}
		}

		/// <summary>
		/// The generic function for assigning data to a cell.
		/// </summary>
		/// <param name="cell">Cell.</param>
		/// <param name="tree_model">Tree model.</param>
		/// <param name="iter">Iter.</param>
		/// <param name="cellAssign">Cell assign, according to property name.</param>
		void CellDataFunction (CellRenderer cell, TreeModel tree_model, TreeIter iter, object defaultValue, Action<CellRenderer, object, string> cellAssign)
		{
			System.Reflection.PropertyInfo prop;
			if (_columnProperties.TryGetValue (cell.Handle.ToInt32 (), out prop)) {
				var value = prop.GetValue ((T)tree_model.GetValue (iter, 0), null);
				if (value != null)
					cellAssign (cell, value, prop.Name);
				else
					cellAssign (cell, defaultValue, prop.Name);
			}
		}

		void SpinCellEdited (object o, EditedArgs args)
		{
			var cell = (CellRendererSpin)o;
			var newValue = Int32.Parse (args.NewText); // (int)cell.Adjustment.Value
			CellEditedFunction (cell, args.Path, newValue);
		}

		void TextCellEdited (object o, EditedArgs args)
		{
			var cell = (CellRendererText)o;
			CellEditedFunction (cell, args.Path, args.NewText);
		}

		void ToggleCellToggled (object o, ToggledArgs args)
		{
			var cell = (CellRendererToggle)o;
			string path = args.Path;
			//CellEditedFunction (cell, args.Path, cell.Active);

			CellEditedToggled (cell, path);
		}

		/// <summary>
		/// Handles when the cell is edited.
		/// </summary>
		/// <param name="cell">Cell.</param>
		/// <param name="path">Path.</param>
		/// <param name="newValue">New value.</param>
		void CellEditedFunction (CellRenderer cell, string path, object newValue)
		{
			TreeIter iter;
			if (_treeStore.GetIterFromString (out iter, path)) {

				System.Reflection.PropertyInfo prop1;
				if (_columnProperties.TryGetValue (cell.Handle.ToInt32 (), out prop1)) {
					var item = (T)_treeStore.GetValue (iter, 0);

					var strVal = newValue as string;
					if (strVal != null) {
						// This is a string value, so check for a parser
						Func<string, object> parser;
						if (ColumnParsers.TryGetValue (prop1.Name, out parser)) {
							newValue = parser (strVal);
						}
					}

					prop1.SetValue (item, newValue, null);
				}
			}
		}

		/// <summary>
		/// Handles when the cell is edited by a toggle button.
		/// </summary>
		/// <param name="cell">Cell.</param>
		/// <param name="path">Path.</param>
		void CellEditedToggled (CellRendererToggle cell, string path)
		{
			TreeIter iter;
			if (_treeStore.GetIterFromString (out iter, path)) {

				System.Reflection.PropertyInfo prop1;
				if (_columnProperties.TryGetValue (cell.Handle.ToInt32 (), out prop1)) {
					var item = (T)_treeStore.GetValue (iter, 0);

					prop1.SetValue (item, !(bool)prop1.GetValue (item, null), null);
				}
			}
		}

		/// <summary>
		/// Gets a new item.
		/// </summary>
		/// <returns>The new.</returns>
		T GetNew ()
		{
			return _creator == null ? new T () : _creator.CreateNew ();
		}

		/// <summary>
		/// Selects the item in the TreeView
		/// </summary>
		/// <param name="iter">Iter.</param>
		void TreeSelectItem (TreeIter iter)
		{
			_tree.Selection.SelectIter (iter);
			_tree.ActivateRow (_treeStore.GetPath (iter), _tree.Columns [0]);
			_tree.HasFocus = true;
		}

		void BtnAdd_Clicked (object sender, EventArgs e)
		{
			TreeIter iter = _treeStore.AppendValues (GetNew ());

			TreeSelectItem (iter);
		}

		void BtnInsert_Clicked (object sender, EventArgs e)
		{
			TreeIter selected;
			if (_tree.Selection.GetSelected (out selected)) {
				TreeIter newItem = _treeStore.InsertBefore (selected);
				_treeStore.SetValue (newItem, 0, GetNew ());

				TreeSelectItem (newItem);
			}
		}

		void BtnDelete_Clicked (object sender, EventArgs e)
		{
			TreeIter iter;
			if (_tree.Selection.GetSelected (out iter)) {
				if (_treeStore.Remove (ref iter)) {
					_tree.HasFocus = true;
				}
			}
		}

		void BtnSave_Clicked (object sender, EventArgs e)
		{
			Save ();

			this.Destroy ();
		}

		void BtnClose_Clicked (object sender, EventArgs e)
		{
			Saved = false;
			this.Destroy ();
		}
	}
}

