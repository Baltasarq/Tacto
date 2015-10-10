using System;

namespace Tacto {
	public class HttpConnectionManager: ConnectionManager {
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
