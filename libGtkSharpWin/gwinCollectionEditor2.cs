using System;
using Gtk;

using GtkSharp.Extensions;

namespace GtkSharp.Win
{
	public class gwinCollectionEditor2 : Dialog
	{
		/// <summary>
		/// Gets or sets the controller for the TreeView widget.
		/// </summary>
		/// <value>The controller.</value>
		public ITreeControllerCollectionEditor Controller { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to allow add/insert/delete of collection items.
		/// </summary>
		/// <value><c>true</c> if allow insert delete; otherwise, <c>false</c>.</value>
		public bool AllowInsertDelete { get; set; }

		protected gwinCollectionEditor2 (string title, Window parent, DialogFlags flags, params object[] button_data)
			: base (title, parent, flags, button_data)
		{
		}

		/// <summary>
		/// Constructor; note: the LoadTree method must be called after the constructor
		/// </summary>
		/// <param name="title">Title.</param>
		/// <param name="parent">Parent.</param>
		/// <param name="controller">Controller.</param>
		/// <param name="allowInsertDelete">If set to <c>true</c> allow insert delete.</param>
		public gwinCollectionEditor2 (string title, Window parent, ITreeControllerCollectionEditor controller, bool allowInsertDelete)
			: this (title, parent, DialogFlags.Modal, "Save", ResponseType.Accept, "Close", ResponseType.Cancel)
		{
			if (controller == null)
				throw new ArgumentNullException ("controller");
			this.Controller = controller;
			this.AllowInsertDelete = allowInsertDelete;

			Build (600, 480, controller);
		}

		// The TreeView widget
		TreeView _tree;

		/// <summary>
		/// Builds this dialog window, with the specified width, height and controller.
		/// </summary>
		/// <param name="width">Width.</param>
		/// <param name="height">Height.</param>
		/// <param name="controller">Controller.</param>
		void Build (int width, int height, ITreeControllerCollectionEditor controller)
		{
			this.SetSizeRequest (width, height);
			this.Modal = true;

			_tree = new TreeView ();
			controller.TreeWidget = _tree;


			HButtonBox buttons = new HButtonBox ();
			buttons.Spacing = 6;
			buttons.Layout = ButtonBoxStyle.Start;

			if (AllowInsertDelete) { //uneditableColumnCount == 0
				buttons.PackStartButton ("Add", BtnAdd_Clicked, false, false, 0);
				buttons.PackStartButton ("Insert", BtnInsert_Clicked, false, false, 0);
				buttons.PackStartButton ("Delete", BtnDelete_Clicked, false, false, 0);
			}

			this.VBox.PackStart (new ScrolledWindow ().Wrap (_tree), true, true, 0);
			this.VBox.PackEnd (buttons.NestInHBox (0, false, false, 6), false, false, 6);

			this.Response += GcwinCollectionEditor_Response;

			if (this.Child != null) {
				this.Child.ShowAll ();
			}
		}

		/// <summary>
		/// Loads the TreeView model and columns.
		/// </summary>
		public void LoadTreeView ()
		{
			_tree.Model = this.Controller.TreeStore;

			int uneditableColumnCount;
			this.Controller.BuildTreeColumns (_tree, out uneditableColumnCount);
		}

		void GcwinCollectionEditor_Response (object o, ResponseArgs args)
		{
			if (args.ResponseId == ResponseType.Accept) {
				this.Controller.Save ();
			}
			this.Destroy ();
		}

		void BtnAdd_Clicked (object sender, EventArgs e)
		{
			this.Controller.Append (_tree);
		}

		void BtnInsert_Clicked (object sender, EventArgs e)
		{
			TreeIter selected;
			if (_tree.Selection.GetSelected (out selected)) {
				this.Controller.Insert (_tree, selected);
			}
		}

		void BtnDelete_Clicked (object sender, EventArgs e)
		{
			TreeIter iter;
			if (_tree.Selection.GetSelected (out iter)) {
				if (Controller.TreeStore.Remove (ref iter)) {
					_tree.HasFocus = true;
				}
			}
		}

		void BtnSave_Clicked (object sender, EventArgs e)
		{
			this.Controller.Save ();

			this.Destroy ();
		}

		void BtnClose_Clicked (object sender, EventArgs e)
		{
//			this.Controller.Saved = false;
			this.Destroy ();
		}
	}
}

