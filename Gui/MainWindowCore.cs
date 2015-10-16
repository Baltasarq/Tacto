// Tacto, a contact management app

using System;
using System.Collections.ObjectModel;

using Tacto.Core;

using GtkUtil;

namespace Tacto.Gui
{
	public partial class MainWindow: Gtk.Window
	{
		public const string EtqNotAvailable = "n/a";
		public const string EtqLastFile = "lastfile";
		public const string EtqWidth    = "width";
		public const string EtqHeight   = "height";
		public const string CfgFileName = ".tacto.cfg";
		
		public static readonly ReadOnlyCollection<string> Headers = new ReadOnlyCollection<string>(
			new string[] { "#", "Surname", "Name", "Email", "Phone" }
		);
		
		public void OnShow()
		{
			this.searchString = "";
			this.searchRow = 0;
			this.fileName = "";
			this.cfgFile = "";
	
			this.New();
			this.ReadConfiguration();
			
			this.UpdateCategories();
			this.UpdateCard();
			this.ActivateGui();
			this.SetStatus( "Ready" );
			this.Init = false;
			return;
		}	
		
		public void SetStatus()
		{
			this.sbStatus.Pop( 0 );
			this.sbStatus.Push( 0, "Ready" );
		}
		
		public void SetStatus(string statusMsg)
		{
			this.sbStatus.Push( 0, statusMsg );
			Util.UpdateUI();
		}
		
		private void InitData(AgendaSystem agenda)
		{
			this.agendaSystem = agenda;
			this.persons = agendaSystem.PersonsList;
		}
		
		private void Quit()
		{
			this.Save();
			this.WriteConfiguration();
            Gtk.Application.Quit();
		}
	
		private void OnAbout()
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
		
		private void OnTableClick(object o, Gtk.ButtonReleaseEventArgs args)
		{
			this.UpdateCard();
			
			if ( args.Event.Button == 3 ) {
				this.popup.Popup();
			}	 				
		}
		
		private void UpdateCategories()
		{
			// Update the cbCategory combo
			this.Init = true;
			
			// remove items
			this.cbCategory.Model = new Gtk.ListStore( typeof(string) );
			
			// add items
			foreach(var category in this.agendaSystem.CategoryList) {
				this.cbCategory.AppendText( category.Name );
			}
			this.cbCategory.Active = 0;
			this.cbCategory.Show();

			this.Init = false;
			this.SetStatus();
		}

		private void UpdatePersons()
		{
			this.SetStatus( "Listing persons..." );

			// Prepare table
			this.tvTable.Hide();
			this.ttTable.RemoveAllRows();
			
			if ( this.persons.Size() > 0 )
			{
				this.lblEmptyExplanation.Hide();
				
				// Update persons table
				for(int i = 0; i < this.persons.Size(); ++i) {
					var p = this.persons.Get( i );
					
					// Update each row
					this.ttTable.AppendRow();
					this.ttTable.Set( i, 1, p.Surname );
					this.ttTable.Set( i, 2, p.Name );
					this.ttTable.Set( i, 3, p.AnyEmail );
					this.ttTable.Set( i, 4, p.AnyPhone );
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

			this.lblNumRecords.Text = this.persons.Size().ToString() + " pax";
			this.SetStatus();
			this.UpdateCard();
			return;
		}
		
		private void UpdateInfo()
		{
			this.UpdateCategories();
			this.UpdatePersons();
			this.UpdateCard();
		}
		
		private void OnAdd()
		{
			Person p = new Person();
			
			// Add category to the person
			if ( this.persons != agendaSystem.PersonsList ) {
				p.Categories = new Category[]{ CurrentCategory };
			}
			
			// Edit it
			this.InsertPerson( p );
			this.Edit( agendaSystem.PersonsList.Find( p ) );
			this.ActivateGui();
			this.UpdatePersons();

			int rowIndex = this.persons.Find( p );
			if ( rowIndex >= 0 ) {
				this.SetCurrentPositionInDocument( rowIndex, 1 );
			}

			return;
		}
		
		private void OnRemove()
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
		
		private void Edit(int pos)
		{
			DlgEditPerson dlg = null;
			Person p = null;
			Person backupPerson = null;
			
			if ( pos >= 0
			  && pos < this.agendaSystem.PersonsList.Size() )
			{
				this.SetStatus( "Editing contact..." );

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
					this.SetStatus();
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
		private void ErasePerson(Person p)
		{
			this.SetStatus( "Removing contact..." );
			this.persons.Remove( this.persons.Find( p ) );
			
			if ( this.persons != this.agendaSystem.PersonsList ) {
				this.agendaSystem.PersonsList.Remove( this.agendaSystem.PersonsList.Find( p ) );
			}

			this.SetStatus();
		}
		
		/// <summary>
		/// Inserts a person.
		/// This is strongly needed, since there are always two lists, the main one
		/// and the one honoring the category selected.
		/// </summary>
		/// <param name='p'>
		/// The Person object to insert.
		/// </param>
		private void InsertPerson(Person p)
		{
			this.persons.Insert( p );
			
			if ( this.persons != this.agendaSystem.PersonsList ) {
				this.agendaSystem.PersonsList.Insert( p );
			}
		}
		
		private void OnEdit()
		{
			int row;
			int column;

			if ( this.persons.Size() > 0 ) {
				this.GetCurrentPositionInDocument( out row, out column );
				
				// Lookup in the main list of persons, since it is going to be changed.
				this.Edit( this.agendaSystem.PersonsList.Find( this.persons.Get( row ) ) );
			}

			return;
		}

		public void SetCurrentPositionInDocument(int row, int col)
		{
			if ( this.persons.Size() > 0 ) {
				this.ttTable.SetCurrentCell( row, col +1 );
				this.UpdateCard();
			}
		}
		
		public void GetCurrentPositionInDocument(out int row, out int col)
		{
			this.ttTable.GetCurrentCell( out row, out col );
			
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
		
		private void Save()
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
		
		private void Open(string file)
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
		
		private void OnOpen()
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
		
		private void OnSettings()
		{
			DlgSettings dlg = null;
			
			try {
				dlg = new DlgSettings( this, this.agendaSystem );
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

		private void OnCategoryChanged()
		{
			if ( !Init ) {
				int categoryNumber = cbCategory.Active;
					
				if ( categoryNumber >= 0 ) {
					this.SetCategoryAsFilter( agendaSystem.CategoryList.Get( categoryNumber ) );
					this.UpdatePersons();
					this.UpdateCard();
				}
			}
		}
		
		private void SetCategoryAsFilter(Category category)
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

		private void OnExport()
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
			
			this.SetStatus( "Searching..." );
			
			// Set search cache
			this.searchString = text.Trim().ToLower();
			this.searchRow = -1;
			
			if ( size < row ) {
				row = 0;
			}
			
			// look for text
			for(int i = row; i < size; ++i) {
				if ( this.persons.Get( i ).LookFor( this.searchString ) ) {
					this.searchRow = i;
					break;
				}
			}
			
			// Move to the person if found
			if ( this.searchRow >= 0 )
				this.SetCurrentPositionInDocument( searchRow, 1 );
			else {
				this.searchRow = 0;
				Util.MsgInfo( this, AppInfo.Name, "'" + searchString + "' was not found" );
			}
			
			this.SetStatus();
			this.tvTable.GrabFocus();
			this.UpdateCard();
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
		
		private void ResetSearchField()
		{
            this.edSearch.ModifyText( Gtk.StateType.Normal, new Gdk.Color( 0xa9, 0xa9, 0xa9 ) );
			this.edSearch.Text = "Search...";
		}
		
		private void OnFindAgain()
		{
			this.Find( this.searchString, searchRow +1 );
		}
		
		private void OnFind()
		{
			this.edSearch.GrabFocus();
		}
		
		private void OnFocusEdSearch()
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
				this.Find( edSearch.Text, row );
			}
			
			return;
		}
		
		private void Import(string fileName, Person.Format fmt)
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
			this.persons = this.agendaSystem.PersonsList;
			this.cbCategory.Active = 0;
			this.UpdatePersons();
			Util.MsgInfo( this, AppInfo.Name, "Added " + pl.Size() + " persons" );

		}
		
		private void OnImport()
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
					this.Import( fileName, fmt );
				}
			} catch(Exception exc)
			{
				Util.MsgError( this, AppInfo.Name, exc.Message );
			}
			
			this.SetStatus();
		}
		
		private void Send(String addr)
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

		private void OpenMap(String address)
		{
			try {
				// Open message
				new HttpConnectionManager( HttpConnectionManager.UrlGMapsSearch + address ).Open();
			} catch(Exception exc)
			{
				Util.MsgError( this, AppInfo.Name, exc.Message );
			}

			return;
		}
		
		private void Sort()
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
		
		private void New()
		{
			// New document
			this.InitData( new AgendaSystem() );
			this.ResetSearchField();
		}
		
		private void OnConnect()
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
		
		private void OnNew()
		{
			// Save current document
			this.Save();
			
			// Create the new document
			this.New();
			
			// Prepare the table for all data
			this.UpdateInfo();
		}
		
		private void ActivateGui()
		{
			bool active = ( this.persons.Size() > 0 );
			
			this.actRemove.Sensitive  = active;
			this.actModify.Sensitive    = active;
			this.actConnect.Sensitive = active;
			this.actFind.Sensitive = active;
			this.actFindAgain.Sensitive = active;
			this.actSort.Sensitive = active;
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
		
		private void OnEdSearchKeyReleased(object o, Gtk.KeyReleaseEventArgs args)
		{
			if ( args.Event.Key == Gdk.Key.KP_Enter
			  || args.Event.Key == Gdk.Key.Return )
			{
				this.TriggerSearch();
			}
			
			return;
		}
		
		private void OnFocusOutEdSearch()
		{
			this.ResetSearchField();
		}
		
		private void UpdateCard()
		{
			int row;
			int column;
			
			// Initialise labels
			this.lblMobilePhone.Markup = EtqNotAvailable;
			this.lblHomePhone.Markup = EtqNotAvailable;
			this.lblWorkPhone.Markup = EtqNotAvailable;
			this.lblAddress.Text = EtqNotAvailable;
			this.lblEmail2.Text = EtqNotAvailable;
			this.lblEmail.Text = EtqNotAvailable;

			if ( this.persons.Size() < 1 ) {
				
				this.lblSurname.Markup = EtqNotAvailable;
				this.lblName.Markup = EtqNotAvailable;
				this.frmCard.Hide();
				
			} else {
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
					this.lblHomePhone.Markup = p.HomePhone;
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
					this.lblWorkPhone.Markup = p.WorkPhone;
				}

				if ( p.Email2.Trim().Length > 0 ) {
					this.lblEmail2.Markup = "<span foreground=\"blue\"><u>" + p.Email2 + "</u></span>";
				}
				
				this.lblEmail.ModifyText( Gtk.StateType.Normal, new Gdk.Color( 0xae, 0xae, 0xae ) );
			}
			
			return;
		}
		
		private void OnViewCard()
		{
			this.frmCard.Visible = !this.frmCard.Visible;
		}

		[GLib.ConnectBefore]
		private void OnAddressClicked()
		{
			this.OpenMap( this.lblAddress.Text );
			return;
		}
		
		[GLib.ConnectBefore]
		private void OnLblEmail1Clicked()
		{
			this.Send( this.lblEmail.Text );
			return;
		}
		
		[GLib.ConnectBefore]
		private void OnLblEmail2Clicked()
		{
			this.Send( this.lblEmail2.Text );
			return;
		}

		public Category CurrentCategory {
			get { 
				Category toret = currentCategory;

				if ( toret == null ) {
					toret = agendaSystem.CategoryList.LookFor( Category.EtqCategoryAll );
				}

				return toret;
			}
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
		
		GtkUtil.TableTextView ttTable;
		protected string searchString;
		protected int searchRow;
		protected string fileName;
		protected string cfgFile;

		/// <summary>
		/// The agenda system.
		/// </summary> 
		protected AgendaSystem agendaSystem;
		
		/// <summary>
		/// The current category.
		/// </summary>
		protected Category currentCategory;
		
		/// <summary>
		/// persons is an attribute carrying out the list of persons
		/// currently active (honoring the category selected,
		/// currentCategory).
		/// </summary>
		protected PersonsList persons;
	}
}
