using System;
using Gtk;
using GtkSharp.Extensions;

namespace GtkSharpExtensions
{
	/// <summary>
	/// Main Gtk# Window
	/// </summary>
	public class gwinMain : Window
	{
		Automotive.AutomobileCollection _cars;

		Automotive.AutoModelCollection _models;

		Automotive.ManufacturerCollection _makes;

		public gwinMain ()
			: base (WindowType.Toplevel)
		{
			this.Title = "Gtk# Extensions Driver Application";

			Build ();

			_makes = new GtkSharpExtensions.Automotive.ManufacturerCollection ();
			_models = new GtkSharpExtensions.Automotive.AutoModelCollection ();
			_cars = new GtkSharpExtensions.Automotive.AutomobileCollection ();
		}

		void Build ()
		{
			this.WindowPosition = WindowPosition.Center;
			this.SetSizeRequest (500, 400);

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

			if (this.Child != null)
				this.Child.ShowAll ();

			this.Destroyed += GwinMain_Destroyed;
		}

		void GwinMain_Destroyed (object sender, EventArgs e)
		{
			Application.Quit ();
		}

		void EditMake_Activated (object sender, EventArgs e)
		{
			var controller = new TreeControllerCollectionEditor<Automotive.Manufacturer> (new StringEqualityComparer (), _makes);

			controller.EditablePropertyNames.AddRange (new []{ "CompanyName", "CompanyInitials" });

			var view = new GtkSharp.Win.gwinCollectionEditor2 ("Edit Auto Manufacturers", this, controller, true);

			view.LoadTreeView ();

			view.ShowAll ();
			view.RunDialog ();
			view.Dispose ();
		}

		void EditModel_Activated (object sender, EventArgs e)
		{
			throw new NotImplementedException ();
		}

		void EditCars_Activated (object sender, EventArgs e)
		{
			throw new NotImplementedException ();
		}
	}
}

