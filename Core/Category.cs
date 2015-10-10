
using System;

namespace Tacto.Core {
	public class Category {
		public const string EtqCategoryAll = "all";
		
		public Category(String name)
		{
			this.name = Decode( name );
		}
		
		public static string Decode(string name)
		{
			name = name.Trim().ToLower();
			
			if ( name.Length > 0 ) {
				name = name.Replace( '_', ' ' );
				name = Char.ToUpper( name[ 0 ] ) + name.Substring( 1 );
			} else {
				name = EtqCategoryAll;
			}
			
			return name;
		}
		
		public static string Encode(string name)
		{
			name = name.Trim().ToLower();
			name = name.Replace( ' ', '_' );
			
			return name;
		}
		
		private string name;
		public string Name {
			get { return name; }
		}
	}
}
