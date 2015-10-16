using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace Tacto.Core {
	public class CsvManager : FileFormatManager {
		public const char CommaDelimiter = ',';
		public const char SemicolonDelimiter = ';';
		public const char TabDelimiter = '\t';
		public static readonly string Delimiters =
			char.ToString( CommaDelimiter ) + char.ToString( SemicolonDelimiter ) + char.ToString( TabDelimiter )
		;
		public const char Quote = '\"';
		
		public CsvManager(string fileName) : base( fileName )
			{}
		
		public override void Export(PersonsList pl)
		{
			Export( pl, CommaDelimiter );
		}
		
		public void Export(PersonsList pl, char delimiter)
		{
			if ( Delimiters.IndexOf( delimiter ) < 0 ) {
				delimiter = CommaDelimiter;
			}
			
			using( var file = new StreamWriter( FileName ) ) {
				file.WriteLine( pl.ToCsv( delimiter ) );
				file.WriteLine();
			}
		}
		
		public override PersonsList Import()
		{
			string[] parts = null;
			PersonsList toret = new PersonsList();
			var file = new StreamReader( FileName );
			
			string line = file.ReadLine();
			while( !file.EndOfStream ) {
				parts = SplitCsvLine( line );
				
				if ( parts.Length >= 4 )
						toret.Insert( new Person( parts[ 0 ], parts[ 1 ], parts[ 2 ], parts[ 3 ] ) );
				else 	throw new ApplicationException( "Bad CSV format" );
				
				line = file.ReadLine();
			}
			
			file.Close();
			
			return toret;
		}
		
		public static string PrepareCsvCell(string cell)
		{
			// Prepare
			string toret = cell.Trim();
			
			if ( toret.Length > 0 ) {
				if ( toret[ 0 ] == '"' ) {
					// Remove double quotes
					toret = toret.Substring( 1 );
					
					if ( toret[ toret.Length -1 ] == '"' ) {
						toret = toret.Substring( 0, toret.Length -1 );
					}
				} else {
					toret = cell;
				}
			}
			
			return toret;
		}
		
		public static string[] SplitCsvLine(string line)
			{
				var row = new List<string>();
				var pos = 0;
				int i = 0;
				bool inQuoted = false;
				char delimiter = '\0';
	
				// Look for cells
				for(; i < line.Length; ++i) {
					// Delimiter found, set delimiter if needed
					if ( !inQuoted
					  && Delimiters.IndexOf( line[ i ] ) > 0
				      && delimiter == '\0' )
					{
						delimiter = line[ i ];
					}
				
					// Delimiter found, add cell
					if ( !inQuoted
					  && line[ i ] == delimiter )
					{
						row.Add( PrepareCsvCell( line.Substring( pos, i - pos ) ) );
						pos = i + 1;
					}
					else
					// Quote found
					if ( line[ i ] == Quote ) {
						inQuoted = !inQuoted;
					}
				}
		
				// Add last column
				if ( pos < line.Length ) {
					row.Add( PrepareCsvCell( line.Substring( pos, line.Length - pos ) ) );
				}
				else
				if ( line[ line.Length -1 ] == CommaDelimiter ) {
					row.Add( "" );
				}
				
				return row.ToArray();
			}
	}
}

