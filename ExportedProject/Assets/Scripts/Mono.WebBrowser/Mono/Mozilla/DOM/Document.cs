using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using Mono.WebBrowser.DOM;

namespace Mono.Mozilla.DOM
{
	internal class Document : Node, IDocument, INode
	{
		private EventHandlerList events;

		internal static object LoadStoppedEvent;

		internal new nsIDOMDocument node
		{
			get
			{
				return base.node as nsIDOMDocument;
			}
			set
			{
				base.node = value;
			}
		}

		internal new nsIDOMDocument XPComObject
		{
			get
			{
				return node;
			}
		}

		public IElement Active
		{
			get
			{
				nsIWebBrowserFocus nsIWebBrowserFocus = (nsIWebBrowserFocus)control.navigation.navigation;
				if (nsIWebBrowserFocus == null)
				{
					return null;
				}
				nsIDOMElement ret;
				nsIWebBrowserFocus.getFocusedElement(out ret);
				return (IElement)GetTypedNode(ret);
			}
		}

		public string ActiveLinkColor
		{
			get
			{
				if (!(node is nsIDOMHTMLDocument))
				{
					return string.Empty;
				}
				nsIDOMHTMLElement ret;
				((nsIDOMHTMLDocument)node).getBody(out ret);
				((nsIDOMHTMLBodyElement)ret).getALink(storage);
				return Base.StringGet(storage);
			}
			set
			{
				if (node is nsIDOMHTMLDocument)
				{
					Base.StringSet(storage, value);
					nsIDOMHTMLElement ret;
					((nsIDOMHTMLDocument)node).getBody(out ret);
					((nsIDOMHTMLBodyElement)ret).setALink(storage);
				}
			}
		}

		public IElementCollection Anchors
		{
			get
			{
				if (!(node is nsIDOMHTMLDocument))
				{
					return null;
				}
				nsIDOMHTMLCollection ret;
				((nsIDOMHTMLDocument)node).getAnchors(out ret);
				return new HTMLElementCollection(control, (nsIDOMNodeList)ret);
			}
		}

		public IElementCollection Applets
		{
			get
			{
				if (!(node is nsIDOMHTMLDocument))
				{
					return null;
				}
				nsIDOMHTMLCollection ret;
				((nsIDOMHTMLDocument)node).getApplets(out ret);
				return new HTMLElementCollection(control, (nsIDOMNodeList)ret);
			}
		}

		public string Background
		{
			get
			{
				if (!(node is nsIDOMHTMLDocument))
				{
					return string.Empty;
				}
				nsIDOMHTMLElement ret;
				((nsIDOMHTMLDocument)node).getBody(out ret);
				((nsIDOMHTMLBodyElement)ret).getBackground(storage);
				return Base.StringGet(storage);
			}
			set
			{
				if (node is nsIDOMHTMLDocument)
				{
					Base.StringSet(storage, value);
					nsIDOMHTMLElement ret;
					((nsIDOMHTMLDocument)node).getBody(out ret);
					((nsIDOMHTMLBodyElement)ret).setBackground(storage);
				}
			}
		}

		public string BackColor
		{
			get
			{
				if (!(node is nsIDOMHTMLDocument))
				{
					return string.Empty;
				}
				nsIDOMHTMLElement ret;
				((nsIDOMHTMLDocument)node).getBody(out ret);
				((nsIDOMHTMLBodyElement)ret).getBgColor(storage);
				return Base.StringGet(storage);
			}
			set
			{
				if (node is nsIDOMHTMLDocument)
				{
					Base.StringSet(storage, value);
					nsIDOMHTMLElement ret;
					((nsIDOMHTMLDocument)node).getBody(out ret);
					((nsIDOMHTMLBodyElement)ret).setBgColor(storage);
				}
			}
		}

		public IElement Body
		{
			get
			{
				if (!(node is nsIDOMHTMLDocument))
				{
					return null;
				}
				if (!resources.Contains("Body"))
				{
					nsIDOMHTMLElement ret;
					((nsIDOMHTMLDocument)node).getBody(out ret);
					resources.Add("Body", GetTypedNode(ret));
				}
				return resources["Body"] as IElement;
			}
		}

		public string Charset
		{
			get
			{
				nsIDOMDocumentView nsIDOMDocumentView = (nsIDOMDocumentView)node;
				nsIDOMAbstractView ret;
				nsIDOMDocumentView.getDefaultView(out ret);
				nsIInterfaceRequestor nsIInterfaceRequestor = (nsIInterfaceRequestor)ret;
				IntPtr result;
				nsIInterfaceRequestor.getInterface(typeof(nsIDocCharset).GUID, out result);
				nsIDocCharset nsIDocCharset = (nsIDocCharset)Marshal.GetObjectForIUnknown(result);
				StringBuilder stringBuilder = new StringBuilder(30);
				IntPtr ret2 = Marshal.StringToHGlobalUni(stringBuilder.ToString());
				nsIDocCharset.getCharset(ref ret2);
				return Marshal.PtrToStringAnsi(ret2);
			}
			set
			{
				nsIDOMDocumentView nsIDOMDocumentView = (nsIDOMDocumentView)node;
				nsIDOMAbstractView ret;
				nsIDOMDocumentView.getDefaultView(out ret);
				nsIInterfaceRequestor nsIInterfaceRequestor = (nsIInterfaceRequestor)ret;
				IntPtr result;
				nsIInterfaceRequestor.getInterface(typeof(nsIDocCharset).GUID, out result);
				nsIDocCharset nsIDocCharset = (nsIDocCharset)Marshal.GetTypedObjectForIUnknown(result, typeof(nsIDocCharset));
				nsIDocCharset.setCharset(value);
				control.navigation.Go(Url, LoadFlags.CharsetChange);
			}
		}

		public string Cookie
		{
			get
			{
				if (!(node is nsIDOMHTMLDocument))
				{
					return string.Empty;
				}
				((nsIDOMHTMLDocument)node).getCookie(storage);
				return Base.StringGet(storage);
			}
			set
			{
				if (node is nsIDOMHTMLDocument)
				{
					Base.StringSet(storage, value);
					((nsIDOMHTMLDocument)node).setCookie(storage);
				}
			}
		}

		public string Domain
		{
			get
			{
				if (!(node is nsIDOMHTMLDocument))
				{
					return string.Empty;
				}
				((nsIDOMHTMLDocument)node).getDomain(storage);
				return Base.StringGet(storage);
			}
		}

		public IElement DocumentElement
		{
			get
			{
				if (!resources.Contains("DocumentElement"))
				{
					nsIDOMElement ret;
					node.getDocumentElement(out ret);
					resources.Add("DocumentElement", GetTypedNode(ret));
				}
				return resources["DocumentElement"] as IElement;
			}
		}

		public IDocumentType DocType
		{
			get
			{
				nsIDOMDocumentType ret;
				node.getDoctype(out ret);
				return new DocumentType(control, ret);
			}
		}

		public string ForeColor
		{
			get
			{
				if (!(node is nsIDOMHTMLDocument))
				{
					return string.Empty;
				}
				nsIDOMHTMLElement ret;
				((nsIDOMHTMLDocument)node).getBody(out ret);
				((nsIDOMHTMLBodyElement)ret).getText(storage);
				return Base.StringGet(storage);
			}
			set
			{
				if (node is nsIDOMHTMLDocument)
				{
					Base.StringSet(storage, value);
					nsIDOMHTMLElement ret;
					((nsIDOMHTMLDocument)node).getBody(out ret);
					((nsIDOMHTMLBodyElement)ret).setText(storage);
				}
			}
		}

		public IElementCollection Forms
		{
			get
			{
				if (!(node is nsIDOMHTMLDocument))
				{
					return null;
				}
				nsIDOMHTMLCollection ret;
				((nsIDOMHTMLDocument)node).getForms(out ret);
				return new HTMLElementCollection(control, (nsIDOMNodeList)ret);
			}
		}

		public IElementCollection Images
		{
			get
			{
				if (!(node is nsIDOMHTMLDocument))
				{
					return null;
				}
				nsIDOMHTMLCollection ret;
				((nsIDOMHTMLDocument)node).getImages(out ret);
				return new HTMLElementCollection(control, (nsIDOMNodeList)ret);
			}
		}

		public IDOMImplementation Implementation
		{
			get
			{
				nsIDOMDOMImplementation ret;
				node.getImplementation(out ret);
				return new DOMImplementation(control, ret);
			}
		}

		public string LinkColor
		{
			get
			{
				if (!(node is nsIDOMHTMLDocument))
				{
					return string.Empty;
				}
				nsIDOMHTMLElement ret;
				((nsIDOMHTMLDocument)node).getBody(out ret);
				((nsIDOMHTMLBodyElement)ret).getLink(storage);
				return Base.StringGet(storage);
			}
			set
			{
				if (node is nsIDOMHTMLDocument)
				{
					Base.StringSet(storage, value);
					nsIDOMHTMLElement ret;
					((nsIDOMHTMLDocument)node).getBody(out ret);
					((nsIDOMHTMLBodyElement)ret).setLink(storage);
				}
			}
		}

		public IElementCollection Links
		{
			get
			{
				if (!(node is nsIDOMHTMLDocument))
				{
					return null;
				}
				nsIDOMHTMLCollection ret;
				((nsIDOMHTMLDocument)node).getLinks(out ret);
				return new HTMLElementCollection(control, (nsIDOMNodeList)ret);
			}
		}

		public IStylesheetList Stylesheets
		{
			get
			{
				nsIDOMDocumentStyle nsIDOMDocumentStyle = (nsIDOMDocumentStyle)node;
				nsIDOMStyleSheetList ret;
				nsIDOMDocumentStyle.getStyleSheets(out ret);
				return new StylesheetList(control, ret);
			}
		}

		public string Title
		{
			get
			{
				if (!(node is nsIDOMHTMLDocument))
				{
					return string.Empty;
				}
				((nsIDOMHTMLDocument)node).getTitle(storage);
				return Base.StringGet(storage);
			}
			set
			{
				Base.StringSet(storage, value);
				((nsIDOMHTMLDocument)node).setTitle(storage);
			}
		}

		public string Url
		{
			get
			{
				if (!(node is nsIDOMHTMLDocument))
				{
					return string.Empty;
				}
				((nsIDOMHTMLDocument)node).getURL(storage);
				return Base.StringGet(storage);
			}
		}

		public string VisitedLinkColor
		{
			get
			{
				if (!(node is nsIDOMHTMLDocument))
				{
					return string.Empty;
				}
				nsIDOMHTMLElement ret;
				((nsIDOMHTMLDocument)node).getBody(out ret);
				((nsIDOMHTMLBodyElement)ret).getVLink(storage);
				return Base.StringGet(storage);
			}
			set
			{
				if (node is nsIDOMHTMLDocument)
				{
					Base.StringSet(storage, value);
					nsIDOMHTMLElement ret;
					((nsIDOMHTMLDocument)node).getBody(out ret);
					((nsIDOMHTMLBodyElement)ret).setVLink(storage);
				}
			}
		}

		public IWindow Window
		{
			get
			{
				nsIDOMDocumentView nsIDOMDocumentView = (nsIDOMDocumentView)node;
				nsIDOMAbstractView ret;
				nsIDOMDocumentView.getDefaultView(out ret);
				nsIInterfaceRequestor nsIInterfaceRequestor = (nsIInterfaceRequestor)ret;
				if (nsIInterfaceRequestor == null)
				{
					return null;
				}
				IntPtr result;
				nsIInterfaceRequestor.getInterface(typeof(nsIDOMWindow).GUID, out result);
				nsIDOMWindow domWindow = (nsIDOMWindow)Marshal.GetObjectForIUnknown(result);
				return new Window(control, domWindow);
			}
		}

		internal new EventHandlerList Events
		{
			get
			{
				if (events == null)
				{
					events = new EventHandlerList();
				}
				return events;
			}
		}

		public event EventHandler LoadStopped
		{
			add
			{
				Events.AddHandler(LoadStoppedEvent, value);
			}
			remove
			{
				Events.RemoveHandler(LoadStoppedEvent, value);
			}
		}

		public Document(WebBrowser control, nsIDOMHTMLDocument document)
			: base(control, document)
		{
			if (control.platform != control.enginePlatform)
			{
				node = nsDOMHTMLDocument.GetProxy(control, document);
			}
			else
			{
				node = document;
			}
		}

		public Document(WebBrowser control, nsIDOMDocument document)
			: base(control, document)
		{
			if (control.platform != control.enginePlatform)
			{
				node = nsDOMDocument.GetProxy(control, document);
			}
			else
			{
				node = document;
			}
		}

		static Document()
		{
			LoadStopped = new object();
		}

		protected override void Dispose(bool disposing)
		{
			if (!disposed && disposing)
			{
				resources.Clear();
				node = null;
			}
			base.Dispose(disposing);
		}

		public IAttribute CreateAttribute(string name)
		{
			Base.StringSet(storage, name);
			nsIDOMAttr ret;
			node.createAttribute(storage, out ret);
			return new Attribute(control, ret);
		}

		public IElement CreateElement(string tagName)
		{
			Base.StringSet(storage, tagName);
			nsIDOMElement ret;
			node.createElement(storage, out ret);
			if (node is nsIDOMHTMLDocument)
			{
				return new HTMLElement(control, (nsIDOMHTMLElement)ret);
			}
			return new Element(control, ret);
		}

		public IElement GetElementById(string id)
		{
			if (!resources.Contains("GetElementById" + id))
			{
				Base.StringSet(storage, id);
				nsIDOMElement ret;
				node.getElementById(storage, out ret);
				if (ret == null)
				{
					return null;
				}
				resources.Add("GetElementById" + id, GetTypedNode(ret));
			}
			return resources["GetElementById" + id] as IElement;
		}

		public IElementCollection GetElementsByTagName(string name)
		{
			if (!resources.Contains("GetElementsByTagName" + name))
			{
				nsIDOMNodeList ret;
				node.getElementsByTagName(storage, out ret);
				if (ret == null)
				{
					return null;
				}
				resources.Add("GetElementsByTagName" + name, new HTMLElementCollection(control, ret));
			}
			return resources["GetElementsByTagName" + name] as IElementCollection;
		}

		public IElement GetElement(int x, int y)
		{
			nsIDOMNodeList ret;
			node.getChildNodes(out ret);
			HTMLElementCollection hTMLElementCollection = new HTMLElementCollection(control, ret);
			IElement result = null;
			foreach (Element item in hTMLElementCollection)
			{
				if (item.Left <= x && item.Top <= y && item.Left + item.Width >= x && item.Top + item.Height >= y)
				{
					return item;
				}
			}
			return result;
		}

		public void Write(string text)
		{
			if (node is nsIDOMHTMLDocument)
			{
				Base.StringSet(storage, text);
				((nsIDOMHTMLDocument)node).write(storage);
			}
		}

		public string InvokeScript(string script)
		{
			return Base.EvalScript(control, script);
		}

		public override int GetHashCode()
		{
			return node.GetHashCode();
		}
	}
}
