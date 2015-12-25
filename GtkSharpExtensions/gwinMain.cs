using System;
using Gtk;
using GtkSharp.Extensions;

namespace GtkSharpExtensions
{
	using GtkSharpExtensions.Automotive;

	/// <summary>
	/// Main Gtk# Window
	/// </summary>
	public class gwinMain : Window
	{
		DataBlock _data;

		public gwinMain ()
			: base (WindowType.Toplevel)
		{
			this.Title = "Gtk# Extensions Driver Application";

			Build ();

			// Create demonstration data
			_data = new DataBlock ();
			_data.FillWithData ();
		}

		void Build ()
		{
			var menu = new MenuBar ();

			var mnuTest = new Menu ();
			mnuTest.AppendNewMenuItem ("Edit Manufacturers").Activated += EditMake_Activated;
			mnuTest.AppendNewMenuItem ("Edit Models").Activated += EditModel_Activated;
			mnuTest.AppendNewMenuItem ("Edit Cars").Activated += EditCars_Activated;

			menu.AppendNewMenu ("Test", mnuTest);


			var vbx = new VBox (false, 0);
			vbx.PackStart (menu, false, true, 0);
			vbx.PackStart (new Fixed (), true, true, 0);

			this.Add (vbx);

			this.WindowPosition = WindowPosition.Center;
			this.SetSizeRequest (500, 400);
			this.Destroyed += GwinMain_Destroyed;

			if (this.Child != null)
				this.Child.ShowAll ();
		}

		void GwinMain_Destroyed (object sender, EventArgs e)
		{
			Application.Quit ();
		}

		void EditMake_Activated (object sender, EventArgs e)
		{
			try {
				var controller = new TreeControllerCollectionEditor<Manufacturer> (new StringEqualityComparer (), _data.Makes);
				
				controller.EditablePropertyNames.AddRange (new [] { "CompanyName", "CompanyInitials" });
				
				ShowCollectionEditor ("Edit Auto Manufacturers", controller, true);
			} catch (Exception ex) {
				GtkSharp.Win.DialogFactory.ShowError (this, ex);
			}
		}

		void EditModel_Activated (object sender, EventArgs e)
		{
			try {
				var controller = new TreeControllerCollectionEditor<AutoModel> (new StringEqualityComparer (), _data.Models);

				controller.EditablePropertyNames.AddRange (new [] { "ModelName", "ModelType", "ModelSubtype", "Make" });
				controller.AddColumnDef<Manufacturer> ("Make", "Make", x => x.CompanyName, _data.Makes, _data.Makes.Comparer, false);

				ShowCollectionEditor ("Edit Auto Models", controller, true);
			} catch (Exception ex) {
				GtkSharp.Win.DialogFactory.ShowError (this, ex);
			}
		}

		void EditCars_Activated (object sender, EventArgs e)
		{
			try {
				var controller = new TreeControllerCollectionEditor<Automobile> (new StringEqualityComparer (), _data.Cars);

				controller.EditablePropertyNames.AddRange (new [] { "Make", "Model", "Year" });
				controller.AddColumnDef<Manufacturer> ("Make", "Make", x => x.CompanyName, _data.Makes, _data.Makes.Comparer, false);
				controller.AddColumnDef<AutoModel> ("Model", "Model", x => x.ModelName, _data.Models, _data.Models.Comparer, false);

				ShowCollectionEditor ("Edit Auto Models", controller, true);
			} catch (Exception ex) {
				GtkSharp.Win.DialogFactory.ShowError (this, ex);
			}
		}

		/// <summary>
		/// Shows the collection editor, using the given controller.
		/// </summary>
		/// <param name="title">The Window Title.</param>
		/// <param name="controller">The View Controller.</param>
		/// <param name="allowInsertDelete">If set to <c>true</c>, then allows insert delete.</param>
		void ShowCollectionEditor (string title, ITreeControllerCollectionEditor controller, bool allowInsertDelete)
		{
			var view = new GtkSharp.Win.gwinCollectionEditor2 (title, this, controller, allowInsertDelete);

			view.LoadTreeView ();

			view.ShowAll ();
			view.RunDialog ();
			view.Dispose ();
		}
	}
}

