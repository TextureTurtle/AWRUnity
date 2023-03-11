using System;
using System.ComponentModel;
using Mono.WebBrowser;
using Mono.WebBrowser.DOM;

namespace Mono.Mozilla.DOM
{
	internal class Node : DOMObject, INode
	{
		internal nsIDOMNode nodeNoProxy;

		private nsIDOMNode _node;

		protected int hashcode;

		private EventListener eventListener;

		private new WebBrowser control;

		private EventHandlerList events;

		internal nsIDOMNode node
		{
			get
			{
				return _node;
			}
			set
			{
				hashcode = value.GetHashCode();
				nodeNoProxy = _node;
				if (!(value is nsIDOMHTMLDocument) && control.platform != control.enginePlatform)
				{
					_node = nsDOMNode.GetProxy(control, value);
				}
				else
				{
					_node = value;
				}
			}
		}

		internal virtual nsIDOMNode XPComObject
		{
			get
			{
				return node;
			}
		}

		public virtual IAttributeCollection Attributes
		{
			get
			{
				if (!resources.Contains("Attributes"))
				{
					nsIDOMNamedNodeMap ret;
					node.getAttributes(out ret);
					if (ret == null)
					{
						return new AttributeCollection(control);
					}
					resources.Add("Attributes", new AttributeCollection(control, ret));
				}
				return resources["Attributes"] as IAttributeCollection;
			}
		}

		public virtual INodeList ChildNodes
		{
			get
			{
				if (!resources.Contains("ChildNodes"))
				{
					nsIDOMNodeList ret;
					node.getChildNodes(out ret);
					resources.Add("ChildNodes", new NodeList(control, ret));
				}
				return resources["ChildNodes"] as INodeList;
			}
		}

		public virtual INode FirstChild
		{
			get
			{
				if (!resources.Contains("FirstChild"))
				{
					nsIDOMNode ret;
					node.getFirstChild(out ret);
					resources.Add("FirstChild", GetTypedNode(ret));
				}
				return resources["FirstChild"] as INode;
			}
		}

		public virtual INode LastChild
		{
			get
			{
				if (!resources.Contains("LastChild"))
				{
					nsIDOMNode ret;
					node.getLastChild(out ret);
					resources.Add("LastChild", GetTypedNode(ret));
				}
				return resources["LastChild"] as INode;
			}
		}

		public virtual INode Parent
		{
			get
			{
				if (!resources.Contains("Parent"))
				{
					nsIDOMNode ret;
					node.getParentNode(out ret);
					resources.Add("Parent", GetTypedNode(ret));
				}
				return resources["Parent"] as INode;
			}
		}

		public virtual INode Previous
		{
			get
			{
				if (!resources.Contains("Previous"))
				{
					nsIDOMNode ret;
					node.getPreviousSibling(out ret);
					resources.Add("Previous", GetTypedNode(ret));
				}
				return resources["Previous"] as INode;
			}
		}

		public virtual INode Next
		{
			get
			{
				if (!resources.Contains("Next"))
				{
					nsIDOMNode ret;
					node.getNextSibling(out ret);
					resources.Add("Next", GetTypedNode(ret));
				}
				return resources["Next"] as INode;
			}
		}

		public virtual string LocalName
		{
			get
			{
				node.getLocalName(storage);
				return Base.StringGet(storage);
			}
		}

		public IDocument Owner
		{
			get
			{
				nsIDOMDocument ret;
				node.getOwnerDocument(out ret);
				if (!control.documents.ContainsKey(ret.GetHashCode()))
				{
					control.documents.Add(ret.GetHashCode(), new Document(control, ret as nsIDOMHTMLDocument));
				}
				return control.documents[ret.GetHashCode()] as IDocument;
			}
		}

		public string Style
		{
			get
			{
				nsIDOMDocument ret;
				node.getOwnerDocument(out ret);
				nsIDOMDocumentView nsIDOMDocumentView = (nsIDOMDocumentView)ret;
				nsIDOMAbstractView ret2;
				nsIDOMDocumentView.getDefaultView(out ret2);
				nsIDOMViewCSS nsIDOMViewCSS = (nsIDOMViewCSS)ret2;
				Base.StringSet(storage, string.Empty);
				AsciiString asciiString = new AsciiString(string.Empty);
				nsIDOMCSSStyleDeclaration ret3;
				nsIDOMViewCSS.getComputedStyle(node as nsIDOMElement, asciiString.Handle, out ret3);
				if (ret3 == null)
				{
					return string.Empty;
				}
				ret3.getCssText(storage);
				return Base.StringGet(storage);
			}
			set
			{
				nsIDOMDocument ret;
				node.getOwnerDocument(out ret);
				nsIDOMDocumentView nsIDOMDocumentView = (nsIDOMDocumentView)ret;
				nsIDOMAbstractView ret2;
				nsIDOMDocumentView.getDefaultView(out ret2);
				nsIDOMViewCSS nsIDOMViewCSS = (nsIDOMViewCSS)ret2;
				Base.StringSet(storage, string.Empty);
				nsIDOMCSSStyleDeclaration ret3;
				nsIDOMViewCSS.getComputedStyle(node as nsIDOMElement, storage, out ret3);
				Base.StringSet(storage, value);
				ret3.setCssText(storage);
			}
		}

		public virtual Mono.WebBrowser.DOM.NodeType Type
		{
			get
			{
				ushort ret;
				node.getNodeType(out ret);
				return (Mono.WebBrowser.DOM.NodeType)(int)Enum.ToObject(typeof(Mono.WebBrowser.DOM.NodeType), ret);
			}
		}

		public virtual string Value
		{
			get
			{
				node.getNodeValue(storage);
				return Base.StringGet(storage);
			}
			set
			{
				Base.StringSet(storage, value);
				node.setNodeValue(storage);
			}
		}

		public virtual IntPtr AccessibleObject
		{
			get
			{
				//Discarded unreachable code: IL_0041, IL_005e
				nsIAccessible ret = null;
				try
				{
					nsIAccessibilityService accessibilityService = control.AccessibilityService;
					nsIDOMDocument ret2;
					node.getOwnerDocument(out ret2);
					accessibilityService.getAccessibleFor(ret2, out ret);
				}
				catch (Mono.WebBrowser.Exception ex)
				{
					Console.Error.WriteLine(ex.Message);
					goto IL_008a;
				}
				catch (System.Exception ex2)
				{
					Console.Error.WriteLine(ex2.Message);
					goto IL_008a;
				}
				if (ret != null)
				{
					IntPtr aOutAccessible = IntPtr.Zero;
					if (ret.getNativeInterface(out aOutAccessible) == 0)
					{
						return aOutAccessible;
					}
				}
				goto IL_008a;
				IL_008a:
				Console.Error.WriteLine("Accessibility not available");
				return IntPtr.Zero;
			}
		}

		private EventListener EventListener
		{
			get
			{
				if (eventListener == null)
				{
					eventListener = new EventListener(node as nsIDOMEventTarget, this);
				}
				return eventListener;
			}
		}

		public new EventHandlerList Events
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

		public event NodeEventHandler Click
		{
			add
			{
				EventListener.AddHandler(value, "click");
			}
			remove
			{
				EventListener.RemoveHandler(value, "click");
			}
		}

		public event NodeEventHandler DoubleClick
		{
			add
			{
				EventListener.AddHandler(value, "dblclick");
			}
			remove
			{
				EventListener.RemoveHandler(value, "dblclick");
			}
		}

		public event NodeEventHandler KeyDown
		{
			add
			{
				EventListener.AddHandler(value, "keydown");
			}
			remove
			{
				EventListener.RemoveHandler(value, "keydown");
			}
		}

		public event NodeEventHandler KeyPress
		{
			add
			{
				EventListener.AddHandler(value, "keypress");
			}
			remove
			{
				EventListener.RemoveHandler(value, "keypress");
			}
		}

		public event NodeEventHandler KeyUp
		{
			add
			{
				EventListener.AddHandler(value, "keyup");
			}
			remove
			{
				EventListener.RemoveHandler(value, "keyup");
			}
		}

		public event NodeEventHandler MouseDown
		{
			add
			{
				EventListener.AddHandler(value, "mousedown");
			}
			remove
			{
				EventListener.RemoveHandler(value, "mousedown");
			}
		}

		public event NodeEventHandler MouseEnter
		{
			add
			{
				EventListener.AddHandler(value, "mouseenter");
			}
			remove
			{
				EventListener.RemoveHandler(value, "mouseenter");
			}
		}

		public event NodeEventHandler MouseLeave
		{
			add
			{
				EventListener.AddHandler(value, "mouseout");
			}
			remove
			{
				EventListener.RemoveHandler(value, "mouseout");
			}
		}

		public event NodeEventHandler MouseMove
		{
			add
			{
				EventListener.AddHandler(value, "mousemove");
			}
			remove
			{
				EventListener.RemoveHandler(value, "mousemove");
			}
		}

		public event NodeEventHandler MouseOver
		{
			add
			{
				EventListener.AddHandler(value, "mouseover");
			}
			remove
			{
				EventListener.RemoveHandler(value, "mouseover");
			}
		}

		public event NodeEventHandler MouseUp
		{
			add
			{
				EventListener.AddHandler(value, "mouseup");
			}
			remove
			{
				EventListener.RemoveHandler(value, "mouseup");
			}
		}

		public event NodeEventHandler OnFocus
		{
			add
			{
				EventListener.AddHandler(value, "focus");
			}
			remove
			{
				EventListener.RemoveHandler(value, "focus");
			}
		}

		public event NodeEventHandler OnBlur
		{
			add
			{
				EventListener.AddHandler(value, "blur");
			}
			remove
			{
				EventListener.RemoveHandler(value, "blur");
			}
		}

		public Node(WebBrowser control, nsIDOMNode domNode)
			: base(control)
		{
			this.control = control;
			node = domNode;
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

		public virtual void FireEvent(string eventName)
		{
			nsIDOMDocument ret;
			node.getOwnerDocument(out ret);
			nsIDOMDocumentEvent nsIDOMDocumentEvent = (nsIDOMDocumentEvent)ret;
			nsIDOMDocumentView nsIDOMDocumentView = (nsIDOMDocumentView)ret;
			nsIDOMAbstractView ret2;
			nsIDOMDocumentView.getDefaultView(out ret2);
			nsIDOMEventTarget nsIDOMEventTarget = (nsIDOMEventTarget)node;
			bool ret3 = false;
			switch (eventName)
			{
			case "mousedown":
			case "mouseup":
			case "click":
			case "dblclick":
			case "mouseover":
			case "mouseout":
			case "mousemove":
			case "contextmenu":
			{
				string text = "mouseevents";
				Base.StringSet(storage, text);
				nsIDOMEvent ret7;
				nsIDOMDocumentEvent.createEvent(storage, out ret7);
				nsIDOMMouseEvent nsIDOMMouseEvent = ret7 as nsIDOMMouseEvent;
				Base.StringSet(storage, eventName);
				nsIDOMMouseEvent.initMouseEvent(storage, true, true, ret2, 1, 0, 0, 0, 0, false, false, false, false, 0, nsIDOMEventTarget);
				nsIDOMEventTarget.dispatchEvent(nsIDOMMouseEvent, out ret3);
				break;
			}
			case "keydown":
			case "keyup":
			case "keypress":
			{
				string text = "keyevents";
				Base.StringSet(storage, text);
				nsIDOMEvent ret6;
				nsIDOMDocumentEvent.createEvent(storage, out ret6);
				Base.StringSet(storage, eventName);
				nsIDOMKeyEvent nsIDOMKeyEvent = ret6 as nsIDOMKeyEvent;
				nsIDOMKeyEvent.initKeyEvent(storage, true, true, ret2, false, false, false, false, 0u, 0u);
				nsIDOMEventTarget.dispatchEvent(nsIDOMKeyEvent, out ret3);
				break;
			}
			case "DOMActivate":
			case "DOMFocusIn":
			case "DOMFocusOut":
			case "input":
			{
				string text = "uievents";
				Base.StringSet(storage, text);
				nsIDOMEvent ret5;
				nsIDOMDocumentEvent.createEvent(storage, out ret5);
				Base.StringSet(storage, eventName);
				nsIDOMUIEvent nsIDOMUIEvent = ret5 as nsIDOMUIEvent;
				nsIDOMUIEvent.initUIEvent(storage, true, true, ret2, 1);
				nsIDOMEventTarget.dispatchEvent(nsIDOMUIEvent, out ret3);
				break;
			}
			default:
			{
				string text = "events";
				Base.StringSet(storage, text);
				nsIDOMEvent ret4;
				nsIDOMDocumentEvent.createEvent(storage, out ret4);
				Base.StringSet(storage, eventName);
				ret4.initEvent(storage, true, true);
				nsIDOMEventTarget.dispatchEvent(ret4, out ret3);
				break;
			}
			}
		}

		public virtual INode InsertBefore(INode child, INode refChild)
		{
			nsIDOMNode ret;
			node.insertBefore(((Node)child).node, ((Node)refChild).node, out ret);
			return child;
		}

		public virtual INode ReplaceChild(INode child, INode oldChild)
		{
			nsIDOMNode ret;
			node.replaceChild(((Node)child).node, ((Node)oldChild).node, out ret);
			return oldChild;
		}

		public virtual INode RemoveChild(INode child)
		{
			nsIDOMNode ret;
			node.removeChild(((Node)child).node, out ret);
			return child;
		}

		public virtual INode AppendChild(INode child)
		{
			nsIDOMNode ret;
			int value = node.appendChild(((Node)child).node, out ret);
			Console.Error.WriteLine(value);
			return child;
		}

		public override bool Equals(object obj)
		{
			return this == obj as Node;
		}

		public override int GetHashCode()
		{
			return hashcode;
		}

		public void AttachEventHandler(string eventName, EventHandler handler)
		{
			EventListener.AddHandler(handler, eventName);
		}

		public void DetachEventHandler(string eventName, EventHandler handler)
		{
			EventListener.RemoveHandler(handler, eventName);
		}

		public void AttachEventHandler(string eventName, Delegate handler)
		{
			string key = string.Intern(node.GetHashCode() + ":" + eventName);
			Events.AddHandler(key, handler);
		}

		public void DetachEventHandler(string eventName, Delegate handler)
		{
			string key = string.Intern(node.GetHashCode() + ":" + eventName);
			Events.RemoveHandler(key, handler);
		}

		public static bool operator ==(Node left, Node right)
		{
			if ((object)left == right)
			{
				return true;
			}
			if ((object)left == null || (object)right == null)
			{
				return false;
			}
			return left.hashcode == right.hashcode;
		}

		public static bool operator !=(Node left, Node right)
		{
			return !(left == right);
		}
	}
}
