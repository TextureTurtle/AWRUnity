using System;

namespace Mono.WebBrowser
{
	public class LoadCommitedEventArgs : EventArgs
	{
		private string uri;

		public string Uri
		{
			get
			{
				return uri;
			}
		}

		public LoadCommitedEventArgs(string uri)
		{
			this.uri = uri;
		}
	}
}
