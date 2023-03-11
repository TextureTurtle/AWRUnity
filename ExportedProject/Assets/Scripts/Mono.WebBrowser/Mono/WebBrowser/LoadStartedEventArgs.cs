using System.ComponentModel;

namespace Mono.WebBrowser
{
	public class LoadStartedEventArgs : CancelEventArgs
	{
		private string uri;

		private string frameName;

		public string Uri
		{
			get
			{
				return uri;
			}
		}

		public string FrameName
		{
			get
			{
				return frameName;
			}
		}

		public LoadStartedEventArgs(string uri, string frameName)
		{
			this.uri = uri;
			this.frameName = frameName;
		}
	}
}
