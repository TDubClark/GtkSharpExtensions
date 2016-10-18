using System;

namespace GtkSharp.Extensions
{
	public static class DialogExtensions
	{
		/// <summary>
		/// Wraps this dialog in a new DialogDisposable class.
		/// </summary>
		/// <returns>The disposable.</returns>
		/// <param name="dialog">Dialog.</param>
		public static GtkSharp.Win.DialogDisposable WrapDisposable (this Gtk.Dialog dialog)
		{
			return new GtkSharp.Win.DialogDisposable (dialog);
		}
	}
}

