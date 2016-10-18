using System;
using System.Collections.Generic;

namespace GtkSharp.Extensions
{
	public static class TreeModelExtensions
	{
		/// <summary>
		/// Gets the Iterator to the previous node in the tree (same level).
		/// </summary>
		/// <returns><c>true</c>, if previous node was found, <c>false</c> otherwise.</returns>
		/// <param name="model">Model.</param>
		/// <param name="iter">The iterator for a node; will be pointed to the previous node (if found).</param>
		public static bool IterPrev (this Gtk.TreeModel model, ref Gtk.TreeIter iter)
		{
			Gtk.TreeIter parent;
			if (model.IterParent (out parent, iter)) {
				// This has a parent
				// Get the first child
				Gtk.TreeIter first;
				if (model.IterNthChild (out first, 0)) {
					if (GetIterPrev (model, first, ref iter)) {
						return true;
					}
				}
			} else {
				// No parent
				Gtk.TreeIter first;
				if (model.GetIterFirst (out first)) {
					if (GetIterPrev (model, first, ref iter)) {
						return true;
					}
				}
			}

			return false;
		}

		/// <summary>
		/// Gets the previous iter, using the first sibling.
		/// </summary>
		/// <returns><c>true</c>, if iter previous was gotten, <c>false</c> otherwise.</returns>
		/// <param name="model">Model.</param>
		/// <param name="firstSibling">The first sibling.</param>
		/// <param name="iter">The iter, which will be assigned to the previous node (if found).</param>
		static bool GetIterPrev (Gtk.TreeModel model, Gtk.TreeIter firstSibling, ref Gtk.TreeIter iter)
		{
			Gtk.TreeIter iterPrev = firstSibling;
			Gtk.TreeIter iterNext = firstSibling;

			while (model.IterNext (ref iterNext)) {
				if (iterNext.Equals (iter)) {
					iter = iterPrev;
					return true;
				}
				// iterPrev tags along
				iterPrev = iterNext;
			}
			return false;
		}

		/// <summary>
		/// Gets the parent items for this TreeModel.
		/// </summary>
		/// <returns>The parent items.</returns>
		/// <param name="model">Model.</param>
		public static IEnumerable<Gtk.TreeIter> GetParentItems (this Gtk.TreeModel model)
		{
			return new TreeModelItemEnumerator (model);
		}

		/// <summary>
		/// Gets the children for the given parent.
		/// </summary>
		/// <returns>The children.</returns>
		/// <param name="model">Model.</param>
		/// <param name="parent">Parent.</param>
		public static IEnumerable<Gtk.TreeIter> GetChildren (this Gtk.TreeModel model, Gtk.TreeIter parent)
		{
			return new TreeModelChildEnumerator (model, parent);
		}

		/// <summary>
		/// Gets the model object from path.
		/// </summary>
		/// <returns><c>true</c>, if model object from path was gotten, <c>false</c> otherwise.</returns>
		/// <param name="model">Model.</param>
		/// <param name="path">Path.</param>
		/// <param name="value">Value.</param>
		public static bool GetModelObjectFromPath (this Gtk.TreeModel model, string path, out object value)
		{
			Gtk.TreeIter iter;
			if (model.GetIterFromString (out iter, path)) {
				value = model.GetValue (iter, 0);
				return true;
			}
			value = null;
			return false;
		}

		/// <summary>
		/// Finds the item; and performs the given function on each item in the tree (depth-first) until the function returns true.
		/// </summary>
		/// <returns><c>true</c>, if item was found, <c>false</c> otherwise.</returns>
		/// <param name="model">Model.</param>
		/// <param name="isFoundFunc">Is found func.</param>
		public static bool FindItem (this Gtk.TreeModel model, Func<Gtk.TreeModel, Gtk.TreeIter, bool> isFoundFunc)
		{
			Gtk.TreeIter iter = Gtk.TreeIter.Zero;
			foreach (var parent in model.GetParentItems ()) {
				if (model.FindItemRecursive (parent, isFoundFunc, ref iter))
					return true;
			}
			return false;
		}

		/// <summary>
		/// Tries to update the given item, if it is found.
		/// </summary>
		/// <returns><c>true</c>, if update item was tryed, <c>false</c> otherwise.</returns>
		/// <param name="model">Model.</param>
		/// <param name="isFoundFunc">Is found func.</param>
		/// <param name="column">Column.</param>
		/// <param name="newItem">New item.</param>
		public static bool TryUpdateItem (this Gtk.TreeModel model, Func<Gtk.TreeModel, Gtk.TreeIter, bool> isFoundFunc, int column, object newItem)
		{
			Gtk.TreeIter iter;
			if (model.FindItem (isFoundFunc, out iter)) {
				model.SetValue (iter, column, newItem);
				return true;
			}
			return false;
		}

		/// <summary>
		/// Finds the item recursively.
		/// </summary>
		/// <returns><c>true</c>, if item recursive was found, <c>false</c> otherwise.</returns>
		/// <param name="model">Model.</param>
		/// <param name="currentIterator">Current iterator.</param>
		/// <param name="isFoundFunc">Is found func.</param>
		internal static bool FindItemRecursive (this Gtk.TreeModel model, Gtk.TreeIter currentIterator, Func<Gtk.TreeModel, Gtk.TreeIter, bool> isFoundFunc)
		{
			Gtk.TreeIter iter = Gtk.TreeIter.Zero;
			return model.FindItemRecursive (currentIterator, isFoundFunc, ref iter);
		}

		/// <summary>
		/// Finds the item; searches the parents first, then depth-first children.
		/// </summary>
		/// <returns><c>true</c>, if item was found, <c>false</c> otherwise.</returns>
		/// <param name="model">Model.</param>
		/// <param name="isFoundFunc">Is found func.</param>
		/// <param name="iter">Iter.</param>
		public static bool FindItem (this Gtk.TreeModel model, Func<Gtk.TreeModel, Gtk.TreeIter, bool> isFoundFunc, out Gtk.TreeIter iter)
		{
			// Search the top level first
			foreach (var parent in model.GetParentItems ()) {
				if (isFoundFunc (model, parent)) {
					iter = parent;
					return true;
				}
			}

			// Search the children
			iter = Gtk.TreeIter.Zero;
			foreach (var parent in model.GetParentItems ()) {
				if (model.FindItemRecursiveChildren (parent, isFoundFunc, ref iter)) {
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Finds the item recursively.
		/// </summary>
		/// <returns><c>true</c>, if item recursive was found, <c>false</c> otherwise.</returns>
		/// <param name="model">Model.</param>
		/// <param name="currentIterator">Current iterator.</param>
		/// <param name="isFoundFunc">Is found func.</param>
		/// <param name="iter">Iter.</param>
		internal static bool FindItemRecursive (this Gtk.TreeModel model, Gtk.TreeIter currentIterator, Func<Gtk.TreeModel, Gtk.TreeIter, bool> isFoundFunc, ref Gtk.TreeIter iter)
		{
			// Look at this item
			if (isFoundFunc (model, currentIterator)) {
				iter = currentIterator;
				return true;
			}

			return model.FindItemRecursiveChildren (currentIterator, isFoundFunc, ref iter);
		}

		/// <summary>
		/// Finds the item recursively, searching only the children of the currentIterator.
		/// </summary>
		/// <returns><c>true</c>, if item was found, <c>false</c> otherwise.</returns>
		/// <param name="model">Model.</param>
		/// <param name="currentIterator">Current iterator.</param>
		/// <param name="isFoundFunc">Is found func.</param>
		/// <param name="iter">Iter.</param>
		internal static bool FindItemRecursiveChildren (this Gtk.TreeModel model, Gtk.TreeIter currentIterator, Func<Gtk.TreeModel, Gtk.TreeIter, bool> isFoundFunc, ref Gtk.TreeIter iter)
		{
			// Look at the children
			if (model.IterHasChild (currentIterator)) {
				// Children
				foreach (var child in model.GetChildren (currentIterator)) {
					if (model.FindItemRecursive (child, isFoundFunc, ref iter))
						return true;
				}
			}

			return false;
		}


		/// <summary>
		/// Tries to find the given item; if found, outputs the Iterator which points to that item and returns true; else, returns <c>false</c>.
		/// </summary>
		/// <returns><c>true</c>, if the item was found, <c>false</c> otherwise.</returns>
		/// <param name="model">Model.</param>
		/// <param name="modelColumnIndex">The column index of the model, where items of type <c>T</c> are found, which are compared against the given item.</param>
		/// <param name="item">Item.</param>
		/// <param name="comparer">Comparer.</param>
		/// <param name="iterator">Iterator.</param>
		/// <typeparam name="T">The type of item found in the model at the given column index.</typeparam>
		public static bool TryFindIter<T> (this Gtk.ListStore model, int modelColumnIndex, T item, IEqualityComparer<T> comparer, out Gtk.TreeIter iterator)
		{
			iterator = Gtk.TreeIter.Zero;

			foreach (var parent in model.GetParentItems()) {
				if (comparer.Equals (item, (T)model.GetValue (parent, modelColumnIndex))) {
					iterator = parent;
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Tries to find the given item; if found, outputs the Iterator which points to that item and returns true; else, returns <c>false</c>.
		/// </summary>
		/// <returns><c>true</c>, if the item was found, <c>false</c> otherwise.</returns>
		/// <param name="model">Model.</param>
		/// <param name="modelColumnIndex">The column index of the model, where items are found, which are compared against the given item.</param>
		/// <param name="item">Item.</param>
		/// <param name="comparer">Comparer.</param>
		/// <param name="iterator">Iterator.</param>
		public static bool TryFindIter (this Gtk.ListStore model, int modelColumnIndex, object item, System.Collections.IEqualityComparer comparer, out Gtk.TreeIter iterator)
		{
			iterator = Gtk.TreeIter.Zero;

			foreach (var parent in model.GetParentItems()) {
				if (comparer.Equals (item, model.GetValue (parent, modelColumnIndex))) {
					iterator = parent;
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Tries to find the given item among the parent items; if found, outputs the Iterator which points to that item and returns true; else, returns <c>false</c>.
		/// </summary>
		/// <returns><c>true</c>, if the item was found, <c>false</c> otherwise.</returns>
		/// <param name="model">Model.</param>
		/// <param name="modelColumnIndex">The column index of the model, where items of type <c>T</c> are found, which are compared against the given item.</param>
		/// <param name="item">Item.</param>
		/// <param name="comparer">Comparer.</param>
		/// <param name="iterator">Iterator.</param>
		/// <typeparam name="T">The type of item found in the model at the given column index.</typeparam>
		public static bool TryFindIter<T> (this Gtk.TreeModel model, int modelColumnIndex, T item, IEqualityComparer<T> comparer, out Gtk.TreeIter iterator)
		{
			iterator = Gtk.TreeIter.Zero;

			foreach (var parent in model.GetParentItems()) {
				if (comparer.Equals (item, (T)model.GetValue (parent, modelColumnIndex))) {
					iterator = parent;
					return true;
				}
			}
			return false;
		}

		public static bool TrySetActiveIter<T> (this Gtk.ComboBox widget, int modelColumnIndex, T item, IEqualityComparer<T> comparer)
		{
			Gtk.TreeIter iter;
			if (widget.Model.TryFindIter<T> (modelColumnIndex, item, comparer, out iter)) {
				widget.SetActiveIter (iter);
				return true;
			}
			return false;
		}

		public static bool TrySetActiveIter<T> (this Gtk.ComboBox widget, int modelColumnIndex, Predicate<T> match)
		{
			Gtk.TreeIter iter;
			if (widget.Model.TryFindIter<T> (modelColumnIndex, x => match (x), out iter)) {
				widget.SetActiveIter (iter);
				return true;
			}
			return false;
		}

		/// <summary>
		/// Tries the find iter.
		/// </summary>
		/// <returns><c>true</c>, if find iter was tryed, <c>false</c> otherwise.</returns>
		/// <param name="model">Model.</param>
		/// <param name="modelColumnIndex">Model column index.</param>
		/// <param name="isItemFound">Is item found.</param>
		/// <param name="iterator">Iterator.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static bool TryFindIter<T> (this Gtk.TreeModel model, int modelColumnIndex, Func<T, bool> isItemFound, out Gtk.TreeIter iterator)
		{
			iterator = Gtk.TreeIter.Zero;

			foreach (var parent in model.GetParentItems()) {
				if (isItemFound ((T)model.GetValue (parent, modelColumnIndex))) {
					iterator = parent;
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Appends the values as separate items.
		/// </summary>
		/// <param name="model">Model.</param>
		/// <param name="valueCollection">Collection of values.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static void AppendValuesAsItems<T> (this Gtk.ListStore model, IEnumerable<T> valueCollection)
		{
			if (valueCollection == null)
				throw new ArgumentNullException ("valueCollection");
			foreach (var value in valueCollection) {
				model.AppendValues (value);
			}
		}

		/// <summary>
		/// Appends the values as separate items.
		/// </summary>
		/// <param name="model">Model.</param>
		/// <param name="valueCollection">Collection of values.</param>
		/// <param name = "getValues">Gets the array of values for the given item</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static void AppendValuesAsItems<T> (this Gtk.ListStore model, IEnumerable<T> valueCollection, Func<T, object[]> getValues)
		{
			if (valueCollection == null)
				throw new ArgumentNullException ("valueCollection");
			foreach (var value in valueCollection) {
				model.AppendValues (getValues (value));
			}
		}

		/// <summary>
		/// Appends the values as separate items.
		/// </summary>
		/// <param name="model">Model.</param>
		/// <param name="valueCollection">Collection of values.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static void AppendValuesAsItems<T> (this Gtk.TreeStore model, IEnumerable<T> valueCollection)
		{
			if (valueCollection == null)
				throw new ArgumentNullException ("valueCollection");
			foreach (var value in valueCollection) {
				model.AppendValues (value);
			}
		}

		/// <summary>
		/// Appends the values as separate items.
		/// </summary>
		/// <param name="model">Model.</param>
		/// <param name="valueCollection">Collection of values.</param>
		/// <param name = "getValues">Gets the array of values for the given item</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static void AppendValuesAsItems<T> (this Gtk.TreeStore model, IEnumerable<T> valueCollection, Func<T, object[]> getValues)
		{
			if (valueCollection == null)
				throw new ArgumentNullException ("valueCollection");
			foreach (var value in valueCollection) {
				model.AppendValues (getValues (value));
			}
		}

		/// <summary>
		/// Gets this collection as a Gtk ListStore (for use in ComboBox and TreeView widgets).
		/// </summary>
		/// <returns>The list store.</returns>
		/// <param name="collection">Collection.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static Gtk.ListStore ToListStore<T> (this IEnumerable<T> collection)
		{
			if (collection == null)
				throw new ArgumentNullException ("collection");
			
			Gtk.ListStore store = new Gtk.ListStore (typeof(T));
			store.AppendValuesAsItems (collection);

			return store;
		}
	}
}

