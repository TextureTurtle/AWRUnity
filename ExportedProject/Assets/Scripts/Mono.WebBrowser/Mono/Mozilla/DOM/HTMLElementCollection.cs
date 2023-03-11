using System;
using System.Collections;
using Mono.WebBrowser.DOM;

namespace Mono.Mozilla.DOM
{
	internal class HTMLElementCollection : NodeList, IEnumerable, IList, ICollection, IElementCollection, INodeList
	{
		public new IElement this[int index]
		{
			get
			{
				if (index < 0 || index >= nodeCount)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				return nodes[index] as IElement;
			}
			set
			{
				if (index < 0 || index >= nodeCount)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				nodes[index] = value;
			}
		}

		public HTMLElementCollection(WebBrowser control, nsIDOMNodeList nodeList)
			: base(control, nodeList)
		{
		}

		public HTMLElementCollection(WebBrowser control)
			: base(control)
		{
		}

		internal override void Load()
		{
			Clear();
			uint ret;
			unmanagedNodes.getLength(out ret);
			Node[] array = new Node[ret];
			for (int i = 0; i < ret; i++)
			{
				nsIDOMNode ret2;
				unmanagedNodes.item((uint)i, out ret2);
				ushort ret3;
				ret2.getNodeType(out ret3);
				if (ret3 == 1)
				{
					array[nodeCount++] = new HTMLElement(control, (nsIDOMHTMLElement)ret2);
				}
			}
			nodes = new Node[nodeCount];
			Array.Copy(array, nodes, nodeCount);
		}
	}
}
