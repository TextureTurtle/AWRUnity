using System;
using System.Collections;
using Mono.WebBrowser.DOM;

namespace Mono.Mozilla.DOM
{
	internal class NamedNodeMap : NodeList, IEnumerable, IList, ICollection, INamedNodeMap
	{
		protected new nsIDOMNamedNodeMap unmanagedNodes;

		public override int Count
		{
			get
			{
				if (unmanagedNodes != null && nodes == null)
				{
					Load();
				}
				return nodeCount;
			}
		}

		public new INode this[int index]
		{
			get
			{
				if (index < 0 || index >= Count)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				return nodes[index];
			}
			set
			{
			}
		}

		public INode this[string name]
		{
			get
			{
				Base.StringSet(storage, name);
				nsIDOMNode ret;
				unmanagedNodes.getNamedItem(storage, out ret);
				for (int i = 0; i < Count; i++)
				{
					if (nodes[i].GetHashCode().Equals(ret.GetHashCode()))
					{
						return nodes[i];
					}
				}
				return null;
			}
			set
			{
			}
		}

		public INode this[string namespaceURI, string localName]
		{
			get
			{
				Base.StringSet(storage, namespaceURI);
				UniString uniString = new UniString(localName);
				nsIDOMNode ret;
				unmanagedNodes.getNamedItemNS(storage, uniString.Handle, out ret);
				for (int i = 0; i < Count; i++)
				{
					if (nodes[i].GetHashCode().Equals(ret.GetHashCode()))
					{
						return nodes[i];
					}
				}
				return null;
			}
			set
			{
			}
		}

		public NamedNodeMap(WebBrowser control, nsIDOMNamedNodeMap nodeMap)
			: base(control, true)
		{
			if (control.platform != control.enginePlatform)
			{
				unmanagedNodes = nsDOMNamedNodeMap.GetProxy(control, nodeMap);
			}
			else
			{
				unmanagedNodes = nodeMap;
			}
		}

		internal override void Load()
		{
			Clear();
			uint ret;
			unmanagedNodes.getLength(out ret);
			nodeCount = (int)ret;
			nodes = new Node[ret];
			for (int i = 0; i < ret; i++)
			{
				nsIDOMNode ret2;
				unmanagedNodes.item((uint)i, out ret2);
				nodes[i] = new Attribute(control, ret2 as nsIDOMAttr);
			}
		}

		public INode RemoveNamedItem(string name)
		{
			Base.StringSet(storage, name);
			nsIDOMNode ret;
			unmanagedNodes.removeNamedItem(storage, out ret);
			for (int i = 0; i < Count; i++)
			{
				if (nodes[i].GetHashCode().Equals(ret.GetHashCode()))
				{
					INode result = nodes[i];
					Remove(nodes[i]);
					return result;
				}
			}
			return null;
		}

		public INode RemoveNamedItemNS(string namespaceURI, string localName)
		{
			Base.StringSet(storage, namespaceURI);
			UniString uniString = new UniString(localName);
			nsIDOMNode ret;
			unmanagedNodes.removeNamedItemNS(storage, uniString.Handle, out ret);
			for (int i = 0; i < Count; i++)
			{
				if (nodes[i].GetHashCode().Equals(ret.GetHashCode()))
				{
					INode result = nodes[i];
					Remove(nodes[i]);
					return result;
				}
			}
			return null;
		}

		public override int GetHashCode()
		{
			return unmanagedNodes.GetHashCode();
		}
	}
}
