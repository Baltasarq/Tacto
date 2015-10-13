
// This file has been generated by the GUI designer. Do not modify.
namespace Tacto.Gui
{
	public partial class MainWindow
	{
		private global::Gtk.UIManager UIManager;
		
		private global::Gtk.Action FileAction;
		
		private global::Gtk.Action quitAction;
		
		private global::Gtk.Action HelpAction;
		
		private global::Gtk.Action aboutAction;
		
		private global::Gtk.Action addAction;
		
		private global::Gtk.Action removeAction;
		
		private global::Gtk.Action editAction;
		
		private global::Gtk.Action EditAction;
		
		private global::Gtk.Action addAction1;
		
		private global::Gtk.Action removeAction1;
		
		private global::Gtk.Action editAction1;
		
		private global::Gtk.Action openAction;
		
		private global::Gtk.Action saveAction;
		
		private global::Gtk.Action saveAction1;
		
		private global::Gtk.Action openAction1;
		
		private global::Gtk.Action propertiesAction;
		
		private global::Gtk.Action findAction;
		
		private global::Gtk.Action openAction2;
		
		private global::Gtk.Action opConvert;
		
		private global::Gtk.Action editCategoriesAction;
		
		private global::Gtk.Action opFindAgain;
		
		private global::Gtk.Action ImportAction;
		
		private global::Gtk.Action ToolsAction;
		
		private global::Gtk.Action SortAction;
		
		private global::Gtk.Action connectAction;
		
		private global::Gtk.Action newAction;
		
		private global::Gtk.Action ViewAction;
		
		private global::Gtk.ToggleAction ContactPanelAction;
		
		private global::Gtk.VBox vbox1;
		
		private global::Gtk.MenuBar menuBar;
		
		private global::Gtk.HBox hbox2;
		
		private global::Gtk.Toolbar tbToolbar;
		
		private global::Gtk.ComboBox cbCategory;
		
		private global::Gtk.Entry edSearch;
		
		private global::Gtk.Frame frmMainContact;
		
		private global::Gtk.Alignment GtkAlignment2;
		
		private global::Gtk.VBox vbox2;
		
		private global::Gtk.HBox hbox1;
		
		private global::Gtk.Label lblSurname;
		
		private global::Gtk.Label lblName;
		
		private global::Gtk.HBox hbox3;
		
		private global::Gtk.HBox hbox5;
		
		private global::Gtk.Label lblAddress;
		
		private global::Gtk.VSeparator vseparator1;
		
		private global::Gtk.Label lblPhone;
		
		private global::Gtk.HBox hbox4;
		
		private global::Gtk.Label lblMobilePhone;
		
		private global::Gtk.EventBox eventbox1;
		
		private global::Gtk.Label lblEmail;
		
		private global::Gtk.Alignment alignment2;
		
		private global::Gtk.HBox hbox6;
		
		private global::Gtk.Label lblPhoneWork;
		
		private global::Gtk.EventBox eventbox2;
		
		private global::Gtk.Label lblEmail2;
		
		private global::Gtk.Alignment alignment3;
		
		private global::Gtk.Label lblFrameContact;
		
		private global::Gtk.ScrolledWindow GtkScrolledWindow;
		
		private global::Gtk.TreeView tvTable;
		
		private global::Gtk.Label lblEmptyExplanation;
		
		private global::Gtk.Statusbar sbStatus;

		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget Tacto.Gui.MainWindow
			this.UIManager = new global::Gtk.UIManager ();
			global::Gtk.ActionGroup w1 = new global::Gtk.ActionGroup ("Default");
			this.FileAction = new global::Gtk.Action ("FileAction", "_File", null, null);
			this.FileAction.ShortLabel = "_File";
			w1.Add (this.FileAction, null);
			this.quitAction = new global::Gtk.Action ("quitAction", "_Quit", null, "gtk-quit");
			this.quitAction.ShortLabel = "_Quit";
			w1.Add (this.quitAction, null);
			this.HelpAction = new global::Gtk.Action ("HelpAction", "_Help", null, null);
			this.HelpAction.ShortLabel = "_Help";
			w1.Add (this.HelpAction, null);
			this.aboutAction = new global::Gtk.Action ("aboutAction", "_About", null, "gtk-about");
			this.aboutAction.ShortLabel = "_About";
			w1.Add (this.aboutAction, null);
			this.addAction = new global::Gtk.Action ("addAction", null, null, "gtk-add");
			w1.Add (this.addAction, null);
			this.removeAction = new global::Gtk.Action ("removeAction", null, null, "gtk-remove");
			w1.Add (this.removeAction, null);
			this.editAction = new global::Gtk.Action ("editAction", null, null, "gtk-edit");
			w1.Add (this.editAction, null);
			this.EditAction = new global::Gtk.Action ("EditAction", "_Edit", null, null);
			this.EditAction.ShortLabel = "_Edit";
			w1.Add (this.EditAction, null);
			this.addAction1 = new global::Gtk.Action ("addAction1", "_Add", null, "gtk-add");
			this.addAction1.ShortLabel = "_Add";
			w1.Add (this.addAction1, null);
			this.removeAction1 = new global::Gtk.Action ("removeAction1", "_Remove", null, "gtk-remove");
			this.removeAction1.ShortLabel = "_Remove";
			w1.Add (this.removeAction1, null);
			this.editAction1 = new global::Gtk.Action ("editAction1", "_Edit", null, "gtk-edit");
			this.editAction1.ShortLabel = "_Edit";
			w1.Add (this.editAction1, null);
			this.openAction = new global::Gtk.Action ("openAction", "_Open", null, "gtk-open");
			this.openAction.ShortLabel = "_Open";
			w1.Add (this.openAction, null);
			this.saveAction = new global::Gtk.Action ("saveAction", "_Save", null, "gtk-save");
			this.saveAction.ShortLabel = "_Save";
			w1.Add (this.saveAction, null);
			this.saveAction1 = new global::Gtk.Action ("saveAction1", null, null, "gtk-save");
			w1.Add (this.saveAction1, null);
			this.openAction1 = new global::Gtk.Action ("openAction1", "_Open", null, "gtk-open");
			this.openAction1.ShortLabel = "_Open";
			w1.Add (this.openAction1, null);
			this.propertiesAction = new global::Gtk.Action ("propertiesAction", "_Categories", null, "gtk-properties");
			this.propertiesAction.ShortLabel = "_Categories";
			w1.Add (this.propertiesAction, null);
			this.findAction = new global::Gtk.Action ("findAction", "_Find", null, "gtk-find");
			this.findAction.ShortLabel = "_Find";
			w1.Add (this.findAction, null);
			this.openAction2 = new global::Gtk.Action ("openAction2", null, null, "gtk-open");
			w1.Add (this.openAction2, null);
			this.opConvert = new global::Gtk.Action ("opConvert", "_Convert", null, "gtk-convert");
			this.opConvert.ShortLabel = "_Convert";
			w1.Add (this.opConvert, null);
			this.editCategoriesAction = new global::Gtk.Action ("editCategoriesAction", null, null, "gtk-preferences");
			w1.Add (this.editCategoriesAction, null);
			this.opFindAgain = new global::Gtk.Action ("opFindAgain", "Find agai_n", null, "gtk-find");
			this.opFindAgain.ShortLabel = "Find agai_n";
			w1.Add (this.opFindAgain, "F3");
			this.ImportAction = new global::Gtk.Action ("ImportAction", "_Import", null, null);
			this.ImportAction.ShortLabel = "_Import";
			w1.Add (this.ImportAction, null);
			this.ToolsAction = new global::Gtk.Action ("ToolsAction", "_Tools", null, null);
			this.ToolsAction.ShortLabel = "_Tools";
			w1.Add (this.ToolsAction, null);
			this.SortAction = new global::Gtk.Action ("SortAction", "_Sort", null, null);
			this.SortAction.ShortLabel = "_Sort";
			w1.Add (this.SortAction, null);
			this.connectAction = new global::Gtk.Action ("connectAction", null, null, "gtk-connect");
			w1.Add (this.connectAction, null);
			this.newAction = new global::Gtk.Action ("newAction", "_New", null, "gtk-new");
			this.newAction.ShortLabel = "_New";
			w1.Add (this.newAction, null);
			this.ViewAction = new global::Gtk.Action ("ViewAction", "_View", null, null);
			this.ViewAction.ShortLabel = "_View";
			w1.Add (this.ViewAction, null);
			this.ContactPanelAction = new global::Gtk.ToggleAction ("ContactPanelAction", "_Contact panel", null, null);
			this.ContactPanelAction.Active = true;
			this.ContactPanelAction.ShortLabel = "_Contact panel";
			w1.Add (this.ContactPanelAction, null);
			this.UIManager.InsertActionGroup (w1, 0);
			this.AddAccelGroup (this.UIManager.AccelGroup);
			this.Name = "Tacto.Gui.MainWindow";
			this.Title = "MainWindow";
			this.Icon = global::Gdk.Pixbuf.LoadFromResource ("Tacto.pixmaps.card-file-icon.png");
			this.WindowPosition = ((global::Gtk.WindowPosition)(4));
			// Container child Tacto.Gui.MainWindow.Gtk.Container+ContainerChild
			this.vbox1 = new global::Gtk.VBox ();
			this.vbox1.Name = "vbox1";
			this.vbox1.Spacing = 6;
			// Container child vbox1.Gtk.Box+BoxChild
			this.UIManager.AddUiFromString (@"<ui><menubar name='menuBar'><menu name='FileAction' action='FileAction'><menuitem name='newAction' action='newAction'/><menuitem name='openAction1' action='openAction1'/><menuitem name='ImportAction' action='ImportAction'/><menuitem name='saveAction' action='saveAction'/><menuitem name='opConvert' action='opConvert'/><menuitem name='quitAction' action='quitAction'/></menu><menu name='EditAction' action='EditAction'><menuitem name='addAction1' action='addAction1'/><menuitem name='removeAction1' action='removeAction1'/><menuitem name='editAction1' action='editAction1'/><menuitem name='findAction' action='findAction'/><menuitem name='opFindAgain' action='opFindAgain'/><menuitem name='propertiesAction' action='propertiesAction'/></menu><menu name='ViewAction' action='ViewAction'><menuitem name='ContactPanelAction' action='ContactPanelAction'/></menu><menu name='ToolsAction' action='ToolsAction'><menuitem name='SortAction' action='SortAction'/></menu><menu name='HelpAction' action='HelpAction'><menuitem name='aboutAction' action='aboutAction'/></menu></menubar></ui>");
			this.menuBar = ((global::Gtk.MenuBar)(this.UIManager.GetWidget ("/menuBar")));
			this.menuBar.Name = "menuBar";
			this.vbox1.Add (this.menuBar);
			global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.menuBar]));
			w2.Position = 0;
			w2.Expand = false;
			w2.Fill = false;
			// Container child vbox1.Gtk.Box+BoxChild
			this.hbox2 = new global::Gtk.HBox ();
			this.hbox2.Name = "hbox2";
			this.hbox2.Spacing = 6;
			// Container child hbox2.Gtk.Box+BoxChild
			this.UIManager.AddUiFromString (@"<ui><toolbar name='tbToolbar'><toolitem name='openAction2' action='openAction2'/><toolitem name='saveAction1' action='saveAction1'/><toolitem name='addAction' action='addAction'/><toolitem name='removeAction' action='removeAction'/><toolitem name='editAction' action='editAction'/><toolitem name='connectAction' action='connectAction'/><toolitem name='editCategoriesAction' action='editCategoriesAction'/></toolbar></ui>");
			this.tbToolbar = ((global::Gtk.Toolbar)(this.UIManager.GetWidget ("/tbToolbar")));
			this.tbToolbar.Name = "tbToolbar";
			this.tbToolbar.ShowArrow = false;
			this.tbToolbar.ToolbarStyle = ((global::Gtk.ToolbarStyle)(0));
			this.tbToolbar.IconSize = ((global::Gtk.IconSize)(2));
			this.hbox2.Add (this.tbToolbar);
			global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.tbToolbar]));
			w3.Position = 0;
			// Container child hbox2.Gtk.Box+BoxChild
			this.cbCategory = global::Gtk.ComboBox.NewText ();
			this.cbCategory.Name = "cbCategory";
			this.hbox2.Add (this.cbCategory);
			global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.cbCategory]));
			w4.Position = 1;
			w4.Expand = false;
			w4.Fill = false;
			// Container child hbox2.Gtk.Box+BoxChild
			this.edSearch = new global::Gtk.Entry ();
			this.edSearch.CanFocus = true;
			this.edSearch.Name = "edSearch";
			this.edSearch.IsEditable = true;
			this.edSearch.InvisibleChar = '●';
			this.hbox2.Add (this.edSearch);
			global::Gtk.Box.BoxChild w5 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.edSearch]));
			w5.Position = 2;
			this.vbox1.Add (this.hbox2);
			global::Gtk.Box.BoxChild w6 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.hbox2]));
			w6.Position = 1;
			w6.Expand = false;
			w6.Fill = false;
			// Container child vbox1.Gtk.Box+BoxChild
			this.frmMainContact = new global::Gtk.Frame ();
			this.frmMainContact.Name = "frmMainContact";
			this.frmMainContact.ShadowType = ((global::Gtk.ShadowType)(2));
			// Container child frmMainContact.Gtk.Container+ContainerChild
			this.GtkAlignment2 = new global::Gtk.Alignment (0F, 0F, 1F, 1F);
			this.GtkAlignment2.Name = "GtkAlignment2";
			this.GtkAlignment2.LeftPadding = ((uint)(12));
			// Container child GtkAlignment2.Gtk.Container+ContainerChild
			this.vbox2 = new global::Gtk.VBox ();
			this.vbox2.Name = "vbox2";
			this.vbox2.Spacing = 6;
			// Container child vbox2.Gtk.Box+BoxChild
			this.hbox1 = new global::Gtk.HBox ();
			this.hbox1.Name = "hbox1";
			this.hbox1.Spacing = 6;
			// Container child hbox1.Gtk.Box+BoxChild
			this.lblSurname = new global::Gtk.Label ();
			this.lblSurname.Name = "lblSurname";
			this.lblSurname.LabelProp = "N/A";
			this.hbox1.Add (this.lblSurname);
			global::Gtk.Box.BoxChild w7 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.lblSurname]));
			w7.Position = 0;
			w7.Expand = false;
			w7.Fill = false;
			// Container child hbox1.Gtk.Box+BoxChild
			this.lblName = new global::Gtk.Label ();
			this.lblName.Name = "lblName";
			this.lblName.LabelProp = "N/A";
			this.lblName.UseMarkup = true;
			this.lblName.SingleLineMode = true;
			this.hbox1.Add (this.lblName);
			global::Gtk.Box.BoxChild w8 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.lblName]));
			w8.Position = 1;
			w8.Expand = false;
			w8.Fill = false;
			this.vbox2.Add (this.hbox1);
			global::Gtk.Box.BoxChild w9 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.hbox1]));
			w9.Position = 0;
			w9.Expand = false;
			w9.Fill = false;
			// Container child vbox2.Gtk.Box+BoxChild
			this.hbox3 = new global::Gtk.HBox ();
			this.hbox3.Name = "hbox3";
			this.hbox3.Spacing = 6;
			// Container child hbox3.Gtk.Box+BoxChild
			this.hbox5 = new global::Gtk.HBox ();
			this.hbox5.Name = "hbox5";
			this.hbox5.Spacing = 6;
			// Container child hbox5.Gtk.Box+BoxChild
			this.lblAddress = new global::Gtk.Label ();
			this.lblAddress.Name = "lblAddress";
			this.lblAddress.LabelProp = "N/A";
			this.hbox5.Add (this.lblAddress);
			global::Gtk.Box.BoxChild w10 = ((global::Gtk.Box.BoxChild)(this.hbox5 [this.lblAddress]));
			w10.Position = 0;
			w10.Expand = false;
			w10.Fill = false;
			// Container child hbox5.Gtk.Box+BoxChild
			this.vseparator1 = new global::Gtk.VSeparator ();
			this.vseparator1.Name = "vseparator1";
			this.hbox5.Add (this.vseparator1);
			global::Gtk.Box.BoxChild w11 = ((global::Gtk.Box.BoxChild)(this.hbox5 [this.vseparator1]));
			w11.Position = 1;
			w11.Expand = false;
			w11.Fill = false;
			this.hbox3.Add (this.hbox5);
			global::Gtk.Box.BoxChild w12 = ((global::Gtk.Box.BoxChild)(this.hbox3 [this.hbox5]));
			w12.Position = 0;
			w12.Expand = false;
			w12.Fill = false;
			// Container child hbox3.Gtk.Box+BoxChild
			this.lblPhone = new global::Gtk.Label ();
			this.lblPhone.Name = "lblPhone";
			this.lblPhone.LabelProp = "N/A";
			this.lblPhone.UseMarkup = true;
			this.hbox3.Add (this.lblPhone);
			global::Gtk.Box.BoxChild w13 = ((global::Gtk.Box.BoxChild)(this.hbox3 [this.lblPhone]));
			w13.Position = 1;
			w13.Expand = false;
			w13.Fill = false;
			this.vbox2.Add (this.hbox3);
			global::Gtk.Box.BoxChild w14 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.hbox3]));
			w14.Position = 1;
			w14.Expand = false;
			w14.Fill = false;
			// Container child vbox2.Gtk.Box+BoxChild
			this.hbox4 = new global::Gtk.HBox ();
			this.hbox4.Name = "hbox4";
			this.hbox4.Spacing = 6;
			// Container child hbox4.Gtk.Box+BoxChild
			this.lblMobilePhone = new global::Gtk.Label ();
			this.lblMobilePhone.Name = "lblMobilePhone";
			this.lblMobilePhone.LabelProp = "N/A";
			this.hbox4.Add (this.lblMobilePhone);
			global::Gtk.Box.BoxChild w15 = ((global::Gtk.Box.BoxChild)(this.hbox4 [this.lblMobilePhone]));
			w15.Position = 0;
			w15.Expand = false;
			w15.Fill = false;
			// Container child hbox4.Gtk.Box+BoxChild
			this.eventbox1 = new global::Gtk.EventBox ();
			this.eventbox1.Name = "eventbox1";
			// Container child eventbox1.Gtk.Container+ContainerChild
			this.lblEmail = new global::Gtk.Label ();
			this.lblEmail.Name = "lblEmail";
			this.lblEmail.LabelProp = "N/A";
			this.lblEmail.UseMarkup = true;
			this.lblEmail.UseUnderline = true;
			this.lblEmail.SingleLineMode = true;
			this.eventbox1.Add (this.lblEmail);
			this.hbox4.Add (this.eventbox1);
			global::Gtk.Box.BoxChild w17 = ((global::Gtk.Box.BoxChild)(this.hbox4 [this.eventbox1]));
			w17.Position = 1;
			w17.Expand = false;
			w17.Fill = false;
			// Container child hbox4.Gtk.Box+BoxChild
			this.alignment2 = new global::Gtk.Alignment (0.5F, 0.5F, 1F, 1F);
			this.alignment2.Name = "alignment2";
			this.hbox4.Add (this.alignment2);
			global::Gtk.Box.BoxChild w18 = ((global::Gtk.Box.BoxChild)(this.hbox4 [this.alignment2]));
			w18.Position = 2;
			this.vbox2.Add (this.hbox4);
			global::Gtk.Box.BoxChild w19 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.hbox4]));
			w19.Position = 2;
			w19.Expand = false;
			w19.Fill = false;
			// Container child vbox2.Gtk.Box+BoxChild
			this.hbox6 = new global::Gtk.HBox ();
			this.hbox6.Name = "hbox6";
			this.hbox6.Spacing = 6;
			// Container child hbox6.Gtk.Box+BoxChild
			this.lblPhoneWork = new global::Gtk.Label ();
			this.lblPhoneWork.Name = "lblPhoneWork";
			this.lblPhoneWork.LabelProp = "N/A";
			this.hbox6.Add (this.lblPhoneWork);
			global::Gtk.Box.BoxChild w20 = ((global::Gtk.Box.BoxChild)(this.hbox6 [this.lblPhoneWork]));
			w20.Position = 0;
			w20.Expand = false;
			w20.Fill = false;
			// Container child hbox6.Gtk.Box+BoxChild
			this.eventbox2 = new global::Gtk.EventBox ();
			this.eventbox2.Name = "eventbox2";
			// Container child eventbox2.Gtk.Container+ContainerChild
			this.lblEmail2 = new global::Gtk.Label ();
			this.lblEmail2.Name = "lblEmail2";
			this.lblEmail2.LabelProp = "N/A";
			this.lblEmail2.UseUnderline = true;
			this.eventbox2.Add (this.lblEmail2);
			this.hbox6.Add (this.eventbox2);
			global::Gtk.Box.BoxChild w22 = ((global::Gtk.Box.BoxChild)(this.hbox6 [this.eventbox2]));
			w22.Position = 1;
			w22.Expand = false;
			w22.Fill = false;
			// Container child hbox6.Gtk.Box+BoxChild
			this.alignment3 = new global::Gtk.Alignment (0.5F, 0.5F, 1F, 1F);
			this.alignment3.Name = "alignment3";
			this.hbox6.Add (this.alignment3);
			global::Gtk.Box.BoxChild w23 = ((global::Gtk.Box.BoxChild)(this.hbox6 [this.alignment3]));
			w23.Position = 2;
			this.vbox2.Add (this.hbox6);
			global::Gtk.Box.BoxChild w24 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.hbox6]));
			w24.Position = 3;
			w24.Expand = false;
			w24.Fill = false;
			this.GtkAlignment2.Add (this.vbox2);
			this.frmMainContact.Add (this.GtkAlignment2);
			this.lblFrameContact = new global::Gtk.Label ();
			this.lblFrameContact.Name = "lblFrameContact";
			this.lblFrameContact.LabelProp = "<b>Contact</b>";
			this.lblFrameContact.UseMarkup = true;
			this.frmMainContact.LabelWidget = this.lblFrameContact;
			this.vbox1.Add (this.frmMainContact);
			global::Gtk.Box.BoxChild w27 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.frmMainContact]));
			w27.Position = 2;
			w27.Expand = false;
			w27.Fill = false;
			// Container child vbox1.Gtk.Box+BoxChild
			this.GtkScrolledWindow = new global::Gtk.ScrolledWindow ();
			this.GtkScrolledWindow.Name = "GtkScrolledWindow";
			this.GtkScrolledWindow.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child GtkScrolledWindow.Gtk.Container+ContainerChild
			this.tvTable = new global::Gtk.TreeView ();
			this.tvTable.CanFocus = true;
			this.tvTable.Name = "tvTable";
			this.GtkScrolledWindow.Add (this.tvTable);
			this.vbox1.Add (this.GtkScrolledWindow);
			global::Gtk.Box.BoxChild w29 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.GtkScrolledWindow]));
			w29.Position = 3;
			// Container child vbox1.Gtk.Box+BoxChild
			this.lblEmptyExplanation = new global::Gtk.Label ();
			this.lblEmptyExplanation.Name = "lblEmptyExplanation";
			this.lblEmptyExplanation.LabelProp = "label1";
			this.vbox1.Add (this.lblEmptyExplanation);
			global::Gtk.Box.BoxChild w30 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.lblEmptyExplanation]));
			w30.Position = 4;
			w30.Expand = false;
			w30.Fill = false;
			// Container child vbox1.Gtk.Box+BoxChild
			this.sbStatus = new global::Gtk.Statusbar ();
			this.sbStatus.Name = "sbStatus";
			this.sbStatus.Spacing = 6;
			this.vbox1.Add (this.sbStatus);
			global::Gtk.Box.BoxChild w31 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.sbStatus]));
			w31.Position = 5;
			w31.Expand = false;
			w31.Fill = false;
			this.Add (this.vbox1);
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.DefaultWidth = 645;
			this.DefaultHeight = 309;
			this.Show ();
			this.DeleteEvent += new global::Gtk.DeleteEventHandler (this.OnDeleteEvent);
			this.quitAction.Activated += new global::System.EventHandler (this.OnQuit);
			this.aboutAction.Activated += new global::System.EventHandler (this.OnAbout);
			this.addAction.Activated += new global::System.EventHandler (this.OnAdd);
			this.removeAction.Activated += new global::System.EventHandler (this.OnRemove);
			this.editAction.Activated += new global::System.EventHandler (this.OnEdit);
			this.addAction1.Activated += new global::System.EventHandler (this.OnAdd);
			this.removeAction1.Activated += new global::System.EventHandler (this.OnRemove);
			this.editAction1.Activated += new global::System.EventHandler (this.OnEdit);
			this.saveAction.Activated += new global::System.EventHandler (this.OnSave);
			this.saveAction1.Activated += new global::System.EventHandler (this.OnSave);
			this.openAction1.Activated += new global::System.EventHandler (this.OnOpen);
			this.propertiesAction.Activated += new global::System.EventHandler (this.OnEditCategories);
			this.findAction.Activated += new global::System.EventHandler (this.OnFind);
			this.openAction2.Activated += new global::System.EventHandler (this.OnOpen);
			this.opConvert.Activated += new global::System.EventHandler (this.OnConvert);
			this.editCategoriesAction.Activated += new global::System.EventHandler (this.OnEditCategories);
			this.opFindAgain.Activated += new global::System.EventHandler (this.OnFindAgain);
			this.ImportAction.Activated += new global::System.EventHandler (this.OnImport);
			this.SortAction.Activated += new global::System.EventHandler (this.OnSort);
			this.connectAction.Activated += new global::System.EventHandler (this.OnConnect);
			this.newAction.Activated += new global::System.EventHandler (this.OnNew);
			this.ContactPanelAction.Activated += new global::System.EventHandler (this.OnViewContactPanel);
			this.cbCategory.Changed += new global::System.EventHandler (this.OnCategoryChanged);
			this.edSearch.FocusInEvent += new global::Gtk.FocusInEventHandler (this.OnFocusEdSearch);
			this.edSearch.FocusOutEvent += new global::Gtk.FocusOutEventHandler (this.OnFocusOutEdSearch);
			this.edSearch.KeyReleaseEvent += new global::Gtk.KeyReleaseEventHandler (this.OnEdSearchKeyReleased);
		}
	}
}
