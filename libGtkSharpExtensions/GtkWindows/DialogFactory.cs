using System;
using System.Collections.Generic;
using System.Linq;
using Gtk;
using GtkSharp.Extensions;

namespace GtkSharp.Win
{
	/// <summary>
	/// This is a factory for Gtk# Dialogs
	/// </summary>
	public static class DialogFactory
	{
		public static void ShowError (Window parent, Exception ex)
		{
			var dlg = new MessageDialog (parent
				, DialogFlags.Modal | DialogFlags.DestroyWithParent
				, MessageType.Error
				, ButtonsType.Close
				, "Error: {0}", ex.Message);

			dlg.Run ();
			dlg.Destroy ();
		}

		public static Dialog GetDialogObject (string title, Window parent, string proceedButtonLabel)
		{
			return GetDialogObject (title, parent, proceedButtonLabel, "Cancel");
		}

		public static Dialog GetDialogObject (string title, Window parent, string proceedButtonLabel, string cancelButtonLabel)
		{
			// Set the button parameters
			object[] buttonParams = null;

			if (String.IsNullOrWhiteSpace (proceedButtonLabel)) {
				// No Accept button - just cancel
				buttonParams = new object[] {
					cancelButtonLabel, ResponseType.Cancel
				};
			} else {
				buttonParams = new object[] {
					proceedButtonLabel, ResponseType.Accept
					, cancelButtonLabel, ResponseType.Cancel
				};
			}

			Dialog dlg = new Dialog (title, parent
				, DialogFlags.Modal | DialogFlags.DestroyWithParent
				, buttonParams
			             );

			return dlg;
		}

		// Source: http://stackoverflow.com/questions/990640/is-there-a-form-showdialog-equivalent-for-gtk-windows
		// Showing a dialog

		public static bool ShowDialogTextEntry (string title, Window parent, string proceedButtonLabel, ref string text)
		{
			// Default value in Gtk.Entry for "no maximum length"
			int maxLen = 0;
			return ShowDialogTextEntry (title, parent, proceedButtonLabel, maxLen, ref text);
		}

		public static bool ShowDialogTextEntry (string title, Window parent, string proceedButtonLabel, int maxLen, ref string text)
		{
			Entry entry = new Entry (maxLen);
			entry.Text = text;

			using (var dlg = new DialogDisposable (GetDialogObject (title, parent, proceedButtonLabel))) {
				dlg.AddWidgetHBox (entry, true, true, 10, 6);

				if (dlg.RunDialog () == ResponseType.Accept) {
					text = entry.Text;
					return true;
				}
			}

			return false;
		}

		public static bool ShowDialogTextEntryMultiLine (string title, Window parent, string proceedButtonLabel, int width, int height, ref string text)
		{
			var dlg = GetDialogObject (title, parent, proceedButtonLabel);
			dlg.SetSizeRequest (width, height);

			TextView entry1 = new TextView ();
			entry1.Editable = true;
			entry1.AcceptsTab = true;
			if (!String.IsNullOrWhiteSpace (text)) {
				entry1.Buffer.Text = text;
			}

			//			HBox hbx = new HBox ();
			//			hbx.PackStart (entry1, true, true, 10);
			//			dlg.VBox.PackStart (hbx, true, true, 6);
			dlg.PackWithHBox (entry1, true, true, 10, 6);

			//			CheckButton check = new CheckButton ("Wrap");
			//			check.Mode = false;
			//			check.Toggled += (object sender, EventArgs e) => {
			//				entry1.WrapMode = check.Mode ? WrapMode.Word : WrapMode.None;
			//			};
			//			check.SetSizeRequest (100, 25);
			//			dlg.VBox.PackEnd (check, true, false, 0);

			bool isAccept = false;
			text = null;

			using (var dgl2 = dlg.WrapDisposable ()) {
				if (dgl2.RunDialog () == ResponseType.Accept) {
					text = entry1.Buffer.Text;
					isAccept = true;
				}
			}

			return isAccept;
		}

		/// <summary>
		/// Shows the dialog selector (returns whether an Item was selected and the "Proceed" button was clicked)
		/// </summary>
		/// <param name="title">Title.</param>
		/// <param name="parent">Parent.</param>
		/// <param name="proceedButtonLabel">Proceed button label.</param>
		/// <param name="itemSelected">Item selected.</param>
		/// <returns>Whether to proceed</returns>
		public static bool ShowDialogSelector (string title, Window parent, string proceedButtonLabel, IEnumerable<string> items, out string itemSelected)
		{
			bool isProceed = false;
			itemSelected = null;

			ComboBox cmbxSelector = new ComboBox (items.ToArray ());
			cmbxSelector.SetActiveFirst ();

			var dlg = GetDialogObject (title, parent, proceedButtonLabel);
			dlg.PackWithHBox (cmbxSelector, false, 6, true, true, 10, 6);

			using (var dlg2 = dlg.WrapDisposable ()) {
				if (dlg2.RunDialog () == ResponseType.Accept) {

					TreeIter iter;
					if (cmbxSelector.GetActiveIter (out iter)) {
						itemSelected = (string)cmbxSelector.Model.GetValue (iter, 0);
						isProceed = true;
					}
				}
			}

			return isProceed;
		}

		/// <summary>
		/// Shows the dialog selector (returns whether an Item was selected and the "Proceed" button was clicked)
		/// </summary>
		/// <param name="title">Title.</param>
		/// <param name="parent">Parent.</param>
		/// <param name="proceedButtonLabel">Proceed button label.</param>
		/// <param name="itemSelected">Item selected.</param>
		/// <returns>Whether to proceed</returns>
		public static bool ShowDialogSelectorListBox (string title, Window parent, int listWidth, int listHeight, string proceedButtonLabel, IEnumerable<string> items, out string itemSelected)
		{
			bool isProceed = false;
			itemSelected = null;

			// Add the itmes to a model for the TreeView
			var model = new Gtk.ListStore (typeof(string));
			foreach (var item in items) {
				model.AppendValues (item);
			}

			// Create TreeView
			var tree = new TreeView (model);

			// Append a column with a Text cell, populate the Text property of the cell from column 0 of the model
			tree.AppendColumn (title, new CellRendererText (), "text", 0);

			// Add the TreeView to a ScrolledWindow
			ScrolledWindow scroller = new ScrolledWindow ();
			scroller.SetSizeRequest (listWidth, listHeight);
			scroller.Add (tree);

			// Create a new Dialog
			var dlg = GetDialogObject (title, parent, proceedButtonLabel);

			// Add the ScrolledWindow to the Dialog's built-in VBox container
			dlg.VBox.Add (scroller);

			// Wrap the dialog in a disposable container (which shows all widgets on RunDialog(), and Destroys the dialog on Dispose)
			using (var dlg2 = dlg.WrapDisposable ()) {
				if (dlg2.RunDialog () == ResponseType.Accept) {

					// Get the selected Tree Iterator, which points to a distict item in the model
					TreeIter iter;
					if (tree.Selection.GetSelected (out iter)) {
						itemSelected = (string)model.GetValue (iter, 0);
						isProceed = true;
					}
				}
			}

			return isProceed;
		}

		public static bool ShowDialogSelectorListBox (string title, Window parent, int listWidth, int listHeight, string proceedButtonLabel, TreeModel model, out TreeIter iterSelected, params TreeViewColumn[] columns)
		{
			var tree = new TreeView (model);
			foreach (var col in columns) {
				tree.AppendColumn (col);
			}
			return ShowDialogSelectorListBox (title, parent, listWidth, listHeight, proceedButtonLabel, tree, out iterSelected);
		}

		public static bool ShowDialogSelectorListBox (string title, Window parent, int listWidth, int listHeight, string proceedButtonLabel, TreeView tree, out TreeIter iterSelected)
		{
			bool isProceed = false;
			iterSelected = TreeIter.Zero;

			ScrolledWindow scroller = new ScrolledWindow ();
			scroller.SetSizeRequest (listWidth, listHeight);
			scroller.Add (tree);

			var dlg = GetDialogObject (title, parent, proceedButtonLabel);
			dlg.VBox.Add (scroller);

			using (var dlg2 = dlg.WrapDisposable ()) {
				if (dlg2.RunDialog () == ResponseType.Accept) {
					isProceed = tree.Selection.GetSelected (out iterSelected);
				}
			}

			return isProceed;
		}

		class ComboBoxListItem
		{
			public object Value { get; set; }

			public string DisplayText { get; set; }

			public ComboBoxListItem ()
			{
			}

			public ComboBoxListItem (object value, string displayText)
			{
				this.Value = value;
				this.DisplayText = displayText;
			}
		}

		/// <summary>
		/// Sets the Text for a CellRendererText cell; for use with ComboBox.
		/// </summary>
		/// <param name="cell_layout">Cell layout.</param>
		/// <param name="cell">Cell.</param>
		/// <param name="tree_model">Tree model.</param>
		/// <param name="iter">Iter.</param>
		static void ComboBoxTextCellDisplay (CellLayout cell_layout, CellRenderer cell, TreeModel tree_model, TreeIter iter)
		{
			((CellRendererText)cell).Text = ((ComboBoxListItem)tree_model.GetValue (iter, 0)).DisplayText;
		}

		/// <summary>
		/// Shows the dialog selector combobox.
		/// </summary>
		/// <returns><c>true</c>, if dialog selector combobox was shown, <c>false</c> otherwise.</returns>
		/// <param name="title">Title.</param>
		/// <param name="parent">Parent.</param>
		/// <param name="proceedButtonLabel">Proceed button label.</param>
		/// <param name="items">Items.</param>
		/// <param name="getDisplayText">Gets the display text for an object from the given list of items.</param>
		/// <param name="itemSelected">Item selected.</param>
		public static bool ShowDialogSelectorCombobox (string title, Window parent, string proceedButtonLabel, System.Collections.IEnumerable items, Func<object, string> getDisplayText, out object itemSelected)
		{
			bool isProceed = false;
			itemSelected = null;

			var store = new ListStore (typeof(ComboBoxListItem));
			foreach (var item in items) {
				store.AppendValues (new ComboBoxListItem (item, getDisplayText (item)));
			}

			ComboBox cmbxSelector = new ComboBox (store);

			var textCell = new CellRendererText ();
			cmbxSelector.PackStart (textCell, true);
			cmbxSelector.SetCellDataFunc (textCell, new CellLayoutDataFunc (ComboBoxTextCellDisplay));

			cmbxSelector.SetActiveFirst ();

			var dlg = GetDialogObject (title, parent, proceedButtonLabel);
			dlg.PackWithHBox (cmbxSelector, false, 6, true, true, 10, 6);

			using (var dlg2 = dlg.WrapDisposable ()) {
				if (dlg2.RunDialog () == ResponseType.Accept) {

					TreeIter iter;
					if (cmbxSelector.GetActiveIter (out iter)) {
						itemSelected = ((ComboBoxListItem)cmbxSelector.Model.GetValue (iter, 0)).Value;
						isProceed = true;
					}
				}
			}

			return isProceed;
		}
	}
}

