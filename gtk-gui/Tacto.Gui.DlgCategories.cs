
// This file has been generated by the GUI designer. Do not modify.
namespace Tacto.Gui
{
	public partial class DlgCategories
	{
		private global::Gtk.HBox hbox1;
		
		private global::Gtk.ScrolledWindow GtkScrolledWindow;
		
		private global::Gtk.TreeView tvTable;
		
		private global::Gtk.VBox vbox3;
		
		private global::Gtk.Button btAdd;
		
		private global::Gtk.Button btRemove;
		
		private global::Gtk.Alignment alignment1;
		
		private global::Gtk.Button buttonOk;

		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget Tacto.Gui.DlgCategories
			this.Name = "Tacto.Gui.DlgCategories";
			this.WindowPosition = ((global::Gtk.WindowPosition)(4));
			// Internal child Tacto.Gui.DlgCategories.VBox
			global::Gtk.VBox w1 = this.VBox;
			w1.Name = "dialog1_VBox";
			w1.BorderWidth = ((uint)(2));
			// Container child dialog1_VBox.Gtk.Box+BoxChild
			this.hbox1 = new global::Gtk.HBox ();
			this.hbox1.Name = "hbox1";
			this.hbox1.Spacing = 6;
			// Container child hbox1.Gtk.Box+BoxChild
			this.GtkScrolledWindow = new global::Gtk.ScrolledWindow ();
			this.GtkScrolledWindow.Name = "GtkScrolledWindow";
			this.GtkScrolledWindow.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child GtkScrolledWindow.Gtk.Container+ContainerChild
			this.tvTable = new global::Gtk.TreeView ();
			this.tvTable.CanFocus = true;
			this.tvTable.Name = "tvTable";
			this.GtkScrolledWindow.Add (this.tvTable);
			this.hbox1.Add (this.GtkScrolledWindow);
			global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.GtkScrolledWindow]));
			w3.Position = 0;
			// Container child hbox1.Gtk.Box+BoxChild
			this.vbox3 = new global::Gtk.VBox ();
			this.vbox3.Name = "vbox3";
			this.vbox3.Spacing = 6;
			// Container child vbox3.Gtk.Box+BoxChild
			this.btAdd = new global::Gtk.Button ();
			this.btAdd.CanFocus = true;
			this.btAdd.Name = "btAdd";
			this.btAdd.UseStock = true;
			this.btAdd.UseUnderline = true;
			this.btAdd.Label = "gtk-add";
			this.vbox3.Add (this.btAdd);
			global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(this.vbox3 [this.btAdd]));
			w4.Position = 0;
			w4.Expand = false;
			w4.Fill = false;
			// Container child vbox3.Gtk.Box+BoxChild
			this.btRemove = new global::Gtk.Button ();
			this.btRemove.CanFocus = true;
			this.btRemove.Name = "btRemove";
			this.btRemove.UseStock = true;
			this.btRemove.UseUnderline = true;
			this.btRemove.Label = "gtk-remove";
			this.vbox3.Add (this.btRemove);
			global::Gtk.Box.BoxChild w5 = ((global::Gtk.Box.BoxChild)(this.vbox3 [this.btRemove]));
			w5.Position = 1;
			w5.Expand = false;
			w5.Fill = false;
			// Container child vbox3.Gtk.Box+BoxChild
			this.alignment1 = new global::Gtk.Alignment (0.5F, 0.5F, 1F, 1F);
			this.alignment1.Name = "alignment1";
			this.vbox3.Add (this.alignment1);
			global::Gtk.Box.BoxChild w6 = ((global::Gtk.Box.BoxChild)(this.vbox3 [this.alignment1]));
			w6.Position = 2;
			this.hbox1.Add (this.vbox3);
			global::Gtk.Box.BoxChild w7 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.vbox3]));
			w7.Position = 1;
			w7.Expand = false;
			w7.Fill = false;
			w1.Add (this.hbox1);
			global::Gtk.Box.BoxChild w8 = ((global::Gtk.Box.BoxChild)(w1 [this.hbox1]));
			w8.Position = 0;
			// Internal child Tacto.Gui.DlgCategories.ActionArea
			global::Gtk.HButtonBox w9 = this.ActionArea;
			w9.Name = "dialog1_ActionArea";
			w9.Spacing = 10;
			w9.BorderWidth = ((uint)(5));
			w9.LayoutStyle = ((global::Gtk.ButtonBoxStyle)(4));
			// Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.buttonOk = new global::Gtk.Button ();
			this.buttonOk.CanDefault = true;
			this.buttonOk.CanFocus = true;
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.UseStock = true;
			this.buttonOk.UseUnderline = true;
			this.buttonOk.Label = "gtk-close";
			this.AddActionWidget (this.buttonOk, -7);
			global::Gtk.ButtonBox.ButtonBoxChild w10 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w9 [this.buttonOk]));
			w10.Expand = false;
			w10.Fill = false;
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.DefaultWidth = 400;
			this.DefaultHeight = 300;
			this.Show ();
			this.btAdd.Clicked += new global::System.EventHandler (this.OnAdd);
			this.btRemove.Clicked += new global::System.EventHandler (this.OnRemove);
		}
	}
}