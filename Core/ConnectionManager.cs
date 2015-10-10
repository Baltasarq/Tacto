using System;

namespace Tacto {
	public abstract class ConnectionManager {
		public enum Protocol { Http, Email, File };
		public const string EmailProtocolPrefix = "mailto:";
		public const string HttpProtocolPrefix = "http://";

		public ConnectionManager(string url)
		{
			this.URL = url;
		}

		public string URL {
			get {
				return this.url;
			}
			set {
				this.url = value.Trim().ToLower();
			}
		}

		public abstract void Open();

		public static void OpenDocument(Protocol p, string url)
		{
			string cmd = "";

			url = url.Trim().ToLower();

			if ( url.Length > 0 ) {
				if ( p == Protocol.Email ) {
					cmd += EmailProtocolPrefix;
				}
				else
				if ( p == Protocol.Http ) {
					if ( !url.StartsWith( HttpProtocolPrefix ) ) {
						cmd += HttpProtocolPrefix;
					}
				}

				cmd += url;
				System.Diagnostics.Process.Start( cmd );
			} else {
				throw new ApplicationException( "Empty url" );
			}

			return;
		}

		private string url;
	}
}
