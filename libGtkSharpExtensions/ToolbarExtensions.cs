using System;

namespace GtkSharp.Extensions
{
	public static class ToolbarExtensions
	{
		/// <summary>
		/// Inserts a new Gtk.ToolButton, with the assigned click event handler, at the given position.
		/// </summary>
		/// <param name="toolbar">Toolbar.</param>
		/// <param name="icon_widget">Icon widget.</param>
		/// <param name="label">Label.</param>
		/// <param name="position">Position.</param>
		/// <param name="Clicked">Clicked Event Handler.</param>
		public static Gtk.ToolButton InsertToolButton (this Gtk.Toolbar toolbar, Gtk.Widget icon_widget, string label, int position, EventHandler Clicked)
		{
			Gtk.ToolButton button = new Gtk.ToolButton (icon_widget, label);
			button.Clicked += Clicked;

			toolbar.Insert (button, position);

			return button;
		}

		/// <summary>
		/// Appends a new Gtk.ToolButton.
		/// </summary>
		/// <returns>The tool button.</returns>
		/// <param name="toolbar">Toolbar.</param>
		/// <param name="label">Label.</param>
		public static Gtk.ToolButton AppendToolButton (this Gtk.Toolbar toolbar, string label)
		{
			Gtk.ToolButton button = new Gtk.ToolButton (null, label);

			toolbar.Insert (button, toolbar.NItems);

			return button;
		}

		/// <summary>
		/// Appends a new stock Gtk.ToolButton.
		/// </summary>
		/// <returns>The stock tool button.</returns>
		/// <param name="toolbar">Toolbar.</param>
		/// <param name="stock_id">Stock identifier.</param>
		public static Gtk.ToolButton AppendToolButtonStock (this Gtk.Toolbar toolbar, string stock_id)
		{
			Gtk.ToolButton button = new Gtk.ToolButton (stock_id);

			toolbar.Insert (button, toolbar.NItems);

			return button;
		}

		/// <summary>
		/// Appends a new Gtk.ToolButton.
		/// </summary>
		/// <returns>The tool button.</returns>
		/// <param name="toolbar">Toolbar.</param>
		/// <param name="icon_widget">The Icon widget</param>
		/// <param name="label">The label.</param>
		public static Gtk.ToolButton AppendToolButton (this Gtk.Toolbar toolbar, Gtk.Widget icon_widget, string label)
		{
			Gtk.ToolButton button = new Gtk.ToolButton (icon_widget, label);

			toolbar.Insert (button, toolbar.NItems);

			return button;
		}

		/// <summary>
		/// Appends a new Gtk.ToolButton.
		/// </summary>
		/// <param name="toolbar">Toolbar.</param>
		/// <param name="label">Label.</param>
		/// <param name="Clicked">Clicked Event Handler.</param>
		public static Gtk.ToolButton AppendToolButton (this Gtk.Toolbar toolbar, string label, EventHandler Clicked)
		{
			return AppendToolButton (toolbar, null, label, Clicked);
		}

		/// <summary>
		/// Appends a new Gtk.ToolButton.
		/// </summary>
		/// <returns>The tool button.</returns>
		/// <param name="toolbar">Toolbar.</param>
		/// <param name="icon_widget">Icon widget.</param>
		/// <param name="label">Label.</param>
		/// <param name="Clicked">Clicked Event Handler.</param>
		public static Gtk.ToolButton AppendToolButton (this Gtk.Toolbar toolbar, Gtk.Widget icon_widget, string label, EventHandler Clicked)
		{
			return toolbar.InsertToolButton (icon_widget, label, toolbar.NItems, Clicked);
		}
	}
}

