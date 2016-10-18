using System;
using Gtk;

namespace GtkSharp.Extensions
{
	public delegate bool ComboValueSelector (Gtk.TreeModel comboModel, Gtk.TreeIter comboIter, string treePathString);

	public delegate void ComboEditedEventHandler(object sender, ComboEditedEventArgs args);

	// Just experimenting with the CellRendererCombo and the ComboBox widgets
	public class CellRendererCombo2 : CellRendererCombo
	{
		CellRenderer _comboCell;

		ComboValueSelector _selector;

		public new System.Collections.Generic.List<CellAttribute> Attributes { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="GtkSharp.Extensions.CellRendererCombo2"/> class.
		/// </summary>
		/// <param name="comboCell">ComboBox cell renderer.</param>
		/// <param name="selector">The selector function for a ComboBox cell.</param>
		public CellRendererCombo2 (CellRenderer comboCell, ComboValueSelector selector)
			: base ()
		{
			if (comboCell == null)
				throw new ArgumentNullException ("comboCell");
			if (selector == null)
				throw new ArgumentNullException ("selector");
			
			_comboCell = comboCell;
			_selector = selector;

			this.EditingStarted += CellRendererCombo2_EditingStarted;
			this.Edited += CellRendererCombo2_Edited;
		}

		// Stored pointer to the current Editable object
		CellEditable _editable = null;

		string _editingPath = null;

		ComboBox _editableComboBox = null;

		void CellRendererCombo2_EditingStarted (object o, EditingStartedArgs args)
		{
			
		}

		void CellRendererCombo2_Edited (object o, EditedArgs args)
		{

		}

		void OnEditingDone (ComboBox widget)
		{
			_editable = null;
			_editingPath = null;
		}



		protected override void OnEdited (string path, string new_text)
		{
			base.OnEdited (path, new_text);
		}

		protected override void OnEditingStarted (CellEditable editable, string path)
		{
			_editable = editable;
			_editingPath = path;

			_editableComboBox = _editable as ComboBox;

			var combo = _editableComboBox;
			if (combo != null) {
				if (combo.Cells != null && combo.Cells.Length > 0) {
					// Cells already exist
				} else {
					// No cells - add the cell, and assign the Attribute
					var cellShow = new CellRendererText ();
					combo.PackStart (_comboCell, true);
					foreach (var attr in this.Attributes) {
						combo.AddAttribute (_comboCell, attr.Attribute, attr.Column);
					}
				}

				// Set the Selected
				combo.SetSelectedItem ((TreeModel model, TreeIter iter) => _selector (model, iter, _editingPath));
			}

			editable.WidgetRemoved += delegate(object sender, EventArgs e) {
				
			};

			editable.EditingDone += delegate (object sender, EventArgs e) {
				OnEditingDone (sender as ComboBox);
			};

			base.OnEditingStarted (editable, path);
		}

		public override CellEditable StartEditing (Gdk.Event evnt, Widget widget, string path, Gdk.Rectangle background_area, Gdk.Rectangle cell_area, CellRendererState flags)
		{
			return base.StartEditing (evnt, widget, path, background_area, cell_area, flags);
		}
	}

	public struct CellAttribute
	{
		/// <summary>
		/// Gets or sets the CellRenderer attribute (ex: "sensitive" or "text).
		/// </summary>
		/// <value>The CellRenderer attribute.</value>
		public string Attribute { get; set; }

		/// <summary>
		/// Gets or sets the Model column index.
		/// </summary>
		/// <value>The Model column index.</value>
		public int Column { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="GtkSharp.Extensions.CellAttribute"/> struct.
		/// </summary>
		/// <param name="attribute">The CellRenderer attribute (ex: "sensitive" or "text).</param>
		/// <param name="column">The Model column index.</param>
		public CellAttribute (string attribute, int column)
			: this ()
		{
			this.Attribute = attribute;
			this.Column = column;
		}
	}

	public class ComboEditedEventArgs : EventArgs
	{
		
	}
}

