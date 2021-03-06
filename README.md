# GtkSharpExtensions
Mono/.NET Extensions for the Gtk# library

Purpose
=======

I had a difficult time learning Gtk#, but I'm glad I took the time, because I find it to be a very powerful widget toolkit.

Most challenging to me was learning widget packing and the TreeView widget (I'm still learning some of the nuances).
<br /><br />


I wanted to make this easier for other newcomers: faster to get started, and plenty of examples to learn from.

To achieve these purposes, I tried to mask some of the complexity for common tasks (which can be learned later), and also provide some useful building blocks (ex: a collection editor).
<br /><br />

My background is in enterprise-level data management applications, so the tools tend toward text-based menus, buttons, and data viewing/editing.
<br /><br />

Please keep in mind, I'm not an expert in Gtk - I just got through the beginner stage myself, and I'm hoping to pave the way a little bit.

Any comments or recommendations would be welcome.


Features
========

Extension Methods:

1. Provide more compact syntax for many common Gtk# widgets, such as Menus, ComboBoxes, and TreeViews
2. Are designed to significantly reduce key strokes for common tasks when building UIs with C#
3. Should feel somewhat more familiar to C# programmers
4. Mask some of the complexity for those learning Gtk# (you can look at the code to see how it works).

Existing classes:
(see class library project libGtkSharpExtensions)

1. An editor for a collection of objects using a Gtk# TreeView
   * TreeControllerCollectionEditor.cs - this is the controller in the Model-View-Controller pattern
   * gwinCollectionEditor2.cs - this is the view in the Model-View-Controller pattern
   * [The model is your collection]
2. A Dialog factory (DialogFactory.cs)


How to Use
==========

1. Download the source.
2. Attach the project libGtkSharpExtensions to your solution.
3. Add references to this project from your Gtk# project.
4. Add the namespaces: using GtkSharp.Extensions, using GtkSharp.Win


Examples
========

You can download the source and run it from MonoDevelop (Xamarin Studio on Mac or Windows).
The main project GtkSharpExtensions is a driver application, with examples of building a menu and editing collections.


Notes
=====

I built these with MonoDevelop 5.10, and they should run in Visual Studio 2012 or later.

The licensed is MIT, which I hope will ease any licensing headaches with other Mono software.
These libraries are dynamically linked to the Gtk# libraries, so no need to follow the GNU licensing.

If interested, you should be able to use these classes in both open source and proprietary projects.
<br /><br />


Travis Clark,
Initial Developer
