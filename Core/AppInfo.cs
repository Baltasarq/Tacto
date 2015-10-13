using System;
using System.Diagnostics;

namespace Tacto.Core {
	public class AppInfo {
		public const string Name = "Tacto";
		public static readonly string Author = GetFileVersionInfo().CompanyName;
		public static readonly string Version = string.Format( "{0}.{1}.{2}",
			                                        GetFileVersionInfo().FileMajorPart,
			                                        GetFileVersionInfo().FileMinorPart,
			                                        GetFileVersionInfo().FileBuildPart );
		public const string FileExtension = "bab";
		public static readonly string Comments = GetFileVersionInfo().FileDescription;
		public const string License = "MIT";
		public const string Website = "http://baltasarq.info/";

		public static FileVersionInfo GetFileVersionInfo() {
			if ( fvi == null ) {
				fvi = FileVersionInfo.GetVersionInfo( System.Reflection.Assembly.GetEntryAssembly().Location );
			}

			return fvi;
		}

		public static FileVersionInfo fvi;
	}
}
