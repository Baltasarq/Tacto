using System;
using Gtk;
using Tacto.Gui;

namespace Tacto
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			Application.Init();
			new MainWindow().ShowAll();
			Application.Run();
		}
	}
}
