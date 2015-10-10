
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace Tacto.Core {
	public class CategoryList : IEnumerable<Category> {
		public const string EtqCategories = "categories";
		public const string EtqCategory   = "category";
		public const string EtqName       = "name";
		
		public CategoryList() {
			categoryList = new List<Category>();
			
			categoryList.Add( new Category( Category.EtqCategoryAll ) );
		}
		
		public bool Modified {
			get; set;
		}
		
		public static bool IsCategoryAll(Category c)
		{
			return ( Category.Encode( c.Name ).CompareTo( Category.Encode( Category.EtqCategoryAll ) ) == 0 );
		}
		
		public Category[] ToArray()
		{
			return categoryList.ToArray();
		}
		
		public Category LookFor(string categoryName)
		{
			Category toret = null;
			categoryName = Category.Encode( categoryName );
			
			foreach( var category in this) {
				if ( Category.Encode( category.Name ) == categoryName ) {
					toret = category;
				}
			}
			
			return toret;
		}		
		
		public void Append(Category c)
		{
			if ( LookFor( c.Name ) == null ) {
				categoryList.Add( c );
				this.Modified = true;
			} else {
				throw new ApplicationException( c.Name + ": already existing category"  );
			}
			
			return;
		}
		
		public void AppendRange(Category[] c)
		{
			foreach(var category in c) {
				if ( !CategoryList.IsCategoryAll( category ) ) {
					this.Append( category );
					this.Modified = true;
				}
			}
			
			return;
		}
		
		public void Clear()
		{
			categoryList.Clear();
		}
		
		public int Size()
		{
			return categoryList.Count;
		}
		
		public void Remove(int pos)
		{
			if ( pos < Size()
			  && pos >= 0 )
			{
				if ( !IsCategoryAll( categoryList[ pos ] ) ) {
					categoryList.RemoveAt( pos );
					this.Modified = true;
				} else throw new ApplicationException( "'" + categoryList[ pos ].Name + "' cannot be modified" );
			}
			else throw new ApplicationException( "internal: invalid position" );
			
			return;
		}
		
		public Category Get(int pos)
		{
			if ( pos < Size()
			  && pos >= 0 )
			{
				return categoryList[ pos ];
			}
			else throw new ApplicationException( "internal: category out of bounds" );
		}
		
		public void Modify(int pos, string categoryName)
		{
			if ( pos < Size()
			  && pos >= 0 )
			{
				if ( !IsCategoryAll( categoryList[ pos ] ) ) {
					categoryList[ pos ] = new Category( categoryName );
					this.Modified = true;
				} else {
					throw new ApplicationException( "'" + Category.EtqCategoryAll + "' cannot be modified" );
				}
			}
			else throw new ApplicationException( "internal: category out of bounds" );
			
			return;
		}
		
		IEnumerator<Category> IEnumerable<Category>.GetEnumerator()
		{
			foreach(var c in categoryList) {
				yield return c;
			}
		}
		
		IEnumerator IEnumerable.GetEnumerator()
		{
			foreach(var c in categoryList) {
				yield return c;
			}
		}
		
		public static CategoryList Load(String fileName)
		{
			CategoryList toret = new CategoryList();
			XmlDocument docXml = new XmlDocument();
			
			// Open document
			toret.Clear();
 			docXml.Load( fileName );
			XmlNode mainNode = docXml.DocumentElement;
			
			LoadCategories( mainNode, toret );
			
			toret.Modified = false;
			return toret;
		}
		
		public static void LoadCategories(XmlNode mainNode, CategoryList toret)
		{
			// Run over all categories
			if ( mainNode.Name.ToLower() == EtqCategories ) {
				toret.Clear();
				
				foreach(XmlNode node in mainNode.ChildNodes) {
					if ( node.Name.ToLower() == EtqCategory ) {
						var attrName = node.Attributes.GetNamedItem( EtqName );
						
						if ( attrName != null ) {
							toret.Append( new Category( Category.Decode( attrName.InnerText ) ) );
						}
					}
				}
				
				if ( toret.LookFor( Category.EtqCategoryAll ) == null ) {
					throw new XmlException( "Expected to find category: '" + Category.EtqCategoryAll + "'" );
				}
			} else throw new XmlException( "Expected: '" + EtqCategories + "'" );
		}
		
		public void Save(String fileName)
		{
			XmlTextWriter textWriter =
 				new XmlTextWriter( fileName, System.Text.Encoding.UTF8 )
			;
			
			// Write standard header and main node
			textWriter.WriteStartDocument();
			
			this.SaveCategories( textWriter );
			
			// Write the end of the document
			textWriter.WriteEndDocument();
			textWriter.Close();
			this.Modified = false;
		}
		
		public void SaveCategories(XmlTextWriter textWriter)
		{
			// Start main node
			textWriter.WriteStartElement( EtqCategories );
			
			foreach(var category in this) {
				// Write each category
				textWriter.WriteStartElement( EtqCategory );
				
				textWriter.WriteStartAttribute( EtqName );
				textWriter.WriteString( Category.Encode( category.Name ) );
				textWriter.WriteEndAttribute();
				
				textWriter.WriteEndElement();
			}
			
			// Close main node and exit
			textWriter.WriteEndElement();
		}
		
		private List<Category> categoryList = null;
	}
}
