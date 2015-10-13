using System;

namespace Tacto {
	public class HttpConnectionManager: ConnectionManager {
		public const string UrlGMapsSearch = "http://www.google.com/maps/search/";

		public HttpConnectionManager(string url)
			: base( url )
		{
		}

		public override void Open()
		{
			OpenDocument( Protocol.Http, this.URL );
		}
	}
}
