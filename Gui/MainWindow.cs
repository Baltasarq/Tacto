// Tacto, a contact management app

using System;
using System.Collections.ObjectModel;

using Tacto.Core;

using GtkUtil;

namespace Tacto.Gui
{
	public partial class MainWindow : Gtk.Window
	{
		public const string EtqNotAvailable = "n/a";
		public const string EtqLastFile = "lastfile";
		public const string EtqWidth    = "width";
		public const string EtqHeight   = "height";
		public const string CfgFileName = ".tacto.cfg";
		
		public static readonly ReadOnlyCollection<string> Headers = new ReadOnlyCollection<string>(
			new string[] { "#", "Surname", "Name", "Email", "Phone" }
		);
		
		public Category CurrentCategory {
			get { 
				Category toret = currentCategory;
				
				if ( toret == null ) {
					toret = agendaSystem.CategoryList.LookFor( Category.EtqCategoryAll );
				}
				
				return toret;
			}
		}
		
		public bool Init {
			get { return init; }
			set { init = value; }
		}
		
		public string CfgFile
		{
			get {
				if ( this.cfgFile.Trim().Length == 0 ) {
					this.cfgFile = Util.GetCfgCompletePath( CfgFileName );
				}
				
				return this.cfgFile;
			}
		}
		
		public MainWindow() : base(Gtk.WindowType.Toplevel)
		{
			// Prepare window
			this.init = true;
			this.Build();
			this.Title = AppInfo.Name;
			this.CreatePopup();
			this.PrepareContactFrame();
			this.eventbox1.ButtonReleaseEvent += this.OnLblEmail1Clicked;
			this.eventbox2.ButtonReleaseEvent += this.OnLblEmail2Clicked;
			this.lblEmptyExplanation.UseMarkup = true;
			
			// Prepare a new document
			this.New();
			
			// Prepare the table for all data
			this.SetStatus( "Ready" );
			this.PrepareTable();
			this.ReadConfiguration();
			
			// If not loaded anything, activate anyway
			if ( this.agendaSystem.PersonsList.Size() == 0 ) {
				this.UpdateCategories();
				this.ActivateGui();
			}
			
			this.init = false;
			this.UpdateContactFrame();
			return;
		}	
		
		protected void PrepareContactFrame()
		{
			// Contact frame labels
			this.lblSurname.ModifyFont(
				Pango.FontDescription.FromString( "Times 18" )
			);
			
			this.lblName.ModifyFont(
				Pango.FontDescription.FromString( "Times 18" )
			);
			
			this.lblPhone.ModifyFont(
				Pango.FontDescription.FromString( "Times 14" )
			);
			
			this.lblPhoneWork.ModifyFont(
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
			
			return;
		}

		
		public void SetStatus()
		{
			sbStatus.Pop( 0 );
		}
		
		public void SetStatus(string statusMsg)
		{
			sbStatus.Push( 0, statusMsg );
			Util.UpdateUI();
		}
		
		protected void InitData(AgendaSystem agenda)
		{
			agendaSystem = agenda;
			persons = agendaSystem.PersonsList;
		}
		
		protected void Quit()
		{
			this.Save();
			this.WriteConfiguration();
            Gtk.Application.Quit();
		}
	
		protected void OnDeleteEvent(object sender, Gtk.DeleteEventArgs a)
		{
			Quit();
			a.RetVal = true;
		}
		
		protected virtual void OnQuit(object sender, System.EventArgs e)
		{
			Quit();
		}
		
		protected virtual void OnAbout(object sender, System.EventArgs e)
		{
			var aboutDlg = new Gtk.AboutDialog();

			aboutDlg.Icon = this.Icon;
			aboutDlg.Logo = this.Icon;
			aboutDlg.ProgramName = AppInfo.Name;
			aboutDlg.Version = AppInfo.Version;
			aboutDlg.Copyright = "(c) " + AppInfo.Author;
			aboutDlg.Authors = new string[]{ AppInfo.Author };
			aboutDlg.Comments = AppInfo.Comments;
			aboutDlg.License = AppInfo.License;
			aboutDlg.Website = AppInfo.Website;
			
			aboutDlg.Parent = this;
			aboutDlg.TransientFor = this;
            aboutDlg.SetPosition( Gtk.WindowPosition.CenterOnParent );
			aboutDlg.Run();
			aboutDlg.Destroy();
		}
		
		protected void PrepareTable()
		{
			SetStatus( "Prepare columns..." );
			
			this.ttvTableView = new TableTextView( this.tvTable, Headers.Count, Headers.Count );
			this.ttvTableView.Headers = Headers;
			 
			SetStatus();
			this.tvTable.ButtonReleaseEvent += OnTableClick;
			this.tvTable.RowActivated += ( sender, evt ) => this.OnEdit( null, null );
			return;
		}
		
		protected virtual void CreatePopup()
		{
			// Menu
			popup = new Gtk.Menu();
			
			// Insert
			var menuItemInsert = new Gtk.ImageMenuItem( "_Insert" );
            menuItemInsert.Image = new Gtk.Image( Gtk.Stock.Add, Gtk.IconSize.Menu );
			menuItemInsert.Activated += delegate { this.OnAdd( null, null ); };
			popup.Append( menuItemInsert );
			
			// Remove
			var menuItemRemove = new Gtk.ImageMenuItem( "_Remove" );
            menuItemRemove.Image = new Gtk.Image( Gtk.Stock.Remove, Gtk.IconSize.Menu );
			menuItemRemove.Activated += delegate { this.OnRemove( null, null ); };
			popup.Append( menuItemRemove );
			
			// Edit
			var menuItemEdit = new Gtk.ImageMenuItem( "_Edit" );
            menuItemEdit.Image = new Gtk.Image( Gtk.Stock.Clear, Gtk.IconSize.Menu );
			menuItemEdit.Activated += (sender, evt) => this.OnEdit( null, null );
			popup.Append( menuItemEdit );

			// Categories
			popup.Append( new Gtk.SeparatorMenuItem() );
			var menuItemProperties = new Gtk.ImageMenuItem( "_Properties" );
            menuItemProperties.Image = new Gtk.Image( Gtk.Stock.Properties, Gtk.IconSize.Menu );
			menuItemProperties.Activated += delegate { this.OnEditCategories( null, null ); };
			popup.Append( menuItemProperties );
			
			// Finish
			popup.ShowAll();
		}
		
		protected virtual void OnTableClick(object o, Gtk.ButtonReleaseEventArgs args)
		{
			this.UpdateContactFrame();
			
			if ( args.Event.Button == 3 ) {
				popup.Popup();
			}	 				
		}
		
		protected void UpdateCategories()
		{
			// Update the cbCategory combo
			Init = true;
			
			// remove items
            cbCategory.Model = new Gtk.ListStore( typeof(string) );
			
			// add items
			foreach(var category in agendaSystem.CategoryList) {
				cbCategory.AppendText( category.Name );
			}
			cbCategory.Active = 0;
			cbCategory.Show();

			Init = false;
			SetStatus();
		}

		protected void UpdatePersons()
		{
			SetStatus( "Listing persons..." );

			// Prepare table
			this.tvTable.Hide();
			this.ttvTableView.RemoveAllRows();
			
			if ( this.persons.Size() > 0 )
			{
				this.lblEmptyExplanation.Hide();
				
				// Update persons table
				for(int i = 0; i < persons.Size(); ++i) {
					var p = persons.Get( i );
					
					// Update each row
					this.ttvTableView.AppendRow();
					this.ttvTableView.Set( i, 1, p.Surname );
					this.ttvTableView.Set( i, 2, p.Name );
					this.ttvTableView.Set( i, 3, p.AnyEmail );
					this.ttvTableView.Set( i, 4, p.AnyPhone );
				}
				
				this.tvTable.Show();
				this.tvTable.GrabFocus();
			} else {
				// Show label explanation: the list is empty...
				if ( this.agendaSystem.PersonsList.Size() == 0 ) {
					this.lblEmptyExplanation.Markup =
						"<b>The contact list is now empty. You can add "
						+ " new contacts at any time,"
						+ " as well as categories, using the <i>Edit</i> menu.</b>"
					;
				} else {
					this.lblEmptyExplanation.Markup =
						"<b>The contact list is now empty, since there are no contacts "
						+ "in this category. Change to another category using the category "
						+ "list in the toolbar.</b>"
					;
				}
				this.lblEmptyExplanation.Show();
			}
			
			this.SetStatus();
			this.UpdateContactFrame();
			return;
		}
		
		protected void UpdateInfo()
		{
			this.UpdateCategories();
			this.UpdatePersons();
			this.UpdateContactFrame();
		}
		
		protected virtual void OnAdd(object sender, System.EventArgs e)
		{
			Person p = new Person();
			
			// Add category to the person
			if ( persons != agendaSystem.PersonsList ) {
				p.Categories = new Category[]{ CurrentCategory };
			}
			
			// Edit it
			this.InsertPerson( p );
			this.Edit( agendaSystem.PersonsList.Find( p ) );
			this.ActivateGui();
			this.UpdatePersons();
			this.SetCurrentPositionInDocument( this.persons.Find( p ), 0 );
		}
		
		protected virtual void OnRemove(object sender, System.EventArgs e)
		{
			int row;
			int column;
			
			GetCurrentPositionInDocument( out row, out column );
			
			Person p = persons.Get( row );
			
			if ( Util.Ask( this, AppInfo.Name, "Erase: " + p.ToString() + "?" ) ) {
				this.ErasePerson( p );
				this.UpdatePersons();
				this.ActivateGui();
				row = Math.Min( persons.Size() -1, row );
				this.SetCurrentPositionInDocument( row, 0 );
			}
		}
		
		protected void Edit(int pos)
		{
			DlgEditPerson dlg = null;
			Person p = null;
			Person backupPerson = null;
			
			if ( pos >= 0
			  && pos < this.agendaSystem.PersonsList.Size() )
			{
				try {
					// Get the person and eliminate it of all lists
					p = this.agendaSystem.PersonsList.Get( pos );
					backupPerson = p.Clone();
					this.ErasePerson( p );
					
					// Edit the person data
					dlg = new DlgEditPerson( this, agendaSystem.CategoryList, p );
					
                    if ( ( (Gtk.ResponseType) dlg.Run() ) == Gtk.ResponseType.Ok ) {
						dlg.UpdateToPerson();
						this.InsertPerson( p );
					} else {
						this.InsertPerson( backupPerson );
						p = backupPerson;
					}
				} catch(Exception exc)
				{
					Util.MsgError( this, AppInfo.Name, exc.Message );
				}
				finally {
					int rowIndex = this.persons.Find( p );

					if ( dlg != null ) {
						dlg.Destroy();
					}

					if ( rowIndex < 0 ) {
						p = backupPerson;
						this.InsertPerson( p );
						rowIndex = this.persons.Find( p );
					}

					this.UpdatePersons();
					this.SetCurrentPositionInDocument( rowIndex, 0 );
				}
			} else {
				Util.MsgError( this, AppInfo.Name, "person position out of bounds" );
			}
			
			return;
		}
		
		/// <summary>
		/// Erases a person.
		/// This is strongly needed, since there are always two lists, the main one
		/// and the one honoring the category selected.
		/// </summary>
		/// <param name='p'>
		/// The Person object to remove
		/// </param>
		protected void ErasePerson(Person p)
		{
			this.persons.Remove( this.persons.Find( p ) );
			
			if ( this.persons != this.agendaSystem.PersonsList ) {
				this.agendaSystem.PersonsList.Remove( this.agendaSystem.PersonsList.Find( p ) );
			}
		}
		
		/// <summary>
		/// Inserts a person.
		/// This is strongly needed, since there are always two lists, the main one
		/// and the one honoring the category selected.
		/// </summary>
		/// <param name='p'>
		/// The Person object to insert.
		/// </param>
		protected void InsertPerson(Person p)
		{
			this.persons.Insert( p );
			
			if ( this.persons != this.agendaSystem.PersonsList ) {
				this.agendaSystem.PersonsList.Insert( p );
			}
		}
		
		protected virtual void OnEdit(object sender, EventArgs evt)
		{
			int row;
			int column;
			
			GetCurrentPositionInDocument( out row, out column );
			
			// Lookup in the main list of persons, since it is going to be changed.
			this.Edit( this.agendaSystem.PersonsList.Find( this.persons.Get( row ) ) );
		}

		public void SetCurrentPositionInDocument(int row, int col)
		{
			if ( this.persons.Size() > 0 ) {
				this.ttvTableView.SetCurrentCell( row, col +1 );
				this.UpdateContactFrame();
			}
		}
		
		public void GetCurrentPositionInDocument(out int row, out int col)
		{
			this.ttvTableView.GetCurrentCell( out row, out col );
			
			// Adapt column from UI
			--col;
			if ( col < 0 ) {
				col = 0;
			}
		}
		
		protected bool AskForFileName()
		{
			const string FileExt = "." + AppInfo.FileExtension;
			
			// Ask for file name
			bool toret = Util.DlgSave( "", AppInfo.Name + " Save", 
					                   this, ref this.fileName,
					                   AppInfo.FileExtension
			);
			
			// Chk file name
			if ( this.fileName == null ) {
				this.fileName = "";
			}
			
			this.fileName = this.fileName.Trim();
			
			// Fix name
			if ( this.fileName.Length == 0 ) {
				toret = false;
			} else {
				if ( !this.fileName.EndsWith( FileExt ) ) {
					this.fileName += FileExt;
				}
			}
			
			return toret;
		}
		
		protected void Save()
		{
			this.SetStatus( "Saving..." );	
			fileName = agendaSystem.FileName;
			
			try {
				
				if ( !agendaSystem.IsFileNameSet() ) {
					// Ask for name
					if ( !this.AskForFileName() ) {
						goto End;
					}
					
					// Set file name
					this.agendaSystem.FileName = fileName;
				}
				
				this.agendaSystem.Save();
			} catch(Exception) {
				Util.MsgError( this, AppInfo.Name, "Unexpected error while saving" );
			}
			
			End:
			this.SetStatus();
			return;
		}
		
		protected virtual void OnSave(object sender, System.EventArgs e)
		{
			Save();
		}
		
		protected virtual void Open(string file)
		{
			try {
				if ( file.Length > 0 ) {
					this.InitData( AgendaSystem.Load( file ) );
				}
			} catch(Exception)
			{
				Util.MsgError( this, AppInfo.Name, "Unexpected error loading:\n'" + file + '\'' );
				this.New();
			}
			finally {
				this.UpdateInfo();
			}
		}
		
		protected virtual void OnOpen(object sender, System.EventArgs e)
		{
			try {
				// Save
				this.Save();
				
				// Open file
				if ( Util.DlgOpen( "", AppInfo.Name + " Load", 
						                   this, ref fileName,
						                   "*." + AppInfo.FileExtension ) )
				{
					this.Open( fileName.Trim() );
				} else {
					this.fileName = "";
				}
			} catch(Exception exc)
			{
				Util.MsgError( this, AppInfo.Name, "Unexpected error loading.\n" + exc.Message );
			}
		}
		
		protected virtual void OnEditCategories(object sender, System.EventArgs e)
		{
			DlgCategories dlg = null;
			
			try {
				dlg = new DlgCategories( this, this.agendaSystem );
				dlg.Run();
				
				this.UpdateCategories();
				
				if ( dlg.CategoryRemoved ) {
					this.SetCategoryAsFilter( null );
					this.UpdatePersons();
				}
				
				dlg.Destroy();
			}
			catch(Exception exc) {
				Util.MsgInfo( this, AppInfo.Name, exc.Message );
			}
			finally {
				if ( dlg != null ) {
					dlg.Destroy();
				}
			}
		}

		protected virtual void OnCategoryChanged(object sender, System.EventArgs e)
		{
			if ( !Init ) {
				int categoryNumber = cbCategory.Active;
					
				if ( categoryNumber >= 0 ) {
					this.SetCategoryAsFilter( agendaSystem.CategoryList.Get( categoryNumber ) );
					this.UpdatePersons();
					this.UpdateContactFrame();
				}
			}
		}
		
		protected void SetCategoryAsFilter(Category category)
		{
			try {
				if ( category != null )
						this.currentCategory = category;
				else 	this.currentCategory = agendaSystem.CategoryList.LookFor( Category.EtqCategoryAll );
									
				if ( CategoryList.IsCategoryAll( CurrentCategory ) ) {
					this.persons = agendaSystem.PersonsList;
				} else {
					persons = new PersonsList();
					
					foreach(var p in agendaSystem.PersonsList) {
						if ( p.HasCategory( CurrentCategory ) ) {
							persons.Insert( p );
						}
					}
				}
			} catch(Exception exc)
			{
				Util.MsgError( this, AppInfo.Name, exc.Message );
			}
		}

		protected virtual void OnConvert(object sender, System.EventArgs e)
		{
			var fmt = Person.Format.CSV;
			var fileName = "";
			var chfmt = new ChooseConversionFormat( this );
			chfmt.Parent = this;
			chfmt.TransientFor = this;
            chfmt.SetPosition( Gtk.WindowPosition.CenterOnParent );
			var result = (Gtk.ResponseType) chfmt.Run();
			
			chfmt.Hide();
			fmt = chfmt.Format;
			fileName = chfmt.FileName;
			chfmt.Destroy();
			
			if ( result != Gtk.ResponseType.Cancel ) {
				Convert( fmt, fileName );
			}
		}
		
		public void Find(string text, int row)
		{
			int size = persons.Size();
			
			SetStatus( "Searching..." );
			
			// set search cache
			searchString = text.Trim().ToLower();
			searchRow = -1;
			
			if ( size < row ) {
				row = 0;
			}
			
			// look for text
			for(int i = row; i < size; ++i) {
				if ( persons.Get( i ).LookFor( searchString ) ) {
					searchRow = i;
					break;
				}
			}
			
			// Move to the person if found
			if ( searchRow >= 0 )
					SetCurrentPositionInDocument( searchRow, 1 );
			else {
				searchRow = 0;
				Util.MsgInfo( this, AppInfo.Name, "'" + searchString + "' was not found" );
			}
			
			this.SetStatus();
			this.tvTable.GrabFocus();
			this.UpdateContactFrame();
			return;
		}
		
		public void Convert(Person.Format fmt, string fileName)
		{
			SetStatus( "Converting..." );
			
			try {
				var manager = FileFormatManager.CreateManager( fmt, fileName );
				
				manager.Export( persons );

				Util.MsgInfo( this, AppInfo.Name, "File generated" );
			} catch(Exception exc)
			{
				Util.MsgError( this, AppInfo.Name, "Unexpected error while converting:\n" + exc.Message );
			}
			
			SetStatus();
		}
		
		protected void ResetSearchField()
		{
            edSearch.ModifyText( Gtk.StateType.Normal, new Gdk.Color( 0xa9, 0xa9, 0xa9 ) );
			edSearch.Text = "Search...";
		}
		
		protected virtual void OnFindAgain(object sender, System.EventArgs e)
		{
			Find( searchString, searchRow +1 );
		}
		
		protected virtual void OnFind(object sender, System.EventArgs e)
		{
			this.edSearch.GrabFocus();
		}
		
		protected virtual void OnFocusEdSearch(object o, Gtk.FocusInEventArgs args)
		{
			this.edSearch.Text = "";
            this.edSearch.ModifyText( Gtk.StateType.Normal, new Gdk.Color( 0, 0, 0 ) );
		}
		
		private void TriggerSearch()
		{
			int row;
			int column;
			
			if ( this.persons.Size() > 0 ) {
				this.GetCurrentPositionInDocument( out row, out column );
				Find( edSearch.Text, row );
			}
			
			return;
		}
		
		protected virtual void OnListRowActivated(object o, Gtk.RowActivatedArgs args)
		{
			this.OnEdit( null, null );
		}
		
		protected void Import(string fileName, Person.Format fmt)
		{
			PersonsList pl = null;
			
			// Import contacts
			if ( fmt == Person.Format.CSV ) {
				pl = PersonsList.ImportCsv( fileName );
			}
			else
			if ( fmt == Person.Format.VCF ) {
				pl = PersonsList.ImportVcf( fileName );
			}
			else throw new ApplicationException( "Sorry, cannot import from HTML" );
				
			// Add new contacts
			foreach( var p in pl) {
				agendaSystem.PersonsList.Insert( p );
				
			}
			
			// Finish
			persons = agendaSystem.PersonsList;
			cbCategory.Active = 0;
			UpdatePersons();
			Util.MsgInfo( this, AppInfo.Name, "Added " + pl.Size() + " persons" );

		}
		
		protected virtual void OnImport(object sender, System.EventArgs e)
		{
			SetStatus( "Importing..." );
			
			try {
				var fmt = Person.Format.CSV;
				var fileName = "";
				var chfmt = new ChooseConversionFormat( this );
				chfmt.Parent = this;
				chfmt.TransientFor = this;
                chfmt.SetPosition( Gtk.WindowPosition.CenterOnParent );
				var result = (Gtk.ResponseType) chfmt.Run();
				
				chfmt.Hide();
				fmt = chfmt.Format;
				fileName = chfmt.FileName;
				chfmt.Destroy();
				
				if ( result != Gtk.ResponseType.Cancel ) {
					Import( fileName, fmt );
				}
			} catch(Exception exc)
			{
				Util.MsgError( this, AppInfo.Name, exc.Message );
			}
			
			SetStatus();
		}
		
		protected void Send(String addr)
		{
			try {
				// Open message
				new EmailConnectionManager( addr ).Open();
			} catch(Exception exc)
			{
				Util.MsgError( this, AppInfo.Name, exc.Message );
			}
			
			return;
		}
		
		protected void Sort()
		{
			try {
				SetStatus( "Sorting..." );

				this.agendaSystem.Sort();
				persons = agendaSystem.PersonsList;
				this.UpdatePersons();
				this.cbCategory.Active = 0;
			} catch(Exception exc) {
				Util.MsgError( this, AppInfo.Name, "Error while sorting\n" + exc.Message );
			}
			
			SetStatus();
			return;
		}
		
		protected void New()
		{
			// New document
			this.InitData( new AgendaSystem() );
			this.ResetSearchField();
		}
		
		protected virtual void OnSort(object sender, System.EventArgs e)
		{
			Sort();
		}
		
		protected virtual void OnConnect(object sender, System.EventArgs e)
		{
			int row;
			int col;
			
			if ( persons.Size() > 0 ) {
				// Get person's email
				this.GetCurrentPositionInDocument( out row, out col );
				var p = persons.Get( row );
				
				this.Send( p.AnyEmail );
			}
			
			return;
		}
		
		protected virtual void OnNew(object sender, System.EventArgs e)
		{
			// Save current document
			this.Save();
			
			// Create the new document
			this.New();
			
			// Prepare the table for all data
			this.UpdateInfo();
		}
		
		protected void ActivateGui()
		{
			bool active = ( this.persons.Size() > 0 );
			
			// Toolbar buttons
			removeAction.Sensitive  = active;
			editAction.Sensitive    = active;
			connectAction.Sensitive = active;
			
			// Menu options
			removeAction1.Sensitive = active;
			editAction1.Sensitive   = active;
			findAction.Sensitive    = active;
			opFindAgain.Sensitive   = active;
			
			// Menus
			ToolsAction.Sensitive   = active;
		}
		
		public void WriteConfiguration()
		{
			// Is the document linked to a file?
			if ( this.agendaSystem != null
			  && this.agendaSystem.IsFileNameSet() )
			{
				this.SetStatus( "Saving configuration..." );
				
				Util.WriteConfiguration( this, this.CfgFile, new string[]{ fileName } );
				
				this.SetStatus();
			}
			
			return;
		}
		
		public void ReadConfiguration()
		{
			this.SetStatus( "Loading configuration..." );
			
			try {
				var lastFiles = Util.ReadConfiguration( this, this.CfgFile );
				
				this.fileName = "";
				if ( lastFiles != null ) {
					this.fileName = lastFiles[ 0 ].Trim();
					this.Open( fileName );
				}
			} catch(Exception)
			{
				string errorMessage = "Error loading previous ";
				
				if ( fileName.Length > 0 ) {
					errorMessage += "contact list file:\n'" + fileName + '\'';
				} else {
					errorMessage += "configuration";
				}
				
				Util.MsgError( this, AppInfo.Name, errorMessage );
			}
			finally {
				this.SetStatus();
			}
		}
		
		protected virtual void OnEdSearchKeyReleased(object o, Gtk.KeyReleaseEventArgs args)
		{
			if ( args.Event.Key == Gdk.Key.KP_Enter
			  || args.Event.Key == Gdk.Key.Return )
			{
				this.TriggerSearch();
			}
			
			return;
		}
		
		protected virtual void OnFocusOutEdSearch(object o, Gtk.FocusOutEventArgs args)
		{
			this.ResetSearchField();
		}
		
		protected void UpdateContactFrame()
		{
			int row;
			int column;
			
			// Initialise labels
			this.lblMobilePhone.Markup = EtqNotAvailable;
			this.lblPhoneWork.Markup = EtqNotAvailable;
			this.lblAddress.Text = EtqNotAvailable;
			this.lblEmail2.Text = EtqNotAvailable;
			this.lblEmail.Text = EtqNotAvailable;
			this.lblPhone.Text = EtqNotAvailable;
			
			if ( this.persons.Size() < 1 ) {
				
				this.lblSurname.Markup = EtqNotAvailable;
				this.lblName.Markup = EtqNotAvailable;
				this.frmMainContact.Hide();
				
			} else {
				this.frmMainContact.Show();
			
				// Get person
				GetCurrentPositionInDocument( out row, out column );
				
				Person p = this.persons.Get( row );
				
				// Fill labels
				this.lblSurname.Markup = "<b>" + p.Surname + "</b>,";
				this.lblName.Markup = p.Name;
				
				if ( p.Email.Trim().Length > 0 ) {
					this.lblEmail.Markup = "<span foreground=\"blue\"><u>" + p.Email + "</u></span>";
				}
				
				if ( p.HomePhone.Trim().Length > 0 ) {
					this.lblPhone.Markup = p.HomePhone;
				}
				
				if ( p.Address.Trim().Length > 0 ) {
					this.lblAddress.Text = p.Address;
				}
				
				if ( p.MobilePhone.Trim().Length > 0 ) {
					this.lblMobilePhone.Markup = p.MobilePhone;
				}
				
				if ( p.WorkPhone.Trim().Length > 0
				  && p.WorkPhone.Trim() != "0" )
				{
					this.lblPhoneWork.Markup = p.WorkPhone;
				}
				
				if ( p.Email2.Trim().Length > 0 ) {
					this.lblEmail2.Markup = "<span foreground=\"blue\"><u>" + p.Email2 + "</u></span>";
				}
				
				this.lblEmail.ModifyText( Gtk.StateType.Normal, new Gdk.Color( 0xae, 0xae, 0xae ) );
			}
			
			return;
		}
		
		protected void OnViewContactPanel (object sender, System.EventArgs e)
		{
			this.frmMainContact.Visible = this.ContactPanelAction.Active;
			
			return;
		}
		
		[GLib.ConnectBefore]
		protected void OnLblEmail1Clicked (object o, Gtk.ButtonReleaseEventArgs args)
		{
			this.Send( this.lblEmail.Text );
			return;
		}
		
		[GLib.ConnectBefore]
		protected void OnLblEmail2Clicked (object o, Gtk.ButtonReleaseEventArgs args)
		{
			this.Send( this.lblEmail2.Text );
			return;
		}
		
		GtkUtil.TableTextView ttvTableView= null;
		protected string searchString = "";
		protected int searchRow = 0;
		protected string fileName = "";
		protected string cfgFile = "";
		protected Boolean init = true;
		
		/// <summary>
		/// The agenda system.
		/// </summary> 
		protected AgendaSystem agendaSystem;
		
		/// <summary>
		/// The current category.
		/// </summary>
		protected Category currentCategory = null;
		
		/// <summary>
		/// persons is an attribute carrying out the list of persons
		/// currently active (honoring the category selected,
		/// currentCategory).
		/// </summary>
		protected PersonsList persons = null;
		
		protected Gtk.Menu popup = null;
		protected void OnNormalizeTextActionActivatedizeTextActionActivated(object sender, EventArgs e)
		{
			throw new NotImplementedException();
		}
	}
}
