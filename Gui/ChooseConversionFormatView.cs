using System;

namespace Tacto.Gui {
	public partial class ChooseConversionFormat: Gtk.Dialog {
		public ChooseConversionFormat(Gtk.Window parent)
		{
			this.Modal = true;
			this.Icon = parent.Icon;
			this.TransientFor = parent;
			this.WindowPosition = Gtk.WindowPosition.CenterOnParent;

			this.Build();
			this.ShowAll();
			this.Resizable = false;
		}

		private void BuildFormatCombo()
		{
			string[] formats = new string[ Tacto.Core.Person.FormatNames.Count ];

			Tacto.Core.Person.FormatNames.CopyTo( formats, 0 );
			this.cbFormat = new Gtk.ComboBox( formats );
			this.cbFormat.Active = 0;
		}

		private void BuildFileNameGroup(Gtk.HBox hbFileName) {
			this.btOpen = new Gtk.Button( Gtk.Stock.Open );
			this.edFileName = new Gtk.Entry();

			this.edFileName.IsEditable = false;
			this.btOpen.Clicked += (sender, e) => this.OnOpen();

			hbFileName.PackStart( this.edFileName, true, true, 5 );
			hbFileName.PackStart( this.btOpen, false, false, 5 );
		}

		private void Build() {
			var hbFileName = new Gtk.HBox( false, 5 );

			this.BuildFormatCombo();
			this.BuildFileNameGroup( hbFileName );

			this.VBox.PackStart( this.cbFormat, true, true, 5 );
			this.VBox.PackStart( hbFileName, true, true, 5 );
			this.AddButton( Gtk.Stock.Close, Gtk.ResponseType.Close );

			this.Modal = true;
		}

		private Gtk.ComboBox cbFormat;
		private Gtk.Entry edFileName;
		private Gtk.Button btOpen;
	}
}
