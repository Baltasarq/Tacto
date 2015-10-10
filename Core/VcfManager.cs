using System;
using System.Collections.ObjectModel;
using System.IO;

namespace Tacto.Core {
	public class VcfManager : FileFormatManager {
		public enum State { TopLevel, InCard };
		public static readonly ReadOnlyCollection<char> AttributeSeparators = new ReadOnlyCollection<char>(
			new char[]{ ';', ',' }
		);
		public const char DataSeparator = ':';
		public const string EtqBeginSection = "BEGIN";
		public const string EtqEndSection = "END";
		public const string EtqCardAttribute = "VCARD";
		public const string EtqNameSection = "N";
		public const string EtqEmailSection = "EMAIL";
		public const string EtqPhoneSection = "TEL";
		public const string EtqAddressSection = "ADR";
		public const string EtqPref = "PREF";
		public const string EtqHome = "HOME";
		public const string EtqWork = "WORK";
		public const string EtqMobile = "MOBILE";
		
		private State status;
		
		public State Status {
			get { return this.status; }
			set { status = value; }
		}

		public VcfManager(string fileName) : base( fileName )
			{}
		
		public override void Export(PersonsList pl)
		{
			using(var file = new StreamWriter( FileName ) ) {
				file.WriteLine( pl.ToVCard() );
				file.WriteLine();
			}
		}
		
		private void SplitVcfLine(string line, out string section, out string[] attributes, out string data)
		{
			section = data = "";
			attributes = new string[ 0 ];
			
			// Find data and section
			string[] mainParts = line.Split( DataSeparator );
			
			// Split section data
			if ( mainParts.Length == 2 ) {
				data = mainParts[ 1 ].Trim();
				section = mainParts[ 0 ].Trim().ToUpper();
				char[] attrSeparators = new char[AttributeSeparators.Count];
				AttributeSeparators.CopyTo( attrSeparators, 0 );
				string[] attrs = section.Split( attrSeparators );
				
				if ( attrs.Length == 1 ) {
					section = attrs[ 0 ].Trim();
				}
				else
				if ( attrs.Length >= 2 ) {
					attributes = new string[ attrs.Length - 1 ];
					section = attrs[ 0 ].Trim();
					for(int i = 1; i < attrs.Length; ++i) {
						attributes[ i -1 ] = attrs[ i ].Trim();
					}
				}
			} else throw new Exception( "Malformed section" );
			
			return;
		}
		
		public static bool AttributesContain(string[] attributes, string key)
		{
			bool toret = false;
			
			foreach(var attr in attributes) {
				if ( attr.IndexOf( key ) > -1 ) {
					toret = true;
					break;
				}
			}
			        
			return toret;
		}
		
		private void FillCard(Person p, string section, string[] attributes, string data)
		{
			if ( section == EtqNameSection
			  && data.Length > 0 )
			{
				string[] nameParts = data.Split( ';' );
				
				if ( nameParts.Length < 2 ) {
					throw new ApplicationException( "Malformed name" );
				}
				
				p.Name = nameParts[ 1 ];
				p.Surname = nameParts[ 0 ];
			}
			else
			if ( section == EtqEmailSection
			  && data.Length > 0
			  && AttributesContain( attributes, EtqPref ) )
			{
				p.Email = data;
			}
			else
			if ( section == EtqEmailSection
			  && data.Length > 0 )
			{
				p.Email2 = data;
			}
			else
			if ( section == EtqPhoneSection
			  && data.Length > 0
			  && AttributesContain( attributes, EtqWork ) )
			{
				p.WorkPhone = data;
			}
			else
			if ( section == EtqPhoneSection
			  && data.Length > 0
			  && AttributesContain( attributes, EtqMobile ) )
			{
				p.MobilePhone = data;
			}
			else
			if ( section == EtqPhoneSection
			  && data.Length > 0
			  && AttributesContain( attributes, EtqHome ) )
			{
				p.HomePhone = data;
			}
			else
			if ( section == EtqAddressSection
			  && data.Length > 0 )
			{
				p.Address = data;
			}
		}
		
		public override PersonsList Import()
		{
			string section;
			string[] attributes;
			string data;
			string line;
			PersonsList toret = new PersonsList();
			Person p = null;
			
			var file = new StreamReader( FileName );
			Status = State.TopLevel;
			
			line = file.ReadLine().Trim();
			while( !file.EndOfStream ) {
				
				if ( line.Length > 0 ) {
					SplitVcfLine( line, out section, out attributes, out data);
					
					if ( Status == State.TopLevel
					  && section == EtqBeginSection
					  && data.ToUpper() == EtqCardAttribute
					  && attributes.Length == 0 )
					{
						// Start vCard
						Status = State.InCard;
						p = new Person();
					}
					else
					if ( Status == State.InCard
					  && section == EtqEndSection
					  && data.ToUpper() == EtqCardAttribute
					  && attributes.Length == 0 )
					{
						// Finish VCard
						Status = State.TopLevel;
						toret.Insert( p );
						p = null;
					}
					else
					if ( Status == State.InCard ) {
						// Fill fields
						FillCard( p, section, attributes, data );
					}
					else throw new ApplicationException( "Misplaced statement:'"
					                         	+ section + "'\nin '"
					                         	+ line + "'\nwhile in state: "
					                         	+ Status
					                         );
				}
				
				line = file.ReadLine().Trim();
			}
			
			file.Close();
			return toret;
		}
	}
}

