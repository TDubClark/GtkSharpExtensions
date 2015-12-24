using System;

namespace GtkSharp.Extensions
{
	public static class ContainerExtensions
	{
		/// <summary>
		/// Wrap the specified widget in this container, returning the container { ex: vbox.PackStart(new ScrolledWindow().Wrap(new TreeView())) }.
		/// </summary>
		/// <param name="container">Container.</param>
		/// <param name="widget">Widget.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T Wrap<T> (this T container, Gtk.Widget widget) where T : Gtk.Container
		{
			container.Add (widget);
			return container;
		}

		/// <summary>
		/// Wrap the specified widget in this container, returning the container { ex: vbox.PackStart(new ScrolledWindow().Wrap(new TreeView())) }.
		/// </summary>
		/// <param name="container">Container.</param>
		/// <param name="widget">Widget.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T WrapPackStart<T> (this T container, Gtk.Widget widget) where T : Gtk.Box
		{
			container.PackStart (widget);
			return container;
		}

		/// <summary>
		/// Wrap the specified widget in this container, returning the container { ex: vbox.PackStart(new ScrolledWindow().Wrap(new TreeView())) }.
		/// </summary>
		/// <param name="container">Container.</param>
		/// <param name="widget">Widget.</param>
		/// <param name = "expand">Whether to expand the widget</param>
		/// <param name = "fill">Whether the widget should fill all available space</param>
		/// <param name = "padding"></param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T WrapPackStart<T> (this T container, Gtk.Widget widget, bool expand, bool fill, uint padding) where T : Gtk.Box
		{
			container.PackStart (widget, expand, fill, padding);
			return container;
		}


		/// <summary>
		/// Wrap the specified widget in this container, returning the container { ex: vbox.PackStart(new ScrolledWindow().Wrap(new TreeView())) }.
		/// </summary>
		/// <param name="container">Container.</param>
		/// <param name="widget">Widget.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T WrapPackEnd<T> (this T container, Gtk.Widget widget) where T : Gtk.Box
		{
			container.PackEnd (widget);
			return container;
		}

		/// <summary>
		/// Wrap the specified widget in this container, returning the container { ex: vbox.PackStart(new ScrolledWindow().Wrap(new TreeView())) }.
		/// </summary>
		/// <param name="container">Container.</param>
		/// <param name="widget">Widget.</param>
		/// <param name = "expand">Whether to expand the widget</param>
		/// <param name = "fill">Whether the widget should fill all available space</param>
		/// <param name = "padding"></param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T WrapPackEnd<T> (this T container, Gtk.Widget widget, bool expand, bool fill, uint padding) where T : Gtk.Box
		{
			container.PackEnd (widget, expand, fill, padding);
			return container;
		}
	}
}

