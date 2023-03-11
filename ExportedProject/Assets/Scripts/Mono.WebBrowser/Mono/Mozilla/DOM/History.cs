using Mono.WebBrowser.DOM;

namespace Mono.Mozilla.DOM
{
	internal class History : DOMObject, IHistory
	{
		private Navigation navigation;

		public int Count
		{
			get
			{
				return navigation.HistoryCount;
			}
		}

		public History(WebBrowser control, Navigation navigation)
			: base(control)
		{
			this.navigation = navigation;
		}

		public void Back(int count)
		{
			navigation.Go(count * -1, true);
		}

		public void Forward(int count)
		{
			navigation.Go(count, true);
		}

		public void GoToIndex(int index)
		{
			navigation.Go(index);
		}

		public void GoToUrl(string url)
		{
			int num = -1;
			nsISHistory ret;
			navigation.navigation.getSessionHistory(out ret);
			int count = Count;
			for (int i = 0; i < count; i++)
			{
				nsIHistoryEntry ret2;
				ret.getEntryAtIndex(i, false, out ret2);
				nsIURI ret3;
				ret2.getURI(out ret3);
				AsciiString asciiString = new AsciiString(string.Empty);
				ret3.getSpec(asciiString.Handle);
				if (string.Compare(asciiString.ToString(), url, true) == 0)
				{
					num = i;
					break;
				}
			}
			if (num > -1)
			{
				GoToIndex(num);
			}
		}
	}
}
