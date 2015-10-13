using System;
using Tacto.Core;

using GtkUtil;

namespace Tacto.Gui {
	public partial class DlgEditPerson: Gtk.Dialog {
		public DlgEditPerson(Gtk.Window super, CategoryList cats, Person p)
		{
			this.person = p;
			this.categories = cats;

			this.TransientFor = super;
			this.Modal = true;
			this.Title = super.Title;
			this.Icon = super.Icon;
			this.Build();
			this.ShowAll();
			this.SetPosition( Gtk.WindowPosition.CenterOnParent );
			this.Resizable = false;
		}

		private void BuildFormatCombo() {
			string[] formats = new string[ Tacto.Core.Person.FormatNames.Count ];

			Tacto.Core.Person.FormatNames.CopyTo( formats, 0 );
			this.cbOutputFormat = new Gtk.ComboBox( formats );
			this.cbOutputFormat.Active = 0;
		}

		private void BuildNotebook() {
			this.nbFiles = new Gtk.Notebook();

			this.vbPage1 = new Gtk.VBox( false, 5 );
			this.vbPage2 = new Gtk.VBox( false, 5 );
			this.nbFiles.AppendPage( this.vbPage1, new Gtk.Label( "Basic" ) );
			this.nbFiles.AppendPage( this.vbPage2, new Gtk.Label( "More..." ) );
			this.VBox.PackStart( this.nbFiles, true, true, 5 );
		}

		private void BuildNameFrame() {
			var vbBox = new Gtk.VBox( false, 5 );

			// The frame
			this.frmName = new Gtk.Frame( "Name" );
			this.frmName.Add( vbBox );

			// Given name
			var hbGivenName = new Gtk.HBox( false, 5 );
			this.edName = new Gtk.Entry();
			hbGivenName.PackStart( new Gtk.Label(){ Markup = "<b>Name</b>" }, false, false, 5 );
			hbGivenName.PackStart( this.edName, true, true, 5 );

			// Family name
			var hbFamilyName = new Gtk.HBox( false, 5 );
			this.edSurname = new Gtk.Entry();
			hbFamilyName.PackStart( new Gtk.Label() { Markup = "<b>Surname</b> <i>(or activity)</i>" }, false, false, 5 );
			hbFamilyName.PackStart( this.edSurname, true, true, 5 );

			vbBox.PackStart( hbGivenName, true, true, 5 );
			vbBox.PackStart( hbFamilyName, true, true, 5 );
			this.vbPage1.PackStart( this.frmName, true, true, 5 );
		}

		private void BuildContactFrame() {
			var vbBox = new Gtk.VBox( false, 5 );

			// The frame
			this.frmContact = new Gtk.Frame( "Main contact info" );
			this.frmContact.Add( vbBox );

			// Email
			var hbEmail = new Gtk.HBox( false, 5 );
			this.edEmail = new Gtk.Entry();
			this.btConnectEmail = new Gtk.Button( Gtk.Stock.Connect );
			this.btConnectEmail.Clicked += (sender, e) => this.OnConnect( sender );
			hbEmail.PackStart( new Gtk.Label(){ Markup = "<b>E.mail</b>" }, false, false, 5 );
			hbEmail.PackStart( this.edEmail, true, true, 5 );
			hbEmail.PackStart( this.btConnectEmail, false, false, 5 );

			// Mobile phone
			var hbMobilePhone = new Gtk.HBox( false, 5 );
			this.edMPhone = new Gtk.Entry();
			hbMobilePhone.PackStart( new Gtk.Label(){ Markup = "<b>Mobil phone</b>" }, false, false, 5 );
			hbMobilePhone.PackStart( this.edMPhone, true, true, 5 );

			vbBox.PackStart( hbEmail, true, true, 5 );
			vbBox.PackStart( hbMobilePhone, true, true, 5 );
			this.vbPage1.PackStart( this.frmContact, true, true, 5 );
		}

		private void BuildCategoriesFrame() {
			var vbBox = new Gtk.VBox( false, 5 );
			var vbExpandedBox = new Gtk.VBox( false, 5 );
			var hbAvailableCategories = new Gtk.HBox( false, 5 );
			var hbCurrentCategories = new Gtk.HBox( false, 5 );
			var exExpandCategories = new Gtk.Expander( "Categories" );

			// The frame
			this.frmCategories = new Gtk.Frame( "Manage categories" );
			this.frmCategories.Add( vbBox );

			// The expander
			exExpandCategories.Expanded = false;
			exExpandCategories.Add( vbExpandedBox );
			this.btAddCategory = new Gtk.Button( Gtk.Stock.Add );
			this.btAddCategory.Clicked += (sender, e) => this.OnAddCategory();
			this.btRemoveCategory = new Gtk.Button( Gtk.Stock.Add );
			this.btRemoveCategory.Clicked += (sender, e) => this.OnRemoveCategory();
			this.btRemoveCategory = new Gtk.Button( Gtk.Stock.Remove );
			this.cbAvailableCategories = new Gtk.ComboBox( new string[] { "" } );
			this.cbCurrentCategories = new Gtk.ComboBox( new string[] { "" } );
			hbAvailableCategories.PackStart( new Gtk.Label( "Available:" ), false, false, 5 );
			hbAvailableCategories.PackStart( this.cbAvailableCategories, true, true, 5 );
			hbAvailableCategories.PackStart( this.btAddCategory, false, false, 5 );
			hbCurrentCategories.PackStart( new Gtk.Label( "Current:" ), false, false, 5 );
			hbCurrentCategories.PackStart( this.cbCurrentCategories, true, true, 5 );
			hbCurrentCategories.PackStart( this.btRemoveCategory, false, false, 5 );
			vbExpandedBox.PackStart( hbCurrentCategories, true, true, 5 );
			vbExpandedBox.PackStart( hbAvailableCategories, true, true, 5 );

			// Categories
			this.lblCategories = new Gtk.Label() {  Markup = "<i>Current categories</i>:" };

			vbBox.PackStart( this.lblCategories, true, true, 5 );
			vbBox.PackStart( exExpandCategories, true, true, 5 );
			this.vbPage1.PackStart( this.frmCategories, true, true, 5 );
		}

		private void BuildSaveAsFrame() {
			var exSaveAsExpand = new Gtk.Expander( "Save as..." );
			var hbSaveAs = new Gtk.HBox( false, 5 );

			exSaveAsExpand.Expanded = false;
			this.BuildFormatCombo();
			this.btSaveAs = new Gtk.Button( Gtk.Stock.SaveAs );
			this.btSaveAs.Clicked += (sender, e) => this.OnSaveAs();

			hbSaveAs.PackStart( this.cbOutputFormat, true, true, 5 );
			hbSaveAs.PackStart( this.btSaveAs, false, false, 5 );

			exSaveAsExpand.Add( hbSaveAs );
			this.vbPage1.PackStart( exSaveAsExpand, true, true, 5 );
		}

		private void BuildSecondAddresses() {
			var hbEmail2 = new Gtk.HBox( false, 5 );
			var hbWeb = new Gtk.HBox( false, 5 );
			var hbWPhone = new Gtk.HBox( false, 5 );
			var hbHPhone = new Gtk.HBox( false, 5 );
			var hbAddress = new Gtk.HBox( false, 5 );
			var hbId = new Gtk.HBox( false, 5 );
			var hbComments = new Gtk.HBox( false, 5 );

			// Email2
			this.edEmail2 = new Gtk.Entry();
			this.btConnectEmail2 = new Gtk.Button( Gtk.Stock.Connect );
			this.btConnectEmail2.Clicked += (sender, e) => this.OnConnect( sender );
			hbEmail2.PackStart( new Gtk.Label( "Email(2):" ), false, false, 5 );
			hbEmail2.PackStart( this.edEmail2, true, true, 5 );
			hbEmail2.PackStart( this.btConnectEmail2, false, false, 5 );

			// Web
			this.edWeb = new Gtk.Entry( "http://" );
			this.btConnectWeb = new Gtk.Button( Gtk.Stock.Connect );
			this.btConnectWeb.Clicked += (sender, e) => this.OnConnectWeb();
			hbWeb.PackStart( new Gtk.Label( "Web:" ), false, false, 5 );
			hbWeb.PackStart( this.edWeb, true, true, 5 );
			hbWeb.PackStart( this.btConnectWeb, false, false, 5 );

			// Work phone
			this.edWPhone = new Gtk.Entry( "0" );
			hbWPhone.PackStart( new Gtk.Label( "Work phone:" ), false, false, 5 );
			hbWPhone.PackStart( this.edWPhone, true, true, 5 );

			// Home phone
			this.edHPhone = new Gtk.Entry();
			hbHPhone.PackStart( new Gtk.Label( "Home phone:" ), false, false, 5 );
			hbHPhone.PackStart( this.edHPhone, true, true, 5 );

			// Address
			this.edAddress = new Gtk.Entry();
			this.btConnectMap = new Gtk.Button( Gtk.Stock.Connect );
			this.btConnectMap.Clicked += (sender, ed) => this.OnConnectMap();
			hbAddress.PackStart( new Gtk.Label( "Address:" ), false, false, 5 );
			hbAddress.PackStart( this.edAddress, true, true, 5 );
			hbAddress.PackStart( this.btConnectMap, false, false, 5 );

			// Id
			this.edId = new Gtk.Entry();
			hbId.PackStart( new Gtk.Label( "Id:" ), false, false, 5 );
			hbId.PackStart( this.edId, true, true, 5 );

			// Comments
			this.edComments = new Gtk.Entry();
			hbComments.PackStart( new Gtk.Label( "Comments:" ), false, false, 5 );
			hbComments.PackStart( this.edComments, true, true, 5 );

			this.vbPage2.PackStart( hbEmail2, true, true, 5 );
			this.vbPage2.PackStart( hbWeb, true, true, 5 );
			this.vbPage2.PackStart( hbWPhone, true, true, 5 );
			this.vbPage2.PackStart( hbHPhone, true, true, 5 );
			this.vbPage2.PackStart( hbAddress, true, true, 5 );
			this.vbPage2.PackStart( hbId, true, true, 5 );
			this.vbPage2.PackStart( hbComments, true, true, 5 );
		}

		private void Build() {
			this.BuildNotebook();
			this.BuildNameFrame();
			this.BuildContactFrame();
			this.BuildCategoriesFrame();
			this.BuildSaveAsFrame();
			this.BuildSecondAddresses();

			this.AddButton( Gtk.Stock.Cancel, Gtk.ResponseType.Cancel );
			this.AddButton( Gtk.Stock.Ok, Gtk.ResponseType.Ok );
			this.Shown += (sender, e) => this.OnShow();
		}

		private Gtk.Entry edName;
		private Gtk.Entry edSurname;
		private Gtk.Entry edEmail;
		private Gtk.Entry edEmail2;
		private Gtk.Entry edWeb;
		private Gtk.Entry edMPhone;
		private Gtk.Entry edHPhone;
		private Gtk.Entry edWPhone;
		private Gtk.Entry edAddress;
		private Gtk.Entry edId;
		private Gtk.Entry edComments;
		private Gtk.VBox vbPage1;
		private Gtk.VBox vbPage2;
		private Gtk.Notebook nbFiles;
		private Gtk.Frame frmName;
		private Gtk.Frame frmContact;
		private Gtk.Frame frmCategories;
		private Gtk.ComboBox cbOutputFormat;
		private Gtk.ComboBox cbAvailableCategories;
		private Gtk.ComboBox cbCurrentCategories;
		private Gtk.Label lblCategories;
		private Gtk.Button btConnectEmail;
		private Gtk.Button btConnectEmail2;
		private Gtk.Button btConnectWeb;
		private Gtk.Button btConnectMap;
		private Gtk.Button btAddCategory;
		private Gtk.Button btRemoveCategory;
		private Gtk.Button btSaveAs;
	}
}

