using System;
using Gtk;

namespace GtkSharp.Extensions
{
	public static class WidgetExtensions
	{
		/// <summary>
		/// Sets the Date value of this calendar widget.
		/// </summary>
		/// <param name="widget">Widget.</param>
		/// <param name="value">The DateTime value.</param>
		public static void SetDate (this Gtk.Calendar widget, DateTime value)
		{
			widget.SelectMonth ((uint)value.Month, (uint)value.Year);
			widget.SelectDay ((uint)value.Day);
		}

		public static Gtk.ResponseType RunDialog (this Gtk.Dialog dialog)
		{
			return (Gtk.ResponseType)dialog.Run ();
		}

		public static void PackWithHBox (this Gtk.Dialog dialog, Gtk.Widget widget, bool expand, bool fill, uint padding)
		{
			PackWithHBox (dialog, widget, expand, fill, padding, padding);
		}

		public static void PackWithHBox (this Gtk.Dialog dialog, Gtk.Widget widget, bool expand, bool fill, uint horizontalPadding, uint verticalPadding)
		{
			PackWithHBox (dialog, widget, false, 0, expand, expand, fill, fill, horizontalPadding, verticalPadding);
		}

		public static void PackWithHBox (this Gtk.Dialog dialog, Gtk.Widget widget, bool hboxHomogenous, int hboxSpacing, bool expand, bool fill, uint horizontalPadding, uint verticalPadding)
		{
			PackWithHBox (dialog, widget, hboxHomogenous, hboxSpacing, expand, expand, fill, fill, horizontalPadding, verticalPadding);
		}

		public static void PackWithHBox (this Gtk.Dialog dialog, Gtk.Widget widget
			, bool hboxHomogenous, int hboxSpacing
			, bool expandHorizontal, bool expandVertical
			, bool fillHorizontal, bool fillVertical
			, uint horizontalPadding, uint verticalPadding)
		{
			Gtk.HBox hbx = new HBox (hboxHomogenous, hboxSpacing);
			hbx.PackStart (widget, expandHorizontal, fillHorizontal, horizontalPadding);
			dialog.VBox.PackStart (hbx, expandVertical, fillVertical, verticalPadding);
		}

		/// <summary>
		/// Nests this widget in a Gtk HBox.
		/// </summary>
		/// <returns>The in HBox.</returns>
		/// <param name="widget">Widget.</param>
		/// <param name="homogeneous">If set to <c>true</c> homogeneous.</param>
		/// <param name="spacing">Spacing.</param>
		public static Gtk.HBox NestInHBox (this Gtk.Widget widget, bool homogeneous, int spacing)
		{
			Gtk.HBox box = new HBox (homogeneous, spacing);
			box.PackStart (widget);
			return box;
		}

		/// <summary>
		/// Nests this widget in a Gtk HBox.
		/// </summary>
		/// <returns>The in HBox.</returns>
		/// <param name="widget">Widget.</param>
		/// <param name="homogeneous">If set to <c>true</c> homogeneous.</param>
		/// <param name="spacing">Spacing.</param>
		/// <param name="expand">If set to <c>true</c> expand.</param>
		/// <param name="fill">If set to <c>true</c> fill.</param>
		/// <param name="padding">Padding.</param>
		public static Gtk.HBox NestInHBox (this Gtk.Widget widget, bool homogeneous, int spacing, bool expand, bool fill, uint padding)
		{
			Gtk.HBox box = new HBox (homogeneous, spacing);
			box.PackStart (widget, expand, fill, padding);
			return box;
		}

		/// <summary>
		/// Nests this widget in a Gtk HBox.
		/// </summary>
		/// <returns>The in HBox.</returns>
		/// <param name="widget">Widget.</param>
		/// <param name="spacing">Spacing.</param>
		/// <param name="expand">If set to <c>true</c> expand.</param>
		/// <param name="fill">If set to <c>true</c> fill.</param>
		/// <param name="padding">Padding.</param>
		public static Gtk.HBox NestInHBox (this Gtk.Widget widget, int spacing, bool expand, bool fill, uint padding)
		{
			Gtk.HBox box = new HBox (false, spacing);
			box.PackStart (widget, expand, fill, padding);
			return box;
		}

		/// <summary>
		/// Creates a new Button, and Packs to the start button, and assigns the given Clicked event handler.
		/// </summary>
		/// <returns>The new button.</returns>
		/// <param name="box">Box.</param>
		/// <param name="label">Label.</param>
		/// <param name="clickEventHanlder">Click event hanlder.</param>
		/// <param name="expand">If set to <c>true</c> expand.</param>
		/// <param name="fill">If set to <c>true</c> fill.</param>
		/// <param name="padding">Padding.</param>
		public static Button PackStartButton (this ButtonBox box, string label, EventHandler clickEventHanlder, bool expand, bool fill, uint padding)
		{
			return box.PackStartButton (new Button (label), clickEventHanlder, expand, fill, padding);
		}

		/// <summary>
		/// Packs the given button into this ButtonBox, and assigns the given Clicked event handler.
		/// </summary>
		/// <returns>The start button.</returns>
		/// <param name="box">Box.</param>
		/// <param name="widget">Widget.</param>
		/// <param name="clickEventHanlder">Click event hanlder.</param>
		/// <param name="expand">If set to <c>true</c> expand.</param>
		/// <param name="fill">If set to <c>true</c> fill.</param>
		/// <param name="padding">Padding.</param>
		public static Button PackStartButton (this ButtonBox box, Button widget, EventHandler clickEventHanlder, bool expand, bool fill, uint padding)
		{
			widget.Clicked += clickEventHanlder;
			box.PackStart (widget, expand, fill, padding);
			return widget;
		}

		/// <summary>
		/// Creates a new Button, and packs it into this ButtonBox (from the start); defaults to expand=false, fill=false.
		/// </summary>
		/// <returns>The start button.</returns>
		/// <param name="box">Box.</param>
		/// <param name="label">The button label.</param>
		/// <param name="padding">Horizontal padding around the button.</param>
		public static Button PackStartButton (this ButtonBox box, string label, uint padding)
		{
			bool expand = false;
			bool fill = false;
			return box.PackStartButton (new Button (label), expand, fill, padding);
		}

		/// <summary>
		/// Creates a new Button, and packs it into this ButtonBox (from the start).
		/// </summary>
		/// <returns>The new button.</returns>
		/// <param name="box">Box.</param>
		/// <param name="label">Label.</param>
		/// <param name="expand">If set to <c>true</c> expand.</param>
		/// <param name="fill">If set to <c>true</c> fill.</param>
		/// <param name="padding">Horizontal padding around the button.</param>
		public static Button PackStartButton (this ButtonBox box, string label, bool expand, bool fill, uint padding)
		{
			return box.PackStartButton (new Button (label), expand, fill, padding);
		}

		/// <summary>
		/// Packs the given button into this ButtonBox.
		/// </summary>
		/// <returns>The start button.</returns>
		/// <param name="box">Box.</param>
		/// <param name="widget">Widget.</param>
		/// <param name="expand">If set to <c>true</c> expand.</param>
		/// <param name="fill">If set to <c>true</c> fill.</param>
		/// <param name="padding">Horizontal padding around the button.</param>
		public static Button PackStartButton (this ButtonBox box, Button widget, bool expand, bool fill, uint padding)
		{
			box.PackStart (widget, expand, fill, padding);
			return widget;
		}

		/// <summary>
		/// Creates a new Button, and Packs to the end button.
		/// </summary>
		/// <returns>The new button.</returns>
		/// <param name="box">Box.</param>
		/// <param name="label">Label.</param>
		/// <param name="clickEventHanlder">Click event hanlder.</param>
		/// <param name="expand">If set to <c>true</c> expand.</param>
		/// <param name="fill">If set to <c>true</c> fill.</param>
		/// <param name="padding">Padding.</param>
		public static Button PackEndButton (this ButtonBox box, string label, EventHandler clickEventHanlder, bool expand, bool fill, uint padding)
		{
			return box.PackEndButton (new Button (label), clickEventHanlder, expand, fill, padding);
		}

		public static Button PackEndButton (this ButtonBox box, Button widget, EventHandler clickEventHanlder, bool expand, bool fill, uint padding)
		{
			widget.Clicked += clickEventHanlder;
			box.PackEnd (widget, expand, fill, padding);
			return widget;
		}
	}
}

