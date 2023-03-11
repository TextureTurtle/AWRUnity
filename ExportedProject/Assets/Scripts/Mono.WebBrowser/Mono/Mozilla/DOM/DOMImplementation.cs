using Mono.WebBrowser.DOM;

namespace Mono.Mozilla.DOM
{
	internal class DOMImplementation : DOMObject, IDOMImplementation
	{
		private nsIDOMDOMImplementation unmanagedDomImpl;

		protected int hashcode;

		public DOMImplementation(WebBrowser control, nsIDOMDOMImplementation domImpl)
			: base(control)
		{
			if (control.platform != control.enginePlatform)
			{
				unmanagedDomImpl = nsDOMDOMImplementation.GetProxy(control, domImpl);
			}
			else
			{
				unmanagedDomImpl = domImpl;
			}
			hashcode = unmanagedDomImpl.GetHashCode();
		}

		protected override void Dispose(bool disposing)
		{
			if (!disposed && disposing)
			{
				unmanagedDomImpl = null;
			}
			base.Dispose(disposing);
		}

		public bool HasFeature(string feature, string version)
		{
			Base.StringSet(storage, feature);
			UniString uniString = new UniString(version);
			bool ret;
			unmanagedDomImpl.hasFeature(storage, uniString.Handle, out ret);
			return ret;
		}

		public IDocumentType CreateDocumentType(string qualifiedName, string publicId, string systemId)
		{
			Base.StringSet(storage, qualifiedName);
			UniString uniString = new UniString(publicId);
			UniString uniString2 = new UniString(systemId);
			nsIDOMDocumentType ret;
			unmanagedDomImpl.createDocumentType(storage, uniString.Handle, uniString2.Handle, out ret);
			return new DocumentType(control, ret);
		}

		public IDocument CreateDocument(string namespaceURI, string qualifiedName, IDocumentType doctype)
		{
			Base.StringSet(storage, namespaceURI);
			UniString uniString = new UniString(qualifiedName);
			nsIDOMDocument ret;
			unmanagedDomImpl.createDocument(storage, uniString.Handle, ((DocumentType)doctype).ComObject, out ret);
			control.documents.Add(ret.GetHashCode(), new Document(control, ret));
			return control.documents[ret.GetHashCode()] as IDocument;
		}
	}
}
