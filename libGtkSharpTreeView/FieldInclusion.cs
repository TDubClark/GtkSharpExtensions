using System;
using System.Collections.Generic;
using System.Linq;

namespace GtkSharp.Extensions
{
	public enum FieldInclusionListType
	{
		/// <summary>
		/// The list will be used to exclude fields (include is the default)
		/// </summary>
		ExcludeList,

		/// <summary>
		/// The list will be used to include fields (exclude is the default)
		/// </summary>
		IncludeList
	}

	/// <summary>
	/// Interface for determining field inclusion.
	/// </summary>
	public interface IFieldInclusion
	{
		/// <summary>
		/// Gets whether the specified fieldName should be included
		/// </summary>
		/// <param name="fieldName">Field name.</param>
		bool Included (string fieldName);
	}

	public class FieldInclusion : IFieldInclusion
	{
		/// <summary>
		/// Gets or sets the list of fields.
		/// </summary>
		/// <value>The fields.</value>
		public List<string> FieldList { get; set; }

		/// <summary>
		/// Gets or sets the type of the list - whether to use it for including or excluding fields.
		/// </summary>
		/// <value>The type of the list.</value>
		public FieldInclusionListType ListType { get; set; }

		/// <summary>
		/// Gets or sets the field name comparer.
		/// </summary>
		/// <value>The field name comparer.</value>
		public IEqualityComparer<string> FieldNameComparer { get; set; }

		/// <summary>
		/// Gets whether the specified fieldName should be included
		/// </summary>
		/// <param name="fieldName">Field name.</param>
		public bool Included (string fieldName)
		{
			bool inList = ListContains (fieldName);

			switch (this.ListType) {
				case FieldInclusionListType.ExcludeList:
					// If in the list, then DO NOT include
					return !inList;
				case FieldInclusionListType.IncludeList:
					// If in the list, then include
					return inList;
				default:
					throw new Exception (String.Format ("Unhandled list type: '{0}'", this.ListType));
			}
		}

		/// <summary>
		/// Gets whether the FieldList contains the given field name.
		/// </summary>
		/// <returns><c>true</c>, if contains was listed, <c>false</c> otherwise.</returns>
		/// <param name="fieldName">Field name.</param>
		public bool ListContains (string fieldName)
		{
			if (this.FieldNameComparer == null)
				return this.FieldList.Contains (fieldName);
			
			return this.FieldList.Contains (fieldName, this.FieldNameComparer);
		}

		/// <summary>
		/// Default constructor - make sure to set each of the properties, and add items to the FieldList
		/// </summary>
		public FieldInclusion ()
		{
			this.FieldList = new List<string> ();
			this.ListType = FieldInclusionListType.ExcludeList;
			this.FieldNameComparer = null;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="libGtkSharpTreeView.FieldInclusion"/> class.
		/// </summary>
		/// <param name="fieldList">Field list.</param>
		/// <param name="listType">List type.</param>
		/// <param name="fieldNameComparer">Field name comparer.</param>
		public FieldInclusion (IEnumerable<string> fieldList, FieldInclusionListType listType, IEqualityComparer<string> fieldNameComparer)
			: this ()
		{
			if (fieldNameComparer == null)
				throw new ArgumentNullException ("fieldNameComparer");
			if (fieldList == null)
				throw new ArgumentNullException ("fieldList");
			
			this.FieldList.AddRange (fieldList);
			this.ListType = listType;
			this.FieldNameComparer = fieldNameComparer;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="libGtkSharpTreeView.FieldInclusion"/> class.
		/// </summary>
		/// <param name="listType">List type.</param>
		/// <param name="fieldNameComparer">Field name comparer.</param>
		/// <param name="fieldList">Field list.</param>
		public FieldInclusion (FieldInclusionListType listType, IEqualityComparer<string> fieldNameComparer, params string[] fieldList)
			: this (fieldList, listType, fieldNameComparer)
		{
		}
	}
}

