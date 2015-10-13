using System;
using Tacto.Core;

using GtkUtil;

namespace Tacto.Gui {
	public partial class ChooseConversionFormat {
		
		public Person.Format Format {
			get {
				Person.Format toret = Person.Format.CSV;
				int op = this.cbFormat.Active;
				
				if ( op >= 0 ) {
					toret += op;
				}
				
				return toret;
			}
		}
		
		public string FileName {
			get { return this.edFileName.Text; }
		}
		
		
		protected virtual void OnOpen()
		{
			string filter = "*";
			string extSuffix = "." + Person.FormatNames[ (int) Format ].ToLower();
			
			filter += extSuffix;

			var result = Util.DlgSave( Core.AppInfo.Name,
			                          	"Save converted data as...",
			                           	this,
			                          	ref fileName,
			                          	filter
			);
			
			if ( result ) {
				if ( !fileName.ToLower().EndsWith( extSuffix ) ) {
					fileName += extSuffix;
				}
				
				edFileName.Text = fileName;
			}
		}

		private string fileName = "";
	}
}
