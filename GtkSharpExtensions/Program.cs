using System;
using Gtk;

namespace GtkSharpExtensions
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Application.Init ();
			gwinMain win = new gwinMain ();
			win.Show ();
			Application.Run ();
		}
	}
}
