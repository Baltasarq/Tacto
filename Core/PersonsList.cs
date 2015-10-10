
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using Tacto.Core;

namespace Tacto.Core
{
	public class PersonsList : IEnumerable<Person>
	{
		public const string EtqPersons  = "persons";
		public const string EtqPerson   = "person";
		public const string EtqName     = "name";
		public const string EtqSurname  = "surname";
		public const string EtqEmail    = "email";
		public const string EtqEmail2   = "email2";
		public const string EtqWeb   	= "web";
		public const string EtqAddress  = "address";
		public const string EtqWPhone   = "work_phone";
		public const string EtqHPhone   = "home_phone";
		public const string EtqMPhone   = "mobile_phone";
		public const string EtqId       = "id";
		public const string EtqComments = "comments";
		public const string EtqCategory	= "category";
		
		public PersonsList()
		{
			listPersons = new List<Person>();
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Tacto.Core.PersonsList"/> is modified.
		/// </summary>
		/// <value><c>true</c> if modified; otherwise, <c>false</c>.</value>
		public bool Modified {
			get; set;
		}
				
		/// <summary>
		/// Sort this list of persons, alphabetically.
		/// </summary>
		public void Sort()
		{
			int i;
			var pl = new List<Person>();
			
			foreach(var p in this) {
				var surname = p.Surname;
				
				// Locate slot
				for(i = 0; i < pl.Count; ++i) {
					if ( String.Compare( surname, pl[ i ].Surname, true ) < 0 ) {
						break;
					}
				}
				
				// Insert it
				pl.Insert( i, p );
			}
			
			// Swap lists 
			this.listPersons = pl;
			this.Modified = true;
			return;
		}

		/// <summary>
		/// Adds a new Person to the list.
		/// </summary>
		/// <param name="p">the new Person object.</param>
		public void Insert(Person p)
		{
			int pos = 0;
			String surname = p.Surname;
			int numPersons = this.Size();
			
			while ( pos < numPersons
			    && String.Compare( surname, this.Get( pos ).Surname, true ) > 0 )
			{
				++pos;
			}
			
			if ( pos >= numPersons )
					this.listPersons.Add( p );
			else 	this.listPersons.Insert( pos, p );
			
			this.Modified = true;
			return;
		}

		/// <summary>
		/// Locates a given Person object.
		/// </summary>
		/// <param name="p">A Person object.</param>
		public int Find(Person p)
		{
			return this.listPersons.IndexOf( p );
		}

		/// <summary>
		/// Number of persons.
		/// </summary>
		public int Size()
		{
			return listPersons.Count;
		}

		/// <summary>
		/// Remove the Person object at the specified pos.
		/// </summary>
		/// <param name="pos">The position, as an int.</param>
		public void Remove(int pos)
		{
			if ( pos < Size()
			  && pos >= 0 )
			{
				listPersons.RemoveAt( pos );
				this.Modified = true;
			}
			
			return;
		}

		/// <summary>
		/// Get the Person object at the specified pos.
		/// </summary>
		/// <param name="pos">The position, as an int.</param>
		public Person Get(int pos)
		{
			if ( pos < Size()
			  && pos >= 0 )
			{
				return listPersons[ pos ];
			}
			else throw new ApplicationException( "person Get out of bounds" );
		}

		/// <summary>
		/// Loads a list of persons with the specified file name.
		/// </summary>
		/// <param name="fileName">The file name, as a string.</param>
		public static PersonsList Load(String fileName)
		{
			PersonsList toret = new PersonsList();
			XmlDocument docXml = new XmlDocument();
			
			// Open document
 			docXml.Load( fileName );
			XmlNode mainNode = docXml.DocumentElement;
			
			LoadPersons( mainNode, toret );
			
			toret.Modified = false;
			return toret;
		}

		/// <summary>
		/// Imports from CSV.
		/// @see CsvManager
		/// </summary>
		/// <returns>The new PersonList imported from CSV</returns>
		/// <param name="fileName">The file name of the CSV, as a string.</param>
		public static PersonsList ImportCsv(string fileName)
		{
			return new CsvManager( fileName ).Import();
		}

		/// <summary>
		/// Imports from VCard.
		/// @see VcfManager
		/// </summary>
		/// <returns>The new PersonList imported from VCard</returns>
		/// <param name="fileName">The file name of the VCard, as a string.</param>
		public static PersonsList ImportVcf(string fileName)
		{
			return new VcfManager( fileName ).Import();
		}

		/// <summary>
		/// Loads the persons under a given XmlNode.
		/// </summary>
		/// <param name="mainNode">The main XML node, as a XmlNode.</param>
		/// <param name="toret">The PersonsList with the extracted info.</param>
		public static void LoadPersons(XmlNode mainNode, PersonsList toret)
		{
			// Run over all persons
			if ( mainNode.Name.ToLower() == EtqPersons ) {
				foreach(XmlNode node in mainNode.ChildNodes) {
					string surname  = "";
					string name     = "";
					string address  = "";
					string email    = "";
					string email2   = "";
					string web      = "";
					string wphone   = "";
					string hphone   = "";
					string mphone   = "";
					string id       = "";
					string comments = "";
					string[] categories = null;

					if ( node.Name.ToLower() == EtqPerson ) {
						// Load name and surname
						XmlNode nameNode = node.Attributes.GetNamedItem( EtqName );
						
						if ( nameNode != null ) {
							name = nameNode.InnerText;
						} else {
							throw new XmlException( EtqName + " attribute should appear in node" );
						}
						
						nameNode = node.Attributes.GetNamedItem( EtqSurname );
						
						if ( nameNode != null ) {
							surname = nameNode.InnerText;
						} else {
							throw new XmlException( EtqSurname + " attribute should appear in node" );
						}
						
						// Load the remaining data
						foreach(XmlNode childNode in node.ChildNodes) {
							if ( childNode.Name.ToLower() == EtqAddress ) {
								address = childNode.InnerText;
							}
							else
							if ( childNode.Name.ToLower() == EtqEmail ) {
								email = childNode.InnerText;
							}
							else
							if ( childNode.Name.ToLower() == EtqEmail2 ) {
								email2 = childNode.InnerText;
							}
							else
							if ( childNode.Name.ToLower() == EtqWeb ) {
								web = childNode.InnerText;
							}
							else
							if ( childNode.Name.ToLower() == EtqWPhone ) {
								wphone = childNode.InnerText;
							}
							else
							if ( childNode.Name.ToLower() == EtqHPhone ) {
								hphone = childNode.InnerText;
							}
							else
							if ( childNode.Name.ToLower() == EtqMPhone ) {
								mphone = childNode.InnerText;
							}
							else
							if ( childNode.Name.ToLower() == EtqId ) {
								id = childNode.InnerText;
							}
							else
							if ( childNode.Name.ToLower() == EtqComments ) {
								comments = childNode.InnerText;
							}
							else
							if ( childNode.Name.ToLower() == EtqCategory ) {
								categories = childNode.InnerText.Split( ';' );
							}
						}
						
						// Prepare categories
						var cats = new List<Category>();
						for(int i = 0; i < categories.Length; ++i) {
							var category = new Category( categories[ i ] );
							if ( !CategoryList.IsCategoryAll( category ) ) {
								cats.Add( category );
							}
						}
						                          
						// Add person
						var p = new Person();
						p.Surname = surname;
						p.Name = name;
						p.Email = email;
						p.WorkPhone = wphone;
						p.HomePhone = hphone;
						p.MobilePhone = mphone;
						p.Address = address;
						p.Email2 = email2;
						p.Web = web;
						p.Id = id;
						p.Comments = comments;
						p.Categories = cats.ToArray();
						toret.Insert( p );
					}
				}
			} else {
				throw new XmlException( "Invalid main label, expected '" + EtqPersons + "'" );
			}
			
			return;
		}

		/// <summary>
		/// Saves the PersonsList to the specified fileName, as XML.
		/// </summary>
		/// <param name="fileName">A file name, as a string.</param>
		public void Save(String fileName)
		{
			// Check we can write the destination file
			if ( !File.Exists( fileName ) ) {
				File.Create( fileName );
				File.Delete( fileName );
			}

			// Write to a temporary file
			string tempFile = Path.GetTempFileName();

			XmlTextWriter textWriter =
				new XmlTextWriter( tempFile, System.Text.Encoding.UTF8 )
			;
			
			// Write standard header and main node
			textWriter.WriteStartDocument();
			
			SavePersons( textWriter );
			
			// Write the end of the document
			textWriter.WriteEndDocument();
			textWriter.Close();
			
			// Finish
			File.Delete( fileName );
			File.Move( tempFile, fileName );
			this.Modified = false;
			return;
		}

		/// <summary>
		/// Saves the persons in the PersonsList, using a given XmlTextWriter.
		/// </summary>
		/// <param name="textWriter">The XML text writer, as a XmlTextWriter.</param>
		public void SavePersons(XmlTextWriter textWriter)
		{
			// Start main node
			string categories;
			textWriter.WriteStartElement( EtqPersons );
			
			foreach(var person in this) {
				// Write each person
				textWriter.WriteStartElement( EtqPerson );
				
				textWriter.WriteStartAttribute( EtqSurname );
				textWriter.WriteString( person.Surname );
				textWriter.WriteEndAttribute();
				
				textWriter.WriteStartAttribute( EtqName );
				textWriter.WriteString( person.Name );
				textWriter.WriteEndAttribute();
				
				textWriter.WriteStartElement( EtqEmail );
				textWriter.WriteString( person.Email );
				textWriter.WriteEndElement();
				
				textWriter.WriteStartElement( EtqWPhone );
				textWriter.WriteString( person.WorkPhone );
				textWriter.WriteEndElement();
				
				if ( person.HomePhone.Length > 0 ) {
					textWriter.WriteStartElement( EtqHPhone );
					textWriter.WriteString( person.HomePhone );
					textWriter.WriteEndElement();
				}
				
				if ( person.MobilePhone.Length > 0 ) {
					textWriter.WriteStartElement( EtqMPhone );
					textWriter.WriteString( person.MobilePhone );
					textWriter.WriteEndElement();
				}
				
				if ( person.Email2.Length > 0 ) {
					textWriter.WriteStartElement( EtqEmail2 );
					textWriter.WriteString( person.Email2 );
					textWriter.WriteEndElement();
				}
				
				if ( person.Web.Length > 0 ) {
					textWriter.WriteStartElement( EtqWeb );
					textWriter.WriteString( person.Web );
					textWriter.WriteEndElement();
				}

				if ( person.Address.Length > 0 ) {
					textWriter.WriteStartElement( EtqAddress );
					textWriter.WriteString( person.Address );
					textWriter.WriteEndElement();
				}

				if ( person.Id.Length > 0 ) {
					textWriter.WriteStartElement( EtqId );
					textWriter.WriteString( person.Id );
					textWriter.WriteEndElement();
				}

				if ( person.Comments.Length > 0 ) {
					textWriter.WriteStartElement( EtqComments );
					textWriter.WriteString( person.Comments );
					textWriter.WriteEndElement();
				}
				
				// Create categories
				textWriter.WriteStartElement( EtqCategory );
				categories = "";
				foreach(var category in person.Categories) {
					categories += category.Name + ';';
				}
				
				// Remove last semicolon and write them
				categories = categories.Substring( 0, categories.Length -1 );
				textWriter.WriteString( categories );
				textWriter.WriteEndElement();
				
				textWriter.WriteEndElement();	
			}
			
			// Close main node and exit
			textWriter.WriteEndElement();
			return;
		}

		/// <summary>
		/// Converts all the Person objects in the list to CSV
		/// </summary>
		/// <returns>The csv.</returns>
		public string ToCsv()
		{
			return ToCsv( CsvManager.CommaDelimiter );
		}

		/// <summary>
		/// Converts all persons in the list to CSV, given a delimiter.
		/// </summary>
		/// <returns>The CSV contents, as string.</returns>
		/// <param name="delimiter">The delimiter, as char.</param>
		public string ToCsv(char delimiter)
		{
			string toret = "";
			
			foreach(var p in this) {
				toret += p.ToCsv( delimiter ) + '\n';
			}
			
			return toret;
		}

		/// <summary>
		/// Converts all the Person objects in the PersonsList to HTML.
		/// </summary>
		/// <returns>The HTML contents, as string.</returns>
		public string ToHtml()
		{
			string toret = "<table border=0>";
			
			foreach(var p in this) {
				toret += p.ToHtml() + '\n';
			}
			
			return ( toret + "</table>" );				
		}

		/// <summary>
		/// Converts all persons in the list to VCard, given a delimiter.
		/// </summary>
		/// <returns>The VCard contents, as string.</returns>
		public string ToVCard()
		{
			string toret = "";
			
			foreach(var p in this) {
				toret += p.ToVCard() + '\n';
			}
			
			return toret;
		}

		/// <summary>
		/// Looks for a given text in all objects Person in the list.
		/// </summary>
		/// <returns>The position of the found object, as int. -1 if not found.</returns>
		/// <param name="txt">The text to look for, as string.</param>
		public int LookFor(string txt)
		{
			return LookFor( txt, 0 );
		}

		/// <summary>
		/// Looks for a given text in all objects
		/// Person in the list, from a given position.
		/// </summary>
		/// <returns>The position of the found object, as int. -1 if not found.</returns>
		/// <param name="txt">The text to look for, as string.</param>
		/// /// <param name="pos">The position to start searching, as int.</param>
		public int LookFor(string txt, int pos)
		{
			int toret = -1;
			txt = txt.Trim().ToLower();
			
			if ( txt.Length > 0 ) {
				for(; pos < this.Size(); ++pos)
				{
					if ( this.Get( pos ).LookFor( txt ) ) {
						toret = pos;
						break;
					}
				}
			}
			
			return toret;
		}

		IEnumerator<Person> IEnumerable<Person>.GetEnumerator()
		{
			foreach(var p in listPersons) {
				yield return p;
			}
		}
		
		IEnumerator IEnumerable.GetEnumerator()
		{
			foreach(var p in listPersons) {
				yield return p;
			}
		}
		
		private List<Person> listPersons;
	}
}
