using System;

namespace Tacto.Gui {
	public partial class MainWindow: Gtk.Window {
		public MainWindow()
			:base( Gtk.WindowType.Toplevel )
		{
			this.Init = true;
			this.Title = Tacto.Core.AppInfo.Name;
			this.Build();
		}

		private void BuildActions() {
			this.actNew = new Gtk.Action( "new", "_New", "New contacts file", Gtk.Stock.New );
			this.actNew.Activated += (sender, e) => this.OnNew();

			this.actOpen = new Gtk.Action( "open", "_Open", "Open contacts file", Gtk.Stock.Open );
			this.actOpen.Activated += (sender, e) => this.OnOpen();

			this.actSave = new Gtk.Action( "save", "_Save", "Save contacts file", Gtk.Stock.Save );
			this.actSave.Activated += (sender, e) => this.Save();

			this.actAdd = new Gtk.Action( "add", "_Add", "Add contact", Gtk.Stock.Add );
			this.actAdd.Activated += (sender, e) => this.OnAdd();

			this.actModify = new Gtk.Action( "modify", "_Modify", "Modify contact", Gtk.Stock.Edit );
			this.actModify.Activated += (sender, e) => this.OnEdit();

			this.actRemove = new Gtk.Action( "remove", "_Remove", "Remove contact", Gtk.Stock.Remove );
			this.actRemove.Activated += (sender, e) => this.OnRemove();

			this.actConnect = new Gtk.Action( "connect", "_Connect", "Connect", Gtk.Stock.Connect );
			this.actConnect.Activated += (sender, e) => this.OnConnect();

			this.actSettings = new Gtk.Action( "settings", "_Settings", "Settings", Gtk.Stock.Preferences );
			this.actSettings.Activated += (sender, e) => this.OnSettings();

			this.actImport = new Gtk.Action( "import", "_Import", "Import", Gtk.Stock.Convert );
			this.actImport.Activated += (sender, e) => this.OnImport();

			this.actExport = new Gtk.Action( "export", "_Export", "Export", Gtk.Stock.Convert );
			this.actExport.Activated += (sender, e) => this.OnExport();

			this.actFind = new Gtk.Action( "find", "_Find", "Find", Gtk.Stock.Find );
			this.actFind.Activated += (sender, e) => this.OnFind();

			this.actFindAgain = new Gtk.Action( "findAgain", "Find a_gain", "Find again", Gtk.Stock.Find );
			this.actFindAgain.Activated += (sender, e) => this.OnFindAgain();

			this.actQuit = new Gtk.Action( "quit", "_Quit", "Quit", Gtk.Stock.Quit );
			this.actQuit.Activated += (sender, e) => this.Quit();

			this.actViewCard = new Gtk.Action( "view-card", "_View card", "View card", null );
			this.actViewCard.Activated += (sender, e) => this.OnViewCard();

			this.actSort = new Gtk.Action( "sort", "_Sort", "Sort", Gtk.Stock.SortAscending );
			this.actSort.Activated += (sender, e) => this.Sort();

			this.actAbout = new Gtk.Action( "about", "_About", "About", Gtk.Stock.About );
			this.actAbout.Activated += (sender, e) => this.OnAbout();
		}

		private void BuildTable() {
			var swView = new Gtk.ScrolledWindow();

			this.tvTable = new Gtk.TreeView();
			this.tvTable.EnableSearch = false;
			this.ttTable = new GtkUtil.TableTextView( this.tvTable, Headers.Count, Headers.Count );
			this.ttTable.Headers = Headers;
			this.tvTable.ButtonReleaseEvent += OnTableClick;
			this.tvTable.RowActivated += ( sender, evt ) => this.OnEdit();

			this.lblEmptyExplanation = new Gtk.Label(){ UseMarkup = true };

			swView.AddWithViewport( this.tvTable );
			this.vbMain.PackStart( swView, true, true, 5 );
			this.vbMain.PackStart( this.lblEmptyExplanation, false, false, 5 );
		}

		private void BuildMenu() {
			var accelerators = new Gtk.AccelGroup();
			var mMain = new Gtk.MenuBar();
			var mFile = new Gtk.Menu();
			var mEdit = new Gtk.Menu();
			var mView = new Gtk.Menu();
			var mTools = new Gtk.Menu();
			var mHelp = new Gtk.Menu();
			var miFile = new Gtk.MenuItem( "_File" );
			var miEdit = new Gtk.MenuItem( "_Edit" );
			var miView = new Gtk.MenuItem( "_View" );
			var miTools = new Gtk.MenuItem( "_Tools" );
			var miHelp = new Gtk.MenuItem( "_Help" );

			// Menu items
			var opNew = this.actNew.CreateMenuItem();
			opNew.AddAccelerator( "activate", accelerators, new Gtk.AccelKey(
				Gdk.Key.n, Gdk.ModifierType.ControlMask, Gtk.AccelFlags.Visible ) );

			var opOpen = this.actOpen.CreateMenuItem();
			opOpen.AddAccelerator( "activate", accelerators, new Gtk.AccelKey(
				Gdk.Key.o, Gdk.ModifierType.ControlMask, Gtk.AccelFlags.Visible ) );

			var opSave = this.actSave.CreateMenuItem();
			opSave.AddAccelerator( "activate", accelerators, new Gtk.AccelKey(
				Gdk.Key.s, Gdk.ModifierType.ControlMask, Gtk.AccelFlags.Visible ) );

			var opQuit = this.actQuit.CreateMenuItem();
			opQuit.AddAccelerator( "activate", accelerators, new Gtk.AccelKey(
				Gdk.Key.q, Gdk.ModifierType.ControlMask, Gtk.AccelFlags.Visible ) );

			var opAdd = this.actAdd.CreateMenuItem();
			opAdd.AddAccelerator( "activate", accelerators, new Gtk.AccelKey(
				Gdk.Key.plus, Gdk.ModifierType.ControlMask, Gtk.AccelFlags.Visible ) );

			var opRemove = this.actRemove.CreateMenuItem();
			opRemove.AddAccelerator( "activate", accelerators, new Gtk.AccelKey(
				Gdk.Key.Delete, Gdk.ModifierType.None, Gtk.AccelFlags.Visible ) );

			var opFind = this.actFind.CreateMenuItem();
			opFind.AddAccelerator( "activate", accelerators, new Gtk.AccelKey(
				Gdk.Key.f, Gdk.ModifierType.ControlMask, Gtk.AccelFlags.Visible ) );

			var opFindAgain = this.actFindAgain.CreateMenuItem();
			opFindAgain.AddAccelerator( "activate", accelerators, new Gtk.AccelKey(
				Gdk.Key.F3, Gdk.ModifierType.None, Gtk.AccelFlags.Visible ) );

			var opSettings = this.actSettings.CreateMenuItem();
			opSettings.AddAccelerator( "activate", accelerators, new Gtk.AccelKey(
				Gdk.Key.F2, Gdk.ModifierType.None, Gtk.AccelFlags.Visible ) );

			var opView = new Gtk.CheckMenuItem( this.actViewCard.Label );
			opView.Active = true;
			opView.Activated += (sender, e) => this.actViewCard.Activate();

			// Create menu structure
			mMain.Append( miFile );
			miFile.Submenu = mFile;
			mFile.Append( opNew );
			mFile.Append( opOpen );
			mFile.Append( this.actImport.CreateMenuItem() );
			mFile.Append( new Gtk.SeparatorMenuItem() );
			mFile.Append( opSave );
			mFile.Append( this.actExport.CreateMenuItem() );
			mFile.Append( new Gtk.SeparatorMenuItem() );
			mFile.Append( opQuit );

			mMain.Append( miEdit );
			miEdit.Submenu = mEdit;
			mEdit.Append( opAdd );
			mEdit.Append( opRemove );
			mEdit.Append( this.actModify.CreateMenuItem() );
			mEdit.Append( this.actConnect.CreateMenuItem() );
			mEdit.Append( new Gtk.SeparatorMenuItem() );
			mEdit.Append( opFind );
			mEdit.Append( opFindAgain );
			mEdit.Append( new Gtk.SeparatorMenuItem() );
			mEdit.Append( opSettings );

			mMain.Append( miView );
			miView.Submenu = mView;
			mView.Append( opView );

			mMain.Append( miTools );
			miTools.Submenu = mTools;
			mTools.Append( this.actSort.CreateMenuItem() );

			mMain.Append( miHelp );
			miHelp.Submenu = mHelp;
			mHelp.Append( this.actAbout.CreateMenuItem() );

			this.AddAccelGroup( accelerators );
			this.vbMain.PackStart( mMain, false, false, 0 );
		}

		private void BuildToolbar() {
			var hbToolbar = new Gtk.HBox( false, 5 );

			this.tbTool = new Gtk.Toolbar();
			this.cbCategory = new Gtk.ComboBox( new string[ 0 ] );
			this.edSearch = new Gtk.Entry();

			this.tbTool.Add( this.actSave.CreateToolItem() );
			this.tbTool.Add( this.actAdd.CreateToolItem() );
			this.tbTool.Add( this.actRemove.CreateToolItem() );
			this.tbTool.Add( this.actModify.CreateToolItem() );
			this.tbTool.Add( this.actConnect.CreateToolItem() );
			this.tbTool.Add( this.actSettings.CreateToolItem() );

			hbToolbar.PackStart( this.tbTool, true, true, 0 );
			hbToolbar.PackStart( this.cbCategory, false, false, 0 );
			hbToolbar.PackStart( this.edSearch, true, true, 0 );
			this.vbMain.PackStart( hbToolbar, false, false, 0 );
		}

		private void BuildStatus() {
			this.sbStatus = new Gtk.Statusbar();
			this.lblNumRecords = new Gtk.Label( "0" );
			this.sbStatus.Push( 0, "Building UI..." );
			this.sbStatus.Add( this.lblNumRecords );
			this.lblNumRecords.SetAlignment( (float) 0.95, 1 );

			this.vbMain.PackEnd( this.sbStatus, false, false, 0 );
		}

		private void BuildCard() {
			var eventBoxEmail1 = new Gtk.EventBox();
			var eventBoxEmail2 = new Gtk.EventBox();
			var eventBoxAddress = new Gtk.EventBox();
			var vbCardMain = new Gtk.VBox( false, 5 );
			var hbName = new Gtk.HBox( false, 0 );
			var hbAddress = new Gtk.HBox( false, 0 );
			var hbMainContact = new Gtk.HBox( false, 0 );
			var hbSecondaryContact = new Gtk.HBox( false, 0 );

			// Frame
			this.frmCard = new Gtk.Frame();
			this.frmCard.LabelWidget = new Gtk.Label() { Markup = "<b>Contact</b>" };

			// Name
			this.lblName = new Gtk.Label( EtqNotAvailable );
			this.lblSurname = new Gtk.Label( EtqNotAvailable );
			vbCardMain.PackStart( hbName, false, false, 0 );
			hbName.PackStart( this.lblSurname, false, false, 5 );
			hbName.PackStart( this.lblName, false, false, 5 );

			// Address
			this.lblAddress = new Gtk.Label( EtqNotAvailable );
			eventBoxAddress.Add( this.lblAddress );
			eventBoxAddress.ButtonPressEvent += (o, args) => this.OnAddressClicked();
			this.lblHomePhone = new Gtk.Label( EtqNotAvailable );
			vbCardMain.PackStart( hbAddress, false, false, 0 );
			hbAddress.PackStart( eventBoxAddress, false, false, 5 );
			hbAddress.PackStart( new Gtk.VSeparator(), false, false, 5 );
			hbAddress.PackStart( this.lblHomePhone, false, false, 5 );

			// Main contact
			this.lblMobilePhone = new Gtk.Label( EtqNotAvailable );
			this.lblEmail = new Gtk.Label( EtqNotAvailable );
			eventBoxEmail1.Add( this.lblEmail );
			eventBoxEmail1.ButtonPressEvent += (o, args) => this.OnLblEmail1Clicked();
			vbCardMain.PackStart( hbMainContact, false, false, 0 );
			hbMainContact.PackStart( this.lblMobilePhone, false, false, 5 );
			hbMainContact.PackStart( new Gtk.VSeparator(), false, false, 5 );
			hbMainContact.PackStart( eventBoxEmail1, false, false, 5 );

			// Secondary contact
			this.lblWorkPhone = new Gtk.Label( EtqNotAvailable );
			this.lblEmail2 = new Gtk.Label( EtqNotAvailable );
			eventBoxEmail2.Add( this.lblEmail2 );
			eventBoxEmail2.ButtonPressEvent += (o, args) => this.OnLblEmail2Clicked();
			vbCardMain.PackStart( hbSecondaryContact, false, false, 0 );
			hbSecondaryContact.PackStart( this.lblWorkPhone, false, false, 5 );
			hbSecondaryContact.PackStart( new Gtk.VSeparator(), false, false, 5 );
			hbSecondaryContact.PackStart( eventBoxEmail2, false, false, 5 );

			this.frmCard.Add( vbCardMain );
			this.vbMain.PackStart( this.frmCard, false, false, 5 );

			// Card labels
			this.lblSurname.ModifyFont(
				Pango.FontDescription.FromString( "Times 18" )
			);

			this.lblName.ModifyFont(
				Pango.FontDescription.FromString( "Times 18" )
			);

			this.lblMobilePhone.ModifyFont(
				Pango.FontDescription.FromString( "Times 14" )
			);

			this.lblWorkPhone.ModifyFont(
				Pango.FontDescription.FromString( "Times 14" )
			);

			this.lblAddress.ModifyFont(
				Pango.FontDescription.FromString( "Times 14" )
			);

			this.lblMobilePhone.ModifyFont(
				Pango.FontDescription.FromString( "Times 14" )
			);

			this.lblEmail.ModifyFont(
				Pango.FontDescription.FromString( "Mono 14" )
			);

			this.lblEmail2.ModifyFont(
				Pango.FontDescription.FromString( "Mono 14" )
			);
		}

		private void BuildPopup()
		{
			popup = new Gtk.Menu();
			popup.Append( this.actAdd.CreateMenuItem() );
			popup.Append( this.actRemove.CreateMenuItem() );
			popup.Append( this.actModify.CreateMenuItem() );
			popup.Append( this.actSettings.CreateMenuItem() );
			popup.ShowAll();
		}

		private void Build() {
			this.vbMain = new Gtk.VBox( false, 5 );

			this.BuildStatus();
			this.BuildActions();
			this.BuildMenu();
			this.BuildToolbar();
			this.BuildCard();
			this.BuildTable();
			this.BuildPopup();

			this.Add( this.vbMain );

			this.Shown += (sender, e) => this.OnShow();
			this.DeleteEvent += (object o, Gtk.DeleteEventArgs evt) => this.Quit( evt );
			this.cbCategory.Changed += (sender, e) => this.OnCategoryChanged();
			this.edSearch.FocusInEvent += (sender, e) => this.OnFocusEdSearch();
			this.edSearch.FocusOutEvent += (sender, e) => this.OnFocusOutEdSearch();
			this.edSearch.KeyReleaseEvent += (o, args) => this.OnEdSearchKeyReleased( o, args );

			this.SetGeometryHints(
				this,
				new Gdk.Geometry() {
					MinWidth = 640,
					MinHeight = 480
				},
				Gdk.WindowHints.MinSize
			);
			this.WindowPosition = Gtk.WindowPosition.Center;
			this.SetStatus();
		}

		/// <summary>
		/// Determines whether the window is being built and/or populated.
		/// </summary>
		/// <value><c>true</c> if in building; otherwise, <c>false</c>.</value>
		public bool Init {
			get; set;
		}

		private void Quit(Gtk.DeleteEventArgs evt) {
			this.Quit();
			evt.RetVal = true;
		}

		private Gtk.Menu popup = null;

		private Gtk.Action actNew;
		private Gtk.Action actOpen;
		private Gtk.Action actImport;
		private Gtk.Action actExport;
		private Gtk.Action actSave;
		private Gtk.Action actQuit;
		private Gtk.Action actAdd;
		private Gtk.Action actModify;
		private Gtk.Action actRemove;
		private Gtk.Action actConnect;
		private Gtk.Action actSettings;
		private Gtk.Action actFind;
		private Gtk.Action actFindAgain;
		private Gtk.Action actSort;
		private Gtk.Action actViewCard;
		private Gtk.Action actAbout;

		private Gtk.TreeView tvTable;
		private Gtk.VBox vbMain;
		private Gtk.Statusbar sbStatus;
		private Gtk.Toolbar tbTool;
		private Gtk.ComboBox cbCategory;
		private Gtk.Entry edSearch;
		private Gtk.Label lblNumRecords;
		private Gtk.Frame frmCard;
		private Gtk.Label lblName;
		private Gtk.Label lblSurname;
		private Gtk.Label lblAddress;
		private Gtk.Label lblMobilePhone;
		private Gtk.Label lblWorkPhone;
		private Gtk.Label lblHomePhone;
		private Gtk.Label lblEmail;
		private Gtk.Label lblEmail2;
		private Gtk.Label lblEmptyExplanation;
	}
}

