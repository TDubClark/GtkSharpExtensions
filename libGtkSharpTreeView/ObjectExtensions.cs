namespace GtkSharp.Extensions
{
	/// <summary>
	/// Basic extensions for the Object class.
	/// </summary>
	internal static class ObjectExtensions
	{
		// These sorts of extension calls on Null objects still work
		// Reference: http://stackoverflow.com/questions/847209/in-c-what-happens-when-you-call-an-extension-method-on-a-null-object

		/// <summary>
		/// Gets the nullable string (returns NULL if this is Null or DBNull).
		/// </summary>
		/// <returns>The nullable string.</returns>
		/// <param name="value">Value.</param>
		internal static string ToNullableString (this object value)
		{
			if (value == null)
				return null;
			if (System.DBNull.Value.Equals (value))
				return null;
			return value.ToString ();
		}

		/// <summary>
		/// Gets the defaultable string (returns the [defaultValue] if this object is Null or DBNull).
		/// </summary>
		/// <returns>The nullable string.</returns>
		/// <param name="value">Value.</param>
		/// <param name="defaultValue">The default value, returned if this object is Null or DBNull</param>
		internal static string ToDefaultableString (this object value, string defaultValue)
		{
			if (value == null)
				return defaultValue;
			if (System.DBNull.Value.Equals (value))
				return defaultValue;
			return value.ToString ();
		}

		/// <summary>
		/// Gets the nullable value (if DBNull, returns Null; else returns [this]).
		/// </summary>
		/// <returns>The nullable value.</returns>
		/// <param name="value">Value.</param>
		internal static object ToNullable (this object value)
		{
			if (value == null)
				return null;
			if (System.DBNull.Value.Equals (value))
				return null;
			return value;
		}
	}
}
