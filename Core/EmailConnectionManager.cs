using System;

namespace Tacto {
	public class EmailConnectionManager: ConnectionManager {
		public EmailConnectionManager(string url)
			: base( url )
		{
		}

		public override void Open()
		{
			OpenDocument( Protocol.Email, this.URL );
		}
	}
}

