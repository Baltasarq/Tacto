
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Tacto.Core
{
	public class Person
	{
		public const string WebProtocolPrefix = "http://";
		public enum Format { CSV, HTML, VCF };
		public ReadOnlyCollection<string> TrimAddressPrefixes = 
			new ReadOnlyCollection<string>(
				new string[] { "c\\", "c/", "avenida", "avda." }

			);

		public static readonly ReadOnlyCollection<string> FormatNames =
			new ReadOnlyCollection<string>(
				new string[]{ "CSV", "HTML", "VCF" }
			);
		
		public Person()
			:this( "Doe", "John", "john@doe.com", "0", null )
		{
		}
		
		public Person(string surname, string name, string email, string phone)
			:this( surname, name, email, phone, null )
		{
		}
			
		public Person(string surname, string name, string email, string phone, Category[] categories)
		{
			this.Surname = surname;
			this.Name = name;
			this.Email = email;
			this.WorkPhone = phone;
			this.HomePhone = MobilePhone = "";
			this.Address = "";
			this.Email2 = "";
			this.Id = "";
			this.Comments = "";
			this.Web = "";
			
			if ( categories != null ) {
				this.Categories = categories;
			}
			
			if ( !this.IsEnoughData() ) {
				throw new ApplicationException( "Person built without enough data" );
			}
		}
		
		public string Address {
			get {
				return this.address;
			}
			set {
				this.address = value.Trim();

				foreach(string prefix in TrimAddressPrefixes) {
					if ( this.address.ToLower().StartsWith( prefix ) ) {
						this.address = this.address.Substring( prefix.Length ).TrimStart();
					}
				}
			}
		}
		
		public string Email2 {
			get {
				return this.email2;
			}
			set {
				this.email2 = FormatEmail( value );
			}
		}

		public string Name {
			get {
				return name;
			}
			set {
				name = NormalizeText( value );
			}
		}

		public string Surname {
			get {
				return this.surname;
			}
			set {
				this.surname = NormalizeText( value );
			}
		}
		
		public string AnyEmail {
			get {
				if ( this.Email.Length == 0 )
						return this.Email2;
				else 	return this.Email;
			}
		}

		public Tacto.Core.Category[] Categories {
			get { return this.categories.ToArray(); }
			set { this.AppendCategories( value ); }
		}
		
		protected void AppendCategories(Category[] categories)
		{
			// Eliminate possible spurious categories
			List<Category> l = new List<Category>( categories );
			l.RemoveAll( x => x == null );

			this.categories.AppendRange( l.ToArray() );
		}

		/// <summary>
		/// Gets or sets the identifier.
		/// This can be a DNI or Passport number.
		/// </summary>
		/// <value>The identifier.</value>
		public string Id {
			get {
				return id;
			}
			set {
				this.id = value.Trim().ToUpper();
			}
		}

		public string Comments {
			get {
				return comments;
			}
			set {
				this.comments = value.Trim();
			}
		}
		
		public string Email {
			get {
				return this.email;
			}
			set {
				this.email = FormatEmail( value );
			}
		}

		public string Web {
			get {
				return this.web;
			}
			set {
				this.web = FormatWeb( value );
			}
		}
		
		public string MobilePhone {
			get {
				return this.mobilePhone;
			}
			set {
				this.mobilePhone = FormatPhoneNumber( value );
			}
		}
		
		public string HomePhone {
			get {
				return this.homePhone;
			}
			set {
				this.homePhone = FormatPhoneNumber( value );
			}
		}

		public string WorkPhone {
			get {
				return this.workPhone;
			}
			set {
				this.workPhone = FormatPhoneNumber( value );
			}
		}

		public string AnyPhone {
			get {
				if ( this.MobilePhone.Length == 0 ) {
					if ( this.HomePhone.Length == 0 ) {
						return this.WorkPhone;
					} else {
						return this.HomePhone;
					}
				} else {
					return MobilePhone;
				}
			}
		}

		/// <summary>
		/// Clone this instance.
		/// </summary>
		public Person Clone()
		{
			var toret = new Person();

			toret.Id          = this.Id;
			toret.Comments    = this.Comments;
			toret.Surname     = this.Surname;
			toret.Name        = this.Name;
			toret.Email       = this.Email;
			toret.Email2      = this.Email2;
			toret.Web         = this.Web;
			toret.WorkPhone   = this.WorkPhone;
			toret.HomePhone   = this.HomePhone;
			toret.MobilePhone = this.MobilePhone;
			toret.Categories  = this.Categories;
			toret.Address     = this.Address;
			
			return toret;
		}

		/// <summary>
		/// Determines whether this instance has enough data.
		/// </summary>
		/// <returns><c>true</c> if this instance has enough data; otherwise, <c>false</c>.</returns>
		public bool IsEnoughData()
		{
			return (
				this.Surname.Length > 0
				&& this.Name.Length > 0
				&& this.AnyEmail.Length > 0
				&& this.AnyPhone.Length > 0
			);
		}

		/// <summary>
		/// Determines whether this instance has the specified category.
		/// </summary>
		/// <returns><c>true</c> if this instance has the specified category; otherwise, <c>false</c>.</returns>
		/// <param name="category">The category, as a Category object.</param>
		public bool HasCategory(Category category)
		{
			return ( this.categories.LookFor( category.Name ) != null );
		}

		/// <summary>
		/// Removes a given category.
		/// </summary>
		/// <param name="category">The category to remove, as a Category object.</param>
		public void RemoveCategory(Category category)
		{
			if ( category == null
			    || category.Name.Trim().Length == 0 )
			{
				throw new ApplicationException( "internal: invalid category name" );
			}

			RemoveCategory( category.Name );
		}

		/// <summary>
		/// Removes a given category.
		/// </summary>
		/// <param name="categoryText">A category, as string.</param>
		public void RemoveCategory(string categoryText)
		{
			int i = 0;
			categoryText = Category.Encode( categoryText );
			int numCategories = categories.Size();
			
			for(; i < numCategories; ++i) {
				if ( Category.Encode( categories.Get( i ).Name ) == categoryText ) {
					categories.Remove( i );
					break;
				}
			}
			
			if ( i >= numCategories ) {
				throw new ApplicationException( "category '" + categoryText + "' does not exist" );
			}
			
			return;
		}

		/// <summary>
		/// Returns a brief summary <see cref="System.String"/> that represents the current <see cref="Tacto.Core.Person"/>.
		/// </summary>
		/// <returns>A brief summary <see cref="System.String"/> that represents the current <see cref="Tacto.Core.Person"/>.</returns>
		public override string ToString()
		{
			return ( Surname + ", " + Name + "(" + Email + "): " + WorkPhone ); 
		}

		/// <summary>
		/// Converts the Person info to CSV, assuming comma as delimiter.
		/// @see CsvManager.CommaDelimiter
		/// </summary>
		/// <returns>The resulting info in csv format, as string.</returns>
		public string ToCsv()
		{
			return ToCsv( CsvManager.CommaDelimiter );
		}

		/// <summary>
		/// Converts the Person info to CSV
		/// </summary>
		/// <returns>The resulting info in csv format, as string.</returns>
		/// <param name="delimiter">The delimiter, normally "," or ";".</param>
		public string ToCsv(char delimiter)
		{
			return (
				'\"' + this.Surname + '\"'
				+ delimiter
				+ '\"' + this.Name + '\"'
				+ delimiter
				+ '\"' + this.Address + '\"'
				+ delimiter
				+ '\"' + this.Email + '\"'
				+ delimiter
				+ '\"' + this.Email2 + '\"'
				+ delimiter
				+ '\"' + this.Web + '\"'
				+ delimiter
				+ '\"' + this.WorkPhone + '\"'
				+ delimiter
				+ '\"' + this.HomePhone + '\"'
				+ delimiter
				+ '\"' + this.MobilePhone + '\"'
				+ delimiter
				+ '\"' + this.Id + '\"'
				+ delimiter
				+ '\"' + this.Comments + '\"'
			);
		}

		/// <summary>
		/// Converts the info of this Person to HTML.
		/// </summary>
		/// <returns>The resulting HTML, as a string.</returns>
		public string ToHtml()
		{
			return (
				"<tr>"
				+ "<td>" + Surname + "</td>"
				+ "<td>" + Name + "</td>"
				+ "<td>" + Address + "</td>"
				+ "<td>" + Email + "</td>"
				+ "<td>" + Email2 + "</td>"
				+ "<td>" + Web + "</td>"
				+ "<td>" + WorkPhone + "</td>"
				+ "<td>" + HomePhone + "</td>"
				+ "<td>" + MobilePhone + "</td>"
				+ "<td>" + Id + "</td>"
				+ "<td>" + Comments + "</td>"
				+ "</tr>"
			);
		}

		/// <summary>
		/// Cnvts any text for suitability to VCard.
		/// </summary>
		/// <returns>The text converted, as string.</returns>
		/// <param name="field">The text to convert.</param>
		public static string CnvtForSuitabilityToVCard(string field)
		{
			return field.Replace( ";", "\\;" ).Replace( ",", "\\," ).Replace( "\\", "\\\\" );
		}

		/// <summary>
		/// Converts the info of this Person to a VCard
		/// </summary>
		/// <returns>The VCard, as a string.</returns>
		public string ToVCard()
		{
			string escName = CnvtForSuitabilityToVCard( this.Name );
			string escSurame = CnvtForSuitabilityToVCard( this.Surname );
			string escAddress = CnvtForSuitabilityToVCard( this.Address );
			string escWorkPhone = CnvtForSuitabilityToVCard( this.WorkPhone );
			string escMobilePhone = CnvtForSuitabilityToVCard( this.MobilePhone );
			string escHomePhone = CnvtForSuitabilityToVCard( this.HomePhone );
			string escEmail = CnvtForSuitabilityToVCard( this.Email );
			string escEmail2 = CnvtForSuitabilityToVCard( this.Email2 );
			string escWeb = CnvtForSuitabilityToVCard( this.Web );
			
			return (
			        "BEGIN:VCARD\nVERSION:3.0\n"
			        + "N;CHARSET=UTF-8:" + escSurame + ";" + escName + "\n"
			        + "FN;CHARSET=UTF-8:" + escName + " " + escSurame + "\n"
			        + "ADR;TYPE=HOME;CHARSET=UTF-8:" + escAddress + "\n"
			        + "TEL;TYPE=WORK,VOICE;CHARSET=UTF-8:" + escWorkPhone + "\n"
			        + "TEL;TYPE=HOME,VOICE;CHARSET=UTF-8:" + escHomePhone + "\n"
			        + "TEL;TYPE=MOBILE,VOICE, MSG;CHARSET=UTF-8:" + escMobilePhone + "\n"
			        + "EMAIL;TYPE=PREF,INTERNET;CHARSET=UTF-8:" + escEmail + "\n"
			        + "EMAIL;TYPE=INTERNET;CHARSET=UTF-8:" + escEmail2 + "\n"
					+ "URL:" + escWeb + "\n"
			        + "END:VCARD\n"
			);
		}

		/// <summary>
		/// Looks for something in this Person record.
		/// </summary>
		/// <returns><c>true</c>, if it was found, <c>false</c> otherwise.</returns>
		/// <param name="txt">A text to look for.</param>
		public bool LookFor(string txt)
		{
			return this.Name.ToLower().Contains( txt )
					|| this.Surname.ToLower().Contains( txt )
					|| this.Address.ToLower().Contains( txt )
					|| this.Email.ToLower().Contains( txt )
					|| this.Email2.ToLower().Contains( txt )
					|| this.Web.ToLower().Contains( txt )
					|| this.WorkPhone.ToLower().Contains( txt )
					|| this.MobilePhone.ToLower().Contains( txt )
					|| this.HomePhone.ToLower().Contains( txt )
					|| this.Id.ToLower().Contains( txt )
					|| this.Comments.ToLower().Contains( txt )
			;
		}
		
		public void Convert(Format format, string fileName)
		{
			System.IO.StreamWriter file = new System.IO.StreamWriter( fileName );
			
			if ( format == Format.CSV ) {
				file.WriteLine( this.ToCsv() );
			}
			else
			if ( format == Format.HTML ) {
				file.WriteLine( this.ToHtml() );
			}
			else
			if ( format == Format.VCF ) {
				file.WriteLine( this.ToVCard() );
			}

			file.Close();
			return;
		}

		/// <summary>
		/// Formats a phone number.
		/// </summary>
		/// <returns>The new, formatted, phone number, as string.</returns>
		/// <param name="txtPhone">The phone, as a string.</param>
		public static string FormatPhoneNumber(string txtPhone)
		{
			txtPhone = txtPhone.Trim();
			var aux = "";
			var toret = "";

			// Remove all text without digits
			foreach (char ch in txtPhone) {
				if( ch == '+'
				 || char.IsDigit( ch ) )
				{
					aux += ch;
				}
			}

			// Group them by three, starting from the right
			while ( aux.Length > 2 ) {
				toret = aux.Substring( aux.Length - 3, 3 ) + ' ' + toret;
				aux = aux.Remove( aux.Length - 3 );
			}

			// Add the remaining parts
			if ( aux.Length > 0 ) {
				toret = aux + ' ' + toret;
			}

			return toret.Trim();
		}

		/// <summary>
		/// Formats the email.
		/// </summary>
		/// <returns>The new, formatted, email, as a string.</returns>
		/// <param name="txtEmail">The email, as string.</param>
		public static string FormatEmail(string txtEmail)
		{
			return txtEmail.Trim().ToLower();
		}

		/// <summary>
		/// Formats the web.
		/// </summary>
		/// <returns>The new, formatted, web, as a string.</returns>
		/// <param name="txtEmail">The web, as string.</param>
		public static string FormatWeb(string txtWeb)
		{
			txtWeb = txtWeb.Trim().ToLower();

			if ( !txtWeb.StartsWith( WebProtocolPrefix ) ) {
				txtWeb = WebProtocolPrefix + txtWeb;
			}

			return txtWeb;
		}

		/// <summary>
		/// Normalizes any text.
		/// Normalized text has all chars in lowercase, except
		/// the first letter of each word.
		/// </summary>
		/// <returns>The normalized text, as string.</returns>
		/// <param name="txt">Text.</param>
		public static string NormalizeText(string txt)
		{
			txt = txt.Trim().ToLower();

			if ( txt.Length > 0 ) {
				txt = char.ToUpper( txt[ 0 ] ) + txt.Substring( 1 );

				for(int pos = 0; pos < txt.Length; ++pos) {
					if ( txt[ pos ] == ' '
					  || txt[ pos ] == '-' )
					{
						if ( ( pos - 2 ) < txt.Length ) {
							txt = txt.Substring( 0, pos + 1 ) + char.ToUpper( txt[ pos + 1 ] ) + txt.Substring( pos + 2 );
							++pos;
						}
					}
				}
			}

			return txt;
		}
			
		private string name;
		private string surname;
		private string address;
		private string email;
		private string email2;
		private string workPhone;
		private string homePhone;
		private string mobilePhone;
		private string id;
		private string comments;
		private string web;
		
		private CategoryList categories = new CategoryList();
	}
}
