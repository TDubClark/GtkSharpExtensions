using System;

namespace GtkSharp.Extensions
{
	public static class MenuExtensions
	{
		/// <summary>
		/// Appends a new Gtk.SeparatorMenuItem to the menu.
		/// </summary>
		/// <param name="menu">Menu.</param>
		public static void AppendSeparator (this Gtk.Menu menu)
		{
			menu.Append (new Gtk.SeparatorMenuItem ());
		}

		public static Gtk.MenuItem AppendNewMenuItem (this Gtk.Menu menu, string label)
		{
			var menuItem = new Gtk.MenuItem (label);

			menu.Append (menuItem);

			return menuItem;
		}

		/// <summary>
		/// Appends a new MenuItem to the menu.
		/// </summary>
		/// <param name="menu">Menu.</param>
		/// <param name="label">The MenuItem label.</param>
		/// <param name="activatedHandler">The MenuItem Activated event handler.</param>
		public static Gtk.MenuItem AppendNewMenuItem (this Gtk.Menu menu, string label, EventHandler activatedHandler)
		{
			var menuItem = new Gtk.MenuItem (label);

			menu.AppendNewMenuItem (menuItem, activatedHandler);

			return menuItem;
		}

		/// <summary>
		/// Appends the new MenuItem.
		/// </summary>
		/// <param name="menu">Menu.</param>
		/// <param name="menuItem">The MenuItem to add to this menu.</param>
		/// <param name="activatedHandler">The MenuItem Activated event handler.</param>
		public static void AppendNewMenuItem (this Gtk.Menu menu, Gtk.MenuItem menuItem, EventHandler activatedHandler)
		{
			menuItem.Activated += activatedHandler;
			menu.Append (menuItem);
		}

		/// <summary>
		/// Appends a new MenuItem with the given label, and with the given sub-menu.
		/// </summary>
		/// <param name="menuBar">Menu bar.</param>
		/// <param name="label">Label.</param>
		/// <param name="submenu">Submenu.</param>
		public static Gtk.MenuItem AppendNewMenu (this Gtk.MenuBar menuBar, string label, Gtk.Menu submenu)
		{
			var menuItem = new Gtk.MenuItem (label);

			menuBar.AppendNewMenu (menuItem, submenu);

			return menuItem;
		}

		/// <summary>
		/// Appends the new MenuItem, with the given sub-menu.
		/// </summary>
		/// <param name="menuBar">Menu bar.</param>
		/// <param name="menuItem">Menu item.</param>
		/// <param name="submenu">Submenu.</param>
		public static void AppendNewMenu (this Gtk.MenuBar menuBar, Gtk.MenuItem menuItem, Gtk.Menu submenu)
		{
			menuItem.Submenu = submenu;
			menuBar.Append (menuItem);
		}
	}
}

