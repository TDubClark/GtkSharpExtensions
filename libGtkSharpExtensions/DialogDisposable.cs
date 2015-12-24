using System;

namespace GtkSharp.Extensions
{
	/// <summary>
	/// Wrapper class for a Gtk.Dialog object; destroys the dialog on disposal (ideal for a C# using statement)
	/// Usage (C#): using (var dlg = new DialogDisposable("Test", this, "OK", "Cancel")) { if (dlg.RunDialog() == Gtk.ResponseType.Accept) { ... } }
	/// </summary>
	public class DialogDisposable : IDisposable
	{
		protected Gtk.Dialog _dialog;

		/// <summary>
		/// Gets or sets the dialog object.
		/// </summary>
		/// <value>The dialog object.</value>
		public Gtk.Dialog DialogObject {
			get { return _dialog; }
			set { _dialog = value; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="GtkSharp.Extensions.DialogDisposable"/> class.
		/// </summary>
		/// <param name="title">Title.</param>
		/// <param name="parent">Parent.</param>
		/// <param name="closeButtonLabel">Close button label.</param>
		public DialogDisposable (string title, Gtk.Window parent, string closeButtonLabel)
			: this (title, parent, new object[] { closeButtonLabel, Gtk.ResponseType.Close })
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="GtkSharp.Extensions.DialogDisposable"/> class.
		/// </summary>
		/// <param name="title">Title.</param>
		/// <param name="parent">Parent.</param>
		/// <param name="acceptButtonLabel">Accept button label (If clicked, returns Gtk.ResponseType.Accept).</param>
		/// <param name="cancelButtonLabel">Cancel button label (If clicked, returns Gtk.ResponseType.Cancel).</param>
		public DialogDisposable (string title, Gtk.Window parent, string acceptButtonLabel, string cancelButtonLabel)
			: this (title, parent, new object[] {
					acceptButtonLabel,
					Gtk.ResponseType.Accept,
					cancelButtonLabel,
					Gtk.ResponseType.Cancel
				})
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="GtkSharp.Extensions.DialogDisposable"/> class.
		/// </summary>
		/// <param name="title">Title.</param>
		/// <param name="parent">Parent.</param>
		/// <param name="buttonData">Button data (for Dialog constructor; pattern of { string button1Label, Gtk.ResponseType button1Response, [ button2 args ]... } ).</param>
		public DialogDisposable (string title, Gtk.Window parent, params object[] buttonData)
			: this (new Gtk.Dialog (title, parent
				, Gtk.DialogFlags.Modal | Gtk.DialogFlags.DestroyWithParent
				, buttonData))
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="GtkSharp.Extensions.DialogDisposable"/> class.
		/// </summary>
		/// <param name="dialog">The Gtk# Dialog.</param>
		public DialogDisposable (Gtk.Dialog dialog)
		{
			if (dialog == null)
				throw new ArgumentNullException ("dialog");
			_dialog = dialog;
		}

		/// <summary>
		/// Calls the ShowAll() method on the dialog ( automatically called by the RunDialog() method ).
		/// </summary>
		public void DialogShowAll ()
		{
			_dialog.ShowAll ();
		}

		/// <summary>
		/// Shows all Widgets and runs the dialog.
		/// </summary>
		/// <returns>The dialog.</returns>
		public Gtk.ResponseType RunDialog ()
		{
			DialogShowAll ();
			return _dialog.RunDialog ();
		}

		/// <summary>
		/// Gets the Dialog's VBox container.
		/// </summary>
		/// <value>The V box.</value>
		public Gtk.VBox VBox {
			get { return _dialog.VBox; }
		}

		/// <summary>
		/// Adds the widget to the dialog's VBox.
		/// </summary>
		/// <param name="widget">Widget.</param>
		public void AddWidget (Gtk.Widget widget)
		{
			_dialog.VBox.PackStart (widget);
		}

		/// <summary>
		/// Adds the widget to the dialog's VBox.
		/// </summary>
		/// <param name="widget">Widget.</param>
		/// <param name="expand">If set to <c>true</c> expand.</param>
		/// <param name="fill">If set to <c>true</c> fill.</param>
		/// <param name="padding">Padding.</param>
		public void AddWidget (Gtk.Widget widget, bool expand, bool fill, uint padding)
		{
			_dialog.VBox.PackStart (widget, expand, fill, padding);
		}

		/// <summary>
		/// Adds the widget to the dialog's VBox, surrounding with a new Gtk.HBox (non-homogenous, no spacing).
		/// </summary>
		/// <param name="widget">Widget.</param>
		/// <param name="expand">If set to <c>true</c> expand.</param>
		/// <param name="fill">If set to <c>true</c> fill.</param>
		/// <param name="padding">Padding.</param>
		public void AddWidgetHBox (Gtk.Widget widget, bool expand, bool fill, uint padding)
		{
			_dialog.PackWithHBox (widget, expand, fill, padding);
		}

		/// <summary>
		/// Adds the widget to the dialog's VBox, surrounding with a new Gtk.HBox (non-homogenous, no spacing).
		/// </summary>
		/// <param name="widget">Widget.</param>
		/// <param name="expand">If set to <c>true</c> expand.</param>
		/// <param name="fill">If set to <c>true</c> fill.</param>
		/// <param name="horizontalPadding">Horizontal padding.</param>
		/// <param name="verticalPadding">Vertical padding.</param>
		public void AddWidgetHBox (Gtk.Widget widget, bool expand, bool fill, uint horizontalPadding, uint verticalPadding)
		{
			_dialog.PackWithHBox (widget, expand, fill, horizontalPadding, verticalPadding);
		}

		#region IDisposable implementation

		/// <summary>
		/// Releases all resource used by the <see cref="GtkSharp.Extensions.DialogDisposable"/> object.
		/// </summary>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="GtkSharp.Extensions.DialogDisposable"/>.
		/// The <see cref="Dispose"/> method leaves the <see cref="GtkSharp.Extensions.DialogDisposable"/> in an unusable
		/// state. After calling <see cref="Dispose"/>, you must release all references to the
		/// <see cref="GtkSharp.Extensions.DialogDisposable"/> so the garbage collector can reclaim the memory that the
		/// <see cref="GtkSharp.Extensions.DialogDisposable"/> was occupying.</remarks>
		public void Dispose ()
		{
			_dialog.Destroy ();
		}

		#endregion
	}
}

