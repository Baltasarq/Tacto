using System;

namespace Tacto.Core {
	public abstract class FileFormatManager {
		private string fileName;
		
		public string FileName {
			get { return this.fileName; }
		}
		
		public FileFormatManager(string fileName)
		{
			this.fileName = fileName;
			
		}
		
		abstract public void Export(PersonsList pl);
		abstract public PersonsList Import();
		
		public static FileFormatManager CreateManager(Person.Format fmt, string fileName)
		{
			FileFormatManager toret = null;
			
			if ( fmt == Person.Format.CSV ) {
				// CSV
				toret = new CsvManager( fileName );
			}
			else
			if ( fmt == Person.Format.HTML ) {
				// HTML
				toret = new HtmlManager( fileName );
			}
			else
			if ( fmt == Person.Format.VCF ) {
				// VCard
				toret = new VcfManager( fileName );
			}
			
			if ( toret == null ) {
				throw new ApplicationException( "Internal: format not found" );
			}
			
			return toret;
		}
	}
}

