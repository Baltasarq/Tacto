
using System;
using System.Xml;

namespace Tacto.Core {
	public class AgendaSystem {
		public const string EtqAgenda = "agenda";

		public AgendaSystem()
		{
			this.personsList = new PersonsList();
			this.categoryList = new CategoryList();
		}

		/// <summary>
		/// Determines whether the agenda was modified.
		/// It also allows to be manually set.
		/// </summary>
		/// <value><c>true</c> if modified; otherwise, <c>false</c>.</value>
		public bool Modified {
			get {
				return ( this.CategoryList.Modified || this.PersonsList.Modified );
			}
			set {
				this.PersonsList.Modified = value;
				this.CategoryList.Modified = value;
			}
		}

		/// <summary>
		/// Gets the persons list.
		/// </summary>
		/// <value>The persons list, as a PersonsList.</value>
		public PersonsList PersonsList {
			get {
				return this.personsList;
			}
		}

		/// <summary>
		/// Gets the category list.
		/// </summary>
		/// <value>The category list, as a CategoryList.</value>
		public CategoryList CategoryList {
			get {
				return this.categoryList;
			}
		}

		/// <summary>
		/// Gets or sets the name of the file associated to this agenda.
		/// </summary>
		/// <value>The name of the file, as a string.</value>
		public string FileName {
			get { return this.fileName; }
			set { this.fileName = value.Trim(); }
		}

		/// <summary>
		/// Determines whether this instance has file name set.
		/// </summary>
		/// <returns><c>true</c> if this instance has file name set; otherwise, <c>false</c>.</returns>
		public bool IsFileNameSet()
		{
			return ( FileName.Length > 0 );
		}

		/// <summary>
		/// Save this agenda, with the associated file name.
		/// </summary>
		public void Save()
		{
			this.Save( this.FileName );
		}

		/// <summary>
		/// Save the agenda, using a specified file name.
		/// </summary>
		/// <param name="fileName">the file name, as a string.</param>
		public void Save(string fileName)
		{
			if ( this.Modified ) {
				// Set name
				this.FileName = fileName;
				
				// Sort
				this.Sort();
				
				// Prepare file
				XmlTextWriter textWriter =
	 				new XmlTextWriter( FileName, System.Text.Encoding.UTF8 )
				;
				
				// Write standard header
				textWriter.WriteStartDocument();
				
				// Write categories & persons
				textWriter.WriteStartElement( EtqAgenda );
				CategoryList.SaveCategories( textWriter );
				PersonsList.SavePersons( textWriter );
				textWriter.WriteEndElement();
				
				// Write the end of the document
				textWriter.WriteEndDocument();
				textWriter.Close();
			}
			
			return;
		}

		/// <summary>
		/// Sorts the persons in this agenda.
		/// @see PersonsList.Sort
		/// </summary>
		public void Sort()
		{
			PersonsList.Sort();
		}

		/// <summary>
		/// Load the specified agenda, using a given file name.
		/// </summary>
		/// <param name="fileName">A file name, as a string.</param>
		public static AgendaSystem Load(string fileName)
		{
			AgendaSystem toret = new AgendaSystem();
			XmlDocument docXml = new XmlDocument();
			
			// Open document
			toret.FileName = fileName;
			docXml.Load( fileName );
			XmlNode mainNode = docXml.DocumentElement;
			
			if ( mainNode.Name == EtqAgenda ) {
				foreach(XmlNode node in mainNode.ChildNodes ) {
					if ( node.Name.ToLower() == PersonsList.EtqPersons ) {
						Tacto.Core.PersonsList.LoadPersons( node, toret.PersonsList );
					}
					else
					if ( node.Name.ToLower() == Tacto.Core.CategoryList.EtqCategories ) {
						Tacto.Core.CategoryList.LoadCategories( node, toret.CategoryList );
					}
				}
			}
			else throw new XmlException( EtqAgenda + " expected" );
			
			toret.Modified = false;
			return toret;
		}

		private string fileName = "";
		private PersonsList personsList = null;
		private CategoryList categoryList = null;
	}
}
