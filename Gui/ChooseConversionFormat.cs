
using System;
using Gtk;
using Tacto.Core;

using GtkUtil;

namespace Tacto.Gui
{
	public partial class ChooseConversionFormat : Gtk.Dialog
	{
		private string fileName = "";

		public ChooseConversionFormat(Gtk.Window super)
		{
			init( super );
		}
		
		protected void init(Gtk.Window super)
		{
			this.Build();
			
			this.Icon = super.Icon;
			this.Parent = super;
			
			initCbFormat( this.cbFormat );
		}
		
		public static void initCbFormat(Gtk.ComboBox cb)
		{
			cb.Clear();
			ListStore listStore = new Gtk.ListStore( typeof( string ) );
			cb.Model = listStore;
			CellRendererText text = new CellRendererText();
			cb.PackStart( text, false );
			cb.AddAttribute( text, "text", 0 );
			
			foreach (var name in Person.FormatNames) {
				listStore.AppendValues( name );
			}
			
			cb.Active = 0;
		}
		
		public Person.Format Format {
			get {
				Person.Format toret = Person.Format.CSV;
				int op = cbFormat.Active;
				
				if ( op >= 0 ) {
					toret += op;
				}
				
				return toret;
			}
		}
		
		public string FileName {
			get { return edFileName.Text; }
		}
		
		
		protected virtual void btOpenActivated(object sender, System.EventArgs e)
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
	}
}
