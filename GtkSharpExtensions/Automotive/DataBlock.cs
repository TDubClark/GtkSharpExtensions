using System;

namespace GtkSharpExtensions.Automotive
{
	/// <summary>
	/// A block of Automotive data.
	/// </summary>
	public class DataBlock
	{
		/// <summary>
		/// Gets or sets the collection of Automobiles.
		/// </summary>
		/// <value>The cars.</value>
		public AutomobileCollection Cars {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the collection of Automobile Models.
		/// </summary>
		/// <value>The models.</value>
		public AutoModelCollection Models {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the collection of Auto Manufacturers.
		/// </summary>
		/// <value>The makes.</value>
		public ManufacturerCollection Makes {
			get;
			set;
		}

		public DataBlock ()
		{
			Makes = new ManufacturerCollection ();
			Models = new AutoModelCollection ();
			Cars = new AutomobileCollection ();
		}

		/// <summary>
		/// Fills the collections with demonstration data.
		/// </summary>
		public void FillWithData ()
		{
			var mkFord = new Manufacturer {
				CompanyName = "Ford Motor Company",
				CompanyInitials = "F"
			};
			var mkGM = new Manufacturer {
				CompanyName = "General Motors",
				CompanyInitials = "GM"
			};
			var mkToyota = new Manufacturer {
				CompanyName = "Toyota",
				CompanyInitials = "T"
			};
			Makes.AddRange (new []{ mkFord, mkGM, mkToyota });

			var mdlTaurus = new AutoModel {
				ModelName = "Taurus",
				ModelType = "Sedan",
				Make = mkFord
			};
			var mdlEscort = new AutoModel {
				ModelName = "Escort",
				ModelType = "Coupe",
				Make = mkFord
			};
			var mdlFocus = new AutoModel {
				ModelName = "Focus",
				ModelType = "Coupe",
				Make = mkFord
			};
			Models.AddRange (new []{ mdlTaurus, mdlEscort, mdlFocus });

			Cars.AddRange (new [] {
				new Automobile{
					Make = mkFord,
					Model = mdlTaurus,
					Year = 1999
				},
				new Automobile{
					Make = mkFord,
					Model = mdlTaurus,
					Year = 2000
				},
				new Automobile{
					Make = mkFord,
					Model = mdlEscort,
					Year = 2000
				},
				new Automobile{
					Make = mkFord,
					Model = mdlEscort,
					Year = 2001
				},
				new Automobile{
					Make = mkFord,
					Model = mdlFocus,
					Year = 2005
				},
				new Automobile{
					Make = mkFord,
					Model = mdlFocus,
					Year = 2006
				},
				new Automobile{
					Make = mkFord,
					Model = mdlFocus,
					Year = 2007
				},
				new Automobile{
					Make = mkFord,
					Model = mdlFocus,
					Year = 2008
				}
			});
		}
	}
}

