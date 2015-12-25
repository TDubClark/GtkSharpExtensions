using System;
using System.Collections.Generic;
using System.Linq;
using Gtk;

namespace GtkSharp.Extensions
{
	public interface ITreeControllerCollectionEditor
	{
		/// <summary>
		/// Saves the items to the collection.
		/// </summary>
		void Save ();

		/// <summary>
		/// Builds the TreeView columns, and appends them to the given widget.
		/// </summary>
		/// <param name="widget">The TreeView widget.</param>
		/// <param name="uneditableColumnCount">The count of uneditable columns</param>
		void BuildTreeColumns (TreeView widget, out int uneditableColumnCount);

		/// <summary>
		/// Gets or sets a pointer to the TreeView widget.
		/// </summary>
		/// <value>The tree widget.</value>
		TreeView TreeWidget { get; set; }

		/// <summary>
		/// Gets or sets the TreeView store.
		/// </summary>
		/// <value>The tree store.</value>
		ListStore TreeStore { get; set; }

		/// <summary>
		/// Inserts a new item into the list store, before the selected item
		/// </summary>
		/// <param name="widget">Widget.</param>
		/// <param name="selected">Selected.</param>
		void Insert (TreeView widget, TreeIter selected);

		/// <summary>
		/// Appends a new item to the list store
		/// </summary>
		/// <param name="widget">Widget.</param>
		void Append (TreeView widget);
	}

	/// <summary>
	/// Definition for a TreeView Combo cell.
	/// </summary>
	public class TreeComboCellDefinition
	{
		/// <summary>
		/// Gets or sets the values which will appear in the combobox.
		/// </summary>
		/// <value>The combo values.</value>
		public System.Collections.IEnumerable ComboValues { get; set; }

		/// <summary>
		/// Gets or sets the equality comparer for items in the combobox.
		/// </summary>
		/// <value>The combo value comparer.</value>
		public System.Collections.IEqualityComparer ComboValueComparer { get; set; }

		/// <summary>
		/// Gets or sets whether custom values are allowed.
		/// </summary>
		/// <value><c>true</c> if custom value allowed; otherwise, <c>false</c>.</value>
		public bool CustomValueAllowed { get; set; }

		public TreeComboCellDefinition ()
		{
		}

		public TreeComboCellDefinition (System.Collections.IEnumerable comboValues, System.Collections.IEqualityComparer comboValueComparer, bool customValueAllowed)
		{
			this.ComboValues = comboValues;
			this.ComboValueComparer = comboValueComparer;
			this.CustomValueAllowed = customValueAllowed;
		}
	}

	/// <summary>
	/// Collection Editor controller for a Gtk# TreeView.
	/// </summary>
	public class TreeControllerCollectionEditor<T> : ITreeControllerCollectionEditor where T : new()
	{
		/// <summary>
		/// Gets or sets the value collection.
		/// </summary>
		/// <value>The value collection.</value>
		public IList<T> ValueCollection { get; set; }

		/// <summary>
		/// Gets or sets the values added to the collection.
		/// </summary>
		/// <value>The values added.</value>
		public List<T> ValuesAdded { get; set; }

		/// <summary>
		/// Gets or sets the values removed from the collection.
		/// </summary>
		/// <value>The values removed.</value>
		public List<T> ValuesRemoved { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether changes were saved.
		/// </summary>
		/// <value><c>true</c> if saved; otherwise, <c>false</c>.</value>
		public bool Saved { get; set; }

		/// <summary>
		/// Gets or sets the object creator.
		/// </summary>
		/// <value>The creator.</value>
		public IObjectCreator<T> Creator { get; set; }

		/// <summary>
		/// Gets or sets the column-property dictionary.
		/// </summary>
		/// <value>The column properties.</value>
		public Dictionary<int, System.Reflection.PropertyInfo> ColumnProperties { get; set; }

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

		/// <summary>
		/// Gets or sets the selectable column values.
		/// </summary>
		/// <value>The column values.</value>
		public Dictionary<string, object[]> ColumnValues { get; set; }

		public Dictionary<string, TreeComboCellDefinition> ColumnComboDef { get; set; }

		public List<string> ColumnCustomValueAllowed { get; set; }

		/// <summary>
		/// Gets or sets the column headers for each propertyName (key=PropName, value=ColumnHeader).
		/// </summary>
		/// <value>The column headers.</value>
		public Dictionary<string, string> ColumnHeaders { get; set; }

		/// <summary>
		/// Gets or sets the editable property names.
		/// </summary>
		/// <value>The editable property names.</value>
		public List<string> EditablePropertyNames { get; set; }

		/// <summary>
		/// Gets or sets the property name comparer.
		/// </summary>
		/// <value>The property name comparer.</value>
		public IEqualityComparer<string> PropertyNameComparer { get; set; }

		/// <summary>
		/// Adds the column definition.
		/// </summary>
		/// <param name="propertyName">Property name.</param>
		/// <param name="columnHeader">Column header.</param>
		/// <param name="editable">Whether this property should be editable</param>
		public void AddColumnDef (string propertyName, string columnHeader, bool editable)
		{
			ColumnHeaders.Add (propertyName, columnHeader);
			if (editable) {
				this.EditablePropertyNames.Add (propertyName);
			}
		}

		/// <summary>
		/// Adds the column definition.
		/// </summary>
		/// <param name="propertyName">Property name.</param>
		/// <param name="columnHeader">Column header.</param>
		/// <param name="displayer">Displayer.</param>
		/// <param name="parser">Parser.</param>
		public void AddColumnDef (string propertyName, string columnHeader, Func<object, string> displayer, Func<string, object> parser)
		{
			ColumnParsers.Add (propertyName, parser);
			ColumnDisplayers.Add (propertyName, displayer);
			ColumnHeaders.Add (propertyName, columnHeader);
		}

		public void AddColumnDef (string propertyName, string columnHeader, Func<object, string> displayer, Func<string, object> parser, System.Collections.IEnumerable selectionItems, System.Collections.IEqualityComparer selectionItemComparer, bool allowCustom)
		{
			AddColumnDef (propertyName, columnHeader, displayer, parser);

			ColumnComboDef.Add (propertyName, new TreeComboCellDefinition (selectionItems, selectionItemComparer, allowCustom));
		}

		public void AddColumnDef<TRef> (string propertyName, string columnHeader
			, Func<TRef, string> displayer
			, System.Collections.IEnumerable selectionItems
			, System.Collections.IEqualityComparer selectionItemComparer
			, bool allowCustom) where TRef : class, new()
		{
			AddColumnDef<TRef> (propertyName, columnHeader, displayer, x => new TRef (), selectionItems, selectionItemComparer, allowCustom);
		}

		public void AddColumnDef<TRef> (string propertyName, string columnHeader
			, Func<TRef, string> displayer, Func<string, TRef> parser
			, System.Collections.IEnumerable selectionItems
			, System.Collections.IEqualityComparer selectionItemComparer
			, bool allowCustom) where TRef : class
		{
			AddColumnDef (propertyName, columnHeader, x => displayer (x as TRef), x => parser (x));

			ColumnComboDef.Add (propertyName, new TreeComboCellDefinition (selectionItems, selectionItemComparer, allowCustom));
		}

		/// <summary>
		/// Gets or sets the TreeView widget.
		/// </summary>
		/// <value>The tree widget.</value>
		public TreeView TreeWidget { get; set; }

		ListStore _treeStore;

		/// <summary>
		/// Gets or sets the TreeView store.
		/// </summary>
		/// <value>The tree store.</value>
		public ListStore TreeStore {
			get { return _treeStore; }
			set { _treeStore = value; }
		}


		/// <summary>
		/// Sets the new collection of values to be edited.
		/// </summary>
		/// <param name="collection">Collection.</param>
		public void SetNewCollection (IList<T> collection)
		{
			this.ValueCollection = collection;
			this.ValuesAdded.Clear ();
			this.ValuesRemoved.Clear ();
			
			_treeStore.Clear ();
			foreach (var item in collection) {
				_treeStore.AppendValues (item);
			}
		}

		/// <summary>
		/// Saves the items to the collection.
		/// </summary>
		public void Save ()
		{
			ValueCollection.Clear ();
			foreach (var iter in _treeStore.GetParentItems ()) {
				ValueCollection.Add ((T)_treeStore.GetValue (iter, 0));
			}
			Saved = true;
		}

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
			return ColumnParsers.ContainsKey (propName);
		}

		bool IsSelectionProp (string propName, out System.Collections.IEnumerable columnValueItems, out bool allowCustom)
		{
			TreeComboCellDefinition def;
			if (ColumnComboDef.TryGetValue (propName, out def)) {
				columnValueItems = def.ComboValues;
				allowCustom = def.CustomValueAllowed;
				return true;
			}

			columnValueItems = null;
			allowCustom = false;
			return false;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="libGtkSharpTreeView.TreeControllerCollectionEditor`1"/> class.
		/// </summary>
		/// <param name="propertyNameComparer">Property name comparer.</param>
		protected TreeControllerCollectionEditor (IEqualityComparer<string> propertyNameComparer)
		{
			this.PropertyNameComparer = propertyNameComparer;
			this.ColumnParsers = new Dictionary<string, Func<string, object>> (propertyNameComparer);
			this.ColumnDisplayers = new Dictionary<string, Func<object, string>> (propertyNameComparer);
//			this.ColumnValues = new Dictionary<string, object[]> ();
			this.ColumnHeaders = new Dictionary<string, string> ();
//			this.ColumnCustomValueAllowed = new List<string> ();
			this.ColumnComboDef = new Dictionary<string, TreeComboCellDefinition> ();
			this.EditablePropertyNames = new List<string> ();

			this.ValuesAdded = new List<T> ();
			this.ValuesRemoved = new List<T> ();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="libGtkSharpTreeView.TreeControllerCollectionEditor`1"/> class.
		/// </summary>
		/// <param name="propertyNameComparer">Property name comparer.</param>
		/// <param name="collection">Collection.</param>
		public TreeControllerCollectionEditor (IEqualityComparer<string> propertyNameComparer, IList<T> collection)
			: this (propertyNameComparer, collection, null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="libGtkSharpTreeView.TreeControllerCollectionEditor`1"/> class.
		/// </summary>
		/// <param name="propertyNameComparer">Property name comparer.</param>
		/// <param name="collection">Collection.</param>
		/// <param name="creator">Creator.</param>
		public TreeControllerCollectionEditor (IEqualityComparer<string> propertyNameComparer, IList<T> collection, IObjectCreator<T> creator)
			: this (propertyNameComparer)
		{
			this.ValueCollection = collection;
			this.Creator = creator;

			_treeStore = new ListStore (typeof(T));
			foreach (var item in collection) {
				_treeStore.AppendValues (item);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="libGtkSharpTreeView.TreeControllerCollectionEditor`1"/> class.
		/// </summary>
		/// <param name="propertyNameComparer">Property name comparer.</param>
		/// <param name="collection">Collection.</param>
		/// <param name="creator">Creator.</param>
		/// <param name="columnParsers">Column parser functions.</param>
		/// <param name="columnDisplayers">Column Displayer functions.</param>
		public TreeControllerCollectionEditor (IEqualityComparer<string> propertyNameComparer
			, IList<T> collection
			, IObjectCreator<T> creator
			, Dictionary<string, Func<string, object>> columnParsers
			, Dictionary<string, Func<object, string>> columnDisplayers)
			: this (propertyNameComparer, collection, creator, columnParsers, columnDisplayers, new string[] { })
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="libGtkSharpTreeView.TreeControllerCollectionEditor`1"/> class.
		/// </summary>
		/// <param name="propertyNameComparer">Property name comparer.</param>
		/// <param name="collection">Collection.</param>
		/// <param name="creator">Creator.</param>
		/// <param name="columnParsers">Column parsers.</param>
		/// <param name="columnDisplayers">Column displayers.</param>
		/// <param name="editablePropNames">Editable property names.</param>
		public TreeControllerCollectionEditor (IEqualityComparer<string> propertyNameComparer
			, IList<T> collection
			, IObjectCreator<T> creator
			, Dictionary<string, Func<string, object>> columnParsers
			, Dictionary<string, Func<object, string>> columnDisplayers
			, string[] editablePropNames)
			: this (propertyNameComparer)
		{
			if (columnParsers == null)
				throw new ArgumentNullException ("columnParsers");
			if (columnDisplayers == null)
				throw new ArgumentNullException ("columnDisplayers");

			this.ValueCollection = collection;
			this.Creator = creator;
			this.ColumnParsers = columnParsers;
			this.ColumnDisplayers = columnDisplayers;

			this.EditablePropertyNames.AddRange (editablePropNames);

			_treeStore = new ListStore (typeof(T));
			foreach (var item in collection) {
				_treeStore.AppendValues (item);
			}
			//_tree.Model = _treeStore;
		}

		/// <summary>
		/// Builds the TreeView columns.
		/// </summary>
		public void BuildTreeColumns (TreeView widget, out int uneditableColumnCount)
		{
			uneditableColumnCount = 0;

			ColumnProperties = new Dictionary<int, System.Reflection.PropertyInfo> ();

			CellRenderer treeCell = null;
			TreeCellDataFunc func = null;

			var props = typeof(T).GetProperties ();
			foreach (var prop in props) {

				System.Collections.IEnumerable columnValues;
				bool allowCustom;
				if (IsSelectionProp (prop.Name, out columnValues, out allowCustom)) {
					var store = new ListStore (typeof(object), typeof(string));

					Func<object, string> displayer;
					if (!ColumnDisplayers.TryGetValue (prop.Name, out displayer)) {
						displayer = null;
					}
					foreach (var val in columnValues) {
						store.AppendValues (val, GetColumnDisplay (val, displayer));
					}

					var comboCell = new CellRendererCombo ();
					comboCell.Editable = true;
					comboCell.HasEntry = allowCustom;
					comboCell.Model = store;
					comboCell.TextColumn = 1;
					comboCell.Edited += ComboCell_Edited;
					comboCell.EditingStarted += ComboCell_EditingStarted;

					treeCell = comboCell;

					func = new TreeCellDataFunc (TextCellFunction);
				} else {
					if (prop.PropertyType.Equals (typeof(int))) {
						var spinCell = new CellRendererSpin ();
						if (IsEditableProp (prop.Name)) {
							spinCell.Editable = true;
							spinCell.Mode = CellRendererMode.Editable;
						}
						spinCell.ClimbRate = 1;
						spinCell.Digits = 0;
						spinCell.Adjustment = new Adjustment (0.0, Int32.MinValue, Int32.MaxValue, 1.0, 1.0, 1.0);
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
				}

				ColumnProperties.Add (treeCell.Handle.ToInt32 (), prop);

				treeCell.Sensitive = true;

				// Set background-color to light gray
				const int clrGray = (15 * 16) + 3;
				treeCell.CellBackgroundGdk = new Gdk.Color (clrGray, clrGray, clrGray);

				var treeCol = new TreeViewColumn ();
				treeCol.Reorderable = true;
				treeCol.Resizable = true;
				treeCol.SortIndicator = true;

				treeCol.Title = GetColumnHeader (prop.Name);
				treeCol.PackStart (treeCell, true);
				treeCol.SetCellDataFunc (treeCell, func);

				widget.AppendColumn (treeCol);
			}
		}

		/// <summary>
		/// Handles the Edited event of the ComboBox Cell
		/// </summary>
		/// <param name="o">The object.</param>
		/// <param name="args">The Edited Arguments.</param>
		void ComboCell_Edited (object o, EditedArgs args)
		{
//			var cell = o as CellRendererCombo;
//			var path = args.Path;
//
//			TreeIter iter;
//			if (_treeStore.GetIterFromString (out iter, path)) {
//
//				System.Reflection.PropertyInfo prop1;
//				if (ColumnProperties.TryGetValue (cell.Handle.ToInt32 (), out prop1)) {
//					var item = (T)_treeStore.GetValue (iter, 0);
//
//					var strVal = newValue as string;
//					if (strVal != null) {
//						// This is a string value, so check for a parser
//						Func<string, object> parser;
//						if (ColumnParsers.TryGetValue (prop1.Name, out parser)) {
//							newValue = parser (strVal);
//						}
//					}
//
//					prop1.SetValue (item, newValue, null);
//				}
//			}
		}

		Dictionary<int, System.Reflection.PropertyInfo> _comboboxProp;

		TreeIter _currentComboEditingIter = TreeIter.Zero;

		/// <summary>
		/// Handles the EditingStarted event of the ComboBox Cell
		/// </summary>
		/// <param name="o">The object.</param>
		/// <param name="args">The EditingStarted Arguments.</param>
		void ComboCell_EditingStarted (object o, EditingStartedArgs args)
		{
			// When the Editing starts, you need to remember information about the combobox widget
			// the widget seems to be created on-the-fly
			var cell = o as CellRenderer;
			var combo = args.Editable as ComboBox;
			if (combo != null) {
				// Do this: http://stackoverflow.com/questions/9983469/gtk3-combobox-shows-parent-items-from-a-treestore
				// How to use the EditingStarted event handler:
				//    https://developer.gnome.org/gtk3/stable/GtkCellRenderer.html#GtkCellRenderer-editing-started
				// Setting the combobox cell renderer column:
				//    http://www.pygtk.org/pygtk2tutorial/sec-ComboBoxAndComboboxEntry.html
				// Other reference pages:
				//    http://lazka.github.io/pgi-docs/Gtk-3.0/classes/CellRendererCombo.html#Gtk.CellRendererCombo
				//    https://developer.gnome.org/gtk3/stable/GtkCellRendererCombo.html

				if (combo.Cells == null || combo.Cells.Length == 0) {
					// Pack a cell into the combobox
					combo.PackStart (new CellRendererText (), true);
				}

				// Always set attribute(s)
				// This says "populate the 'Text' attribute (property) of the given cell from the model value in column index # 1"
				combo.AddAttribute (combo.Cells [0], "text", 1);

				// Get the Property for this cell
				System.Reflection.PropertyInfo prop;
				if (this.ColumnProperties.TryGetValue (cell.Handle.ToInt32 (), out prop)) {
					
					// Assign the handler for the ComboBox widget, and remember the property
					combo.Changed += Combo_Changed;
					if (_comboboxProp == null)
						_comboboxProp = new Dictionary<int, System.Reflection.PropertyInfo> ();
					_comboboxProp.Add (combo.Handle.ToInt32 (), prop);

					// Set the selected value of the combobox
					TreeIter iterDataModel;
					if (_treeStore.GetIterFromString (out iterDataModel, args.Path)) {
						// This is the object of the current row
						object storeObject = _treeStore.GetValue (iterDataModel, 0);

						// Remember the current iter
						_currentComboEditingIter = iterDataModel;


						// Get the property assigned to this cell
						// Get the cell value for the current row
						object cellValue = prop.GetValue (storeObject, null);

						// Get the Combo Definition
						TreeComboCellDefinition def;
						if (this.ColumnComboDef.TryGetValue (prop.Name, out def)) {

							// Assign the selected combo cell
							var comparer = def.ComboValueComparer;
							combo.SetSelectedItem ((TreeModel argModel, TreeIter argIter) => comparer.Equals (cellValue, argModel.GetValue (argIter, 0)));
						}
					}
				}
			}
		}

		/// <summary>
		/// Handles the change for a ComboBox
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		void Combo_Changed (object sender, EventArgs e)
		{
			var combo = sender as ComboBox;

			// Get the selected item from the ComboBox
			TreeIter iter;
			if (combo.GetActiveIter (out iter)) {

				// Get the property for this combobox
				System.Reflection.PropertyInfo prop;
				if (_comboboxProp.TryGetValue (combo.Handle.ToInt32 (), out prop)) {


					if (!_currentComboEditingIter.Equals (TreeIter.Zero)) {
						prop.SetValue (_treeStore.GetValue (_currentComboEditingIter, 0), combo.Model.GetValue (iter, 0), null);
					}
				}
			}

			// Reset the remembered values
			_comboboxProp.Clear ();
			_currentComboEditingIter = TreeIter.Zero;
		}

		/// <summary>
		/// Gets the column header for a given property name.
		/// </summary>
		/// <returns>The column header.</returns>
		/// <param name="propertyName">Property name.</param>
		protected string GetColumnHeader (string propertyName)
		{
			string colHeader;
			if (this.ColumnHeaders.TryGetValue (propertyName, out colHeader))
				return colHeader;
			
			return propertyName;
		}

		/// <summary>
		/// Gets the display text for the given value, in the given column.
		/// </summary>
		/// <returns>The column display.</returns>
		/// <param name="value">Value.</param>
		/// <param name="columnName">Column name.</param>
		protected string GetColumnDisplay (object value, string columnName)
		{
			if (value == null)
				return "";

			Func<object, string> displayer;
			if (ColumnDisplayers.TryGetValue (columnName, out displayer)) {
				return displayer (value);
			}
			return value.ToString ();
		}

		protected string GetColumnDisplay (object value, Func<object, string> displayer)
		{
			if (value == null)
				return "";

			if (displayer != null) {
				return displayer (value);
			}
			return value.ToString ();
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
			// A Spin Cell functions as a text cell on display
			TextCellFunction (tree_column, cell, tree_model, iter);
			//CellDataFunction (cell, tree_model, iter, 0, (CellRenderer oCell, object value) => ((CellRendererSpin)oCell).Adjustment.Value = (int)value);
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
			var defaultValue = "";
			CellDataFunction (cell, tree_model, iter, defaultValue, (CellRenderer oCell, object value, string columnName) => ((CellRendererText)oCell).Text = GetColumnDisplay (value, columnName));
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
			if (ColumnProperties.TryGetValue (cell.Handle.ToInt32 (), out prop)) {
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
			if (ColumnProperties.TryGetValue (cell.Handle.ToInt32 (), out prop)) {
				var value = prop.GetValue ((T)tree_model.GetValue (iter, 0), null);
				if (value != null)
					cellAssign (cell, value, prop.Name);
				else {
					object value2;
					Func<string, object> parser;
					if (defaultValue is string && ColumnParsers.TryGetValue (prop.Name, out parser)) {
						value2 = parser (defaultValue as string);
					} else {
						value2 = defaultValue;
					}

					cellAssign (cell, value2, prop.Name);
				}
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
				if (ColumnProperties.TryGetValue (cell.Handle.ToInt32 (), out prop1)) {
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
				if (ColumnProperties.TryGetValue (cell.Handle.ToInt32 (), out prop1)) {
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
			return Creator == null ? new T () : Creator.CreateNew ();
		}

		/// <summary>
		/// Selects the item in the TreeView
		/// </summary>
		/// <param name="widget">The TreeView widget</param>
		/// <param name="iter">The TreeView Iter.</param>
		public static void TreeSelectItem (TreeView widget, TreeIter iter)
		{
			widget.Selection.SelectIter (iter);
			widget.ActivateRow (widget.Model.GetPath (iter), widget.Columns [0]);
			widget.HasFocus = true;
		}

		public void Insert (TreeView widget, TreeIter selected)
		{
			TreeIter newItem = _treeStore.InsertBefore (selected);

			var newValue = GetNew ();

			_treeStore.SetValue (newItem, 0, newValue);

			this.ValuesAdded.Add (newValue);

			TreeSelectItem (widget, newItem);
		}

		public void Append (TreeView widget)
		{
			var newItem = GetNew ();

			TreeIter iter = _treeStore.AppendValues (newItem);

			this.ValuesAdded.Add (newItem);

			TreeSelectItem (widget, iter);
		}

		/// <summary>
		/// Remove the specified item and selected.
		/// </summary>
		/// <param name="widget">Widget.</param>
		/// <param name="selected">Selected.</param>
		public void Remove (Gtk.TreeView widget, ref Gtk.TreeIter selected)
		{
			var removedItem = (T)_treeStore.GetValue (selected, 0);
			if (_treeStore.Remove (ref selected)) {
				this.ValuesRemoved.Add (removedItem);

				TreeSelectItem (widget, selected);
			}
		}
	}
}

