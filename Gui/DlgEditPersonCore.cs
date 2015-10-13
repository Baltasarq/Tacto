using System;
using Tacto.Core;

using GtkUtil;

namespace Tacto.Gui {
	public partial class DlgEditPerson : Gtk.Dialog {

		private void OnShow()
		{
			this.UpdateFromPerson();
			this.UpdateAll();

			this.nbFiles.Page = 0;
		}

		private void UpdateFromPerson()
		{
			this.edSurname.Text = this.person.Surname;
			this.edName.Text    = this.person.Name;
			this.edEmail.Text   = this.person.Email;
			this.edEmail2.Text  = this.person.Email2;
			this.edWeb.Text 	= this.person.Web;
			this.edWPhone.Text  = this.person.WorkPhone;
			this.edHPhone.Text  = this.person.HomePhone;
			this.edMPhone.Text  = this.person.MobilePhone;
			this.edAddress.Text = this.person.Address;
			this.edId.Text 	    = this.person.Id;
			this.edComments.Text= this.person.Comments;
		}
		
		public void UpdateToPerson()
		{
			this.person.Surname     = this.edSurname.Text;
			this.person.Name        = this.edName.Text;
			this.person.Email       = this.edEmail.Text;
			this.person.Email2      = this.edEmail2.Text;
			this.person.Web		    = this.edWeb.Text;
			this.person.WorkPhone   = this.edWPhone.Text;
			this.person.HomePhone   = this.edHPhone.Text;
			this.person.MobilePhone = this.edMPhone.Text;
			this.person.Address     = this.edAddress.Text;
			this.person.Id          = this.edId.Text;
			this.person.Comments    = this.edComments.Text;
			
			if ( !this.person.IsEnoughData() ) {
				throw new ApplicationException( "Not enough data given: some fields are mandatory." );
			}
			  
		}
		
		private void UpdateAll()
		{
			this.FillAvailableCategories();
			this.FillCurrentCategories();
			this.UpdatePersonCategories();
		}

		private void UpdatePersonCategories()
		{
			string cats = "";
			
			foreach(var category in this.person.Categories) {
				cats += category.Name + ',' + ' ';
			}
			
			if ( cats.Length > 0 ) {
				cats = cats.Substring( 0, cats.Length -2 );
			}
			
			lblCategories.Markup = "<i>Current categories</i>: " + cats;
		}
		
		private void FillAvailableCategories()
		{
			( this.cbAvailableCategories.Model as Gtk.ListStore ).Clear();
			
			foreach(var category in this.categories) {
				this.cbAvailableCategories.AppendText( category.Name );
			}

			this.cbAvailableCategories.Active = 0;
		}
		
		private void FillCurrentCategories()
		{
			( this.cbCurrentCategories.Model as Gtk.ListStore ).Clear();
			
			foreach(var category in this.person.Categories) {
				this.cbCurrentCategories.AppendText( category.Name );
			}

			this.cbCurrentCategories.Active = 0;
		}
		
		private void OnAddCategory()
		{
			int categoryIndex = cbAvailableCategories.Active;
			
			try {
				this.person.Categories = new Category[]{ categories.Get( categoryIndex ) };
				this.FillCurrentCategories();
				this.UpdatePersonCategories();
			} catch(Exception exc)
			{
				Util.MsgError( this, AppInfo.Name, "Category could not be added: " + exc.Message );
			}
		}
		
		private void OnRemoveCategory()
		{
			string categoryText = cbCurrentCategories.ActiveText;
			
			if ( categoryText != null ) {
				try {
					this.person.RemoveCategory( categoryText );
					this.FillCurrentCategories();
					this.UpdatePersonCategories();
				} catch(Exception exc)
				{
					Util.MsgError( this, AppInfo.Name, "Error removing category: " + exc.Message );
				}
			} else Util.MsgError( this, AppInfo.Name, "Please select one category" );
		}
		
		private void OnSaveAs()
		{
			string fileName = "";
			string filter   = "*";
			int op = this.cbOutputFormat.Active;
			
			if ( op >= 0 ) {
				string ext = "." + Person.FormatNames[ op ].ToLower();
				filter += ext;
				
				if ( Util.DlgSave(  AppInfo.Name, "Save as...", this, ref fileName, filter ) )
				{
					if ( !fileName.EndsWith( ext ) ) {
						fileName += ext;
					}
					
					try {
						this.person.Convert( Person.Format.CSV + op, fileName );
					} catch(Exception exc)
					{
						Util.MsgError( this, AppInfo.Name, "Unexpected error saving\n" + exc.Message );
					}
				}
			}

			return;
		}
		
		private void OnConnect(object sender)
		{
			string email = "";

			if ( sender == btConnectEmail ) {
				email = this.edEmail.Text;
			}
			else
			if ( sender == btConnectEmail2 ) {
				email = this.edEmail2.Text;
			}

			try {
				email = email.Trim();
				var mailer = new EmailConnectionManager( email );
				mailer.Open();
			} catch(Exception exc)
			{
				Util.MsgError( this, AppInfo.Name, exc.Message );
			}

			return;
		}

		private void OnConnectWeb()
		{
			try {
				var weber = new HttpConnectionManager( this.edWeb.Text );
				weber.Open();
			} catch(Exception exc)
			{
				Util.MsgError( this, AppInfo.Name, exc.Message );
			}

			return;
		}

		private void OnConnectMap() {
			string url = HttpConnectionManager.UrlGMapsSearch
							+ this.edAddress.Text.Trim().Replace( ' ', '+' );

			try {
				var weber = new HttpConnectionManager( url );
				weber.Open();
			} catch(Exception exc)
			{
				Util.MsgError( this, AppInfo.Name, exc.Message );
			}

			return;
		}

		public string PersonName {
			get { return this.edName.Text; }
		}
		
		public string PersonSurname {
			get { return this.edSurname.Text; }
		}
		
		public string PersonAddress {
			get { return this.edAddress.Text; }
		}
		
		public string Email {
			get { return this.edEmail.Text; }
		}
		
		public string Email2 {
			get { return this.edEmail2.Text; }
		}

		public string Web {
			get { return this.edWeb.Text; }
		}
		
		public string WPhone {
			get { return this.edWPhone.Text; }
		}
		
		public string HPhone {
			get { return this.edHPhone.Text; }
		}
		
		public string MPhone {
			get { return this.edMPhone.Text; }
		}

		public string Id {
			get { return this.edId.Text; }
		}

		public string Comments {
			get { return edComments.Text; }
		}

		private Person person = null;
		private CategoryList categories = null;
	}
}
