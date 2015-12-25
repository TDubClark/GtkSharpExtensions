using System;
using Gtk;

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
	}
}

