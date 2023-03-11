using System.Collections;
using System.Collections.Generic;
using Mono.WebBrowser.DOM;

namespace Mono.Mozilla.DOM
{
	internal class StylesheetList : DOMObject, IEnumerable, IStylesheetList
	{
		private nsIDOMStyleSheetList unmanagedStyles;

		private List<IStylesheet> styles;

		public IStylesheet this[int index]
		{
			get
			{
				return styles[index];
			}
			set
			{
				styles[index] = value;
			}
		}

		public int Count
		{
			get
			{
				if (styles.Count == 0)
				{
					Load();
				}
				return styles.Count;
			}
		}

		public StylesheetList(WebBrowser control, nsIDOMStyleSheetList stylesheetList)
			: base(control)
		{
			if (control.platform != control.enginePlatform)
			{
				unmanagedStyles = nsDOMStyleSheetList.GetProxy(control, stylesheetList);
			}
			else
			{
				unmanagedStyles = stylesheetList;
			}
			styles = new List<IStylesheet>();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			if (styles.Count == 0)
			{
				Load();
			}
			return styles.GetEnumerator();
		}

		protected override void Dispose(bool disposing)
		{
			if (!disposed && disposing)
			{
				Clear();
			}
			base.Dispose(disposing);
		}

		protected void Clear()
		{
			styles.Clear();
		}

		internal void Load()
		{
			Clear();
			uint ret;
			unmanagedStyles.getLength(out ret);
			for (int i = 0; i < ret; i++)
			{
				nsIDOMStyleSheet ret2;
				unmanagedStyles.item((uint)i, out ret2);
				styles.Add(new Stylesheet(control, ret2));
			}
		}
	}
}
