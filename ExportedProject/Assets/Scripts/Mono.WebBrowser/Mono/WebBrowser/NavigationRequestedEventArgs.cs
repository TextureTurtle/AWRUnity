using System.ComponentModel;

namespace Mono.WebBrowser
{
	public class NavigationRequestedEventArgs : CancelEventArgs
	{
		private string uri;

		public string Uri
		{
			get
			{
				return uri;
			}
		}

		public NavigationRequestedEventArgs(string uri)
		{
			this.uri = uri;
		}
	}
}
