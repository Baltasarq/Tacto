using System;
using System.IO;

namespace Tacto.Core {
	public class HtmlManager : FileFormatManager {
		public HtmlManager(string fileName) : base( fileName )
			{}
		
		public override void Export(PersonsList pl)
		{
			using(var file = new StreamWriter( FileName ) ) {
				file.WriteLine( "<html>\n<head>\n" );
				file.WriteLine( "<meta http-equiv=\"CONTENT-TYPE\" content=\"text/html; charset=UTF-8\">" );
				file.WriteLine( "\n<title>Tacto Conversion</title>\n</head>\n<body>" );
				
				file.WriteLine( pl.ToHtml() );
				
				file.WriteLine( "</body></html>" );
			}
		}
		
		public override PersonsList Import()
		{
			throw new ApplicationException( "Sorry, cannot import from HTML" ); 
		}
	}
}

