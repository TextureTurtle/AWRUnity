using System.IO;
using System.Text;
using Mono.WebBrowser.DOM;

namespace Mono.Mozilla.DOM
{
	internal class HTMLElement : Element, IElement, INode
	{
		protected new nsIDOMHTMLElement node
		{
			get
			{
				return base.node as nsIDOMHTMLElement;
			}
			set
			{
				base.node = value;
			}
		}

		public new string InnerHTML
		{
			get
			{
				nsIDOMNSHTMLElement nsIDOMNSHTMLElement = node as nsIDOMNSHTMLElement;
				if (nsIDOMNSHTMLElement == null)
				{
					return null;
				}
				nsIDOMNSHTMLElement.getInnerHTML(storage);
				return Base.StringGet(storage);
			}
			set
			{
				nsIDOMNSHTMLElement nsIDOMNSHTMLElement = node as nsIDOMNSHTMLElement;
				if (nsIDOMNSHTMLElement != null)
				{
					Base.StringSet(storage, value);
					nsIDOMNSHTMLElement.setInnerHTML(storage);
				}
			}
		}

		public override string OuterHTML
		{
			get
			{
				//Discarded unreachable code: IL_0061, IL_015b
				try
				{
					control.DocEncoder.Flags = DocumentEncoderFlags.OutputRaw;
					if (Equals(Owner.DocumentElement))
					{
						return control.DocEncoder.EncodeToString((Document)Owner);
					}
					return control.DocEncoder.EncodeToString(this);
				}
				catch
				{
					string tagName = TagName;
					string text = "<" + tagName;
					string text2;
					foreach (IAttribute attribute in Attributes)
					{
						text2 = text;
						text = text2 + " " + attribute.Name + "=\"" + attribute.Value + "\"";
					}
					nsIDOMNSHTMLElement nsIDOMNSHTMLElement = node as nsIDOMNSHTMLElement;
					nsIDOMNSHTMLElement.getInnerHTML(storage);
					text2 = text;
					return text2 + ">" + Base.StringGet(storage) + "</" + tagName + ">";
				}
			}
			set
			{
				nsIDOMDocumentRange nsIDOMDocumentRange = ((Document)control.Document).XPComObject as nsIDOMDocumentRange;
				nsIDOMRange ret;
				nsIDOMDocumentRange.createRange(out ret);
				ret.setStartBefore(node);
				nsIDOMNSRange nsIDOMNSRange = ret as nsIDOMNSRange;
				Base.StringSet(storage, value);
				nsIDOMDocumentFragment ret2;
				nsIDOMNSRange.createContextualFragment(storage, out ret2);
				nsIDOMNode ret3;
				node.getParentNode(out ret3);
				ret3 = nsDOMNode.GetProxy(control, ret3);
				nsIDOMNode ret4;
				ret3.replaceChild(ret2, node, out ret4);
				node = ret4 as nsIDOMHTMLElement;
			}
		}

		public override System.IO.Stream ContentStream
		{
			get
			{
				//Discarded unreachable code: IL_0061, IL_016e
				try
				{
					control.DocEncoder.Flags = DocumentEncoderFlags.OutputRaw;
					if (Equals(Owner.DocumentElement))
					{
						return control.DocEncoder.EncodeToStream((Document)Owner);
					}
					return control.DocEncoder.EncodeToStream(this);
				}
				catch
				{
					string tagName = TagName;
					string text = "<" + tagName;
					string text2;
					foreach (IAttribute attribute in Attributes)
					{
						text2 = text;
						text = text2 + " " + attribute.Name + "=\"" + attribute.Value + "\"";
					}
					nsIDOMNSHTMLElement nsIDOMNSHTMLElement = node as nsIDOMNSHTMLElement;
					nsIDOMNSHTMLElement.getInnerHTML(storage);
					text2 = text;
					text = text2 + ">" + Base.StringGet(storage) + "</" + tagName + ">";
					byte[] bytes = Encoding.UTF8.GetBytes(text);
					return new MemoryStream(bytes);
				}
			}
		}

		public override bool Disabled
		{
			get
			{
				if (HasAttribute("disabled"))
				{
					string attribute = GetAttribute("disabled");
					return bool.Parse(attribute);
				}
				return false;
			}
			set
			{
				if (HasAttribute("disabled"))
				{
					SetAttribute("disabled", value.ToString());
				}
			}
		}

		public override int TabIndex
		{
			get
			{
				int ret;
				((nsIDOMNSHTMLElement)node).getTabIndex(out ret);
				return ret;
			}
			set
			{
				((nsIDOMNSHTMLElement)node).setTabIndex(value);
			}
		}

		public HTMLElement(WebBrowser control, nsIDOMHTMLElement domHtmlElement)
			: base(control, domHtmlElement)
		{
			node = domHtmlElement;
		}

		protected override void Dispose(bool disposing)
		{
			if (!disposed && disposing)
			{
				node = null;
			}
			base.Dispose(disposing);
		}

		public override int GetHashCode()
		{
			return hashcode;
		}
	}
}
