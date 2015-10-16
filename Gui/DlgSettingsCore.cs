using System;
using Tacto.Core;
using Gtk;

using GtkUtil;

namespace Tacto.Gui {
	public partial class DlgSettings: Gtk.Dialog {
		public static string Header = "Category";

		public void OnShow()
		{
			this.PrepareTable();
			this.Update();
		}
		
		protected void PrepareTable()
		{
			// Create liststore
			var listStore = new Gtk.ListStore( typeof(string) );
			this.tvCategories.Model = listStore;
			
			// Create columns belonging to the document
			var column = new Gtk.TreeViewColumn();
			var cell = new Gtk.CellRendererText();
			column.Title = Header;
			column.PackStart( cell, true );
			cell.Editable = true;
			cell.Edited += OnEdited;
			column.AddAttribute( cell, "text", 0 );
			this.tvCategories.AppendColumn( column );
		}
		
		protected void Update()
		{
			var model = ( (ListStore) this.tvCategories.Model );
			
			model.Clear();
			foreach(var category in this.agendaSystem.CategoryList) {
				model.AppendValues( new string[] { category.Name } );
			}
		}
		
		protected void OnEdited(object sender, Gtk.EditedArgs args)
		{
			int rowIndex = 0;
		
			// Get current position
			TreePath rowPath;
			TreeIter rowPointer;
	
			// Convert path in row and rowPointer
			rowPath = new Gtk.TreePath( args.Path );
			this.tvCategories.Model.GetIter( out rowPointer, rowPath );
			rowIndex = rowPath.Indices[ 0 ];
			
			try {
				this.agendaSystem.CategoryList.Modify( rowIndex, args.NewText );
				this.tvCategories.Model.SetValue( rowPointer, 0, args.NewText );
			} catch(Exception exc)
			{
				Util.MsgError( this, AppInfo.Name, exc.Message );
				args.RetVal = false;
			}
		}
		
		public int GetCurrentRow()
		{
			int row = 0;
			TreePath rowPath;
			TreeIter rowPointer;
			TreeViewColumn colPath;
		
			// Convert path in row and rowPointer
			this.tvCategories.GetCursor( out rowPath, out colPath );
			
			if ( rowPath != null ) {
				this.tvCategories.Model.GetIter( out rowPointer, rowPath );
				row = rowPath.Indices[ 0 ];
			}
		
			return row;
		}
		
		protected virtual void OnAdd(object sender, System.EventArgs e)
		{
			this.agendaSystem.CategoryList.Append(
				new Category( "category_"
			    + Convert.ToString( this.agendaSystem.CategoryList.Size() + 1 )
			));
			
			this.Update();
		}
		
		protected virtual void OnRemove(object sender, System.EventArgs e)
		{
			try {
				this.agendaSystem.CategoryList.Remove( this.GetCurrentRow() );
				this.CategoryRemoved = true;
				Update();
			} catch(Exception ex)
			{
				Util.MsgError( this, AppInfo.Name, ex.Message );
			}
		}
		
		public Boolean CategoryRemoved {
			get; set;
		}

		private AgendaSystem agendaSystem;
	}
}
