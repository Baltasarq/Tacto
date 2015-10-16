using System;

namespace Tacto.Gui
{
	public partial class DlgSettings: Gtk.Dialog {
		public DlgSettings(Gtk.Window parent, Tacto.Core.AgendaSystem agenda)
		{
			this.agendaSystem = agenda;
			this.TransientFor = parent;
			this.Icon = parent.Icon;

			this.Build();
			this.ShowAll();
		}

		private void Build() {
			var hBox = new Gtk.HBox( false, 5 );
			var vbButtons = new Gtk.VBox( false, 5 );
			var swView = new Gtk.ScrolledWindow();

			this.tvCategories = new Gtk.TreeView();
			swView.AddWithViewport( this.tvCategories );
			this.BuildButtons( vbButtons );

			hBox.PackStart( swView, true, true, 5 );
			hBox.PackStart( vbButtons, false, false, 5 );

			this.AddButton( Gtk.Stock.Close, Gtk.ResponseType.Close );
			this.VBox.PackStart( hBox, true, true, 5 );

			this.SetGeometryHints(
				this,
				new Gdk.Geometry() {
					MinHeight = 200,
					MinWidth = 320,
				},
				Gdk.WindowHints.MinSize
			);
			this.WindowPosition = Gtk.WindowPosition.CenterOnParent;
			this.Shown += (object sender, EventArgs e) =>  this.OnShow();
			this.Modal = true;
		}

		private void BuildButtons(Gtk.VBox vbButtons) {
			this.btAdd = new Gtk.Button( Gtk.Stock.Add );
			this.btRemove = new Gtk.Button( Gtk.Stock.Remove );

			this.btAdd.Clicked += this.OnAdd;
			this.btRemove.Clicked += this.OnRemove;

			vbButtons.PackStart( this.btAdd, false, false, 5 );
			vbButtons.PackStart( this.btRemove, false, false, 5 );
		}

		private Gtk.TreeView tvCategories;
		private Gtk.Button btAdd;
		private Gtk.Button btRemove;
	}
}

