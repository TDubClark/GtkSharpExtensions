using System;
using System.Collections.Generic;
using Gtk;

namespace GtkSharp.Extensions
{
	public interface IObjectCreator<T>
	{
		T CreateNew ();
	}

}

