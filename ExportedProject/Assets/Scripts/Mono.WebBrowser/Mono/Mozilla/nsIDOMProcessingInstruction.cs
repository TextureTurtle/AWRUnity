using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Mono.Mozilla
{
	[ComImport]
	[Guid("a6cf907f-15b3-11d2-932e-00805f8add32")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface nsIDOMProcessingInstruction : nsIDOMNode
	{
		[MethodImpl(MethodImplOptions.InternalCall | MethodImplOptions.PreserveSig)]
		new int getNodeName(HandleRef ret);

		[MethodImpl(MethodImplOptions.InternalCall | MethodImplOptions.PreserveSig)]
		new int getNodeValue(HandleRef ret);

		[MethodImpl(MethodImplOptions.InternalCall | MethodImplOptions.PreserveSig)]
		new int setNodeValue(HandleRef value);

		[MethodImpl(MethodImplOptions.InternalCall | MethodImplOptions.PreserveSig)]
		new int getNodeType(out ushort ret);

		[MethodImpl(MethodImplOptions.InternalCall | MethodImplOptions.PreserveSig)]
		new int getParentNode([MarshalAs(UnmanagedType.Interface)] out nsIDOMNode ret);

		[MethodImpl(MethodImplOptions.InternalCall | MethodImplOptions.PreserveSig)]
		new int getChildNodes([MarshalAs(UnmanagedType.Interface)] out nsIDOMNodeList ret);

		[MethodImpl(MethodImplOptions.InternalCall | MethodImplOptions.PreserveSig)]
		new int getFirstChild([MarshalAs(UnmanagedType.Interface)] out nsIDOMNode ret);

		[MethodImpl(MethodImplOptions.InternalCall | MethodImplOptions.PreserveSig)]
		new int getLastChild([MarshalAs(UnmanagedType.Interface)] out nsIDOMNode ret);

		[MethodImpl(MethodImplOptions.InternalCall | MethodImplOptions.PreserveSig)]
		new int getPreviousSibling([MarshalAs(UnmanagedType.Interface)] out nsIDOMNode ret);

		[MethodImpl(MethodImplOptions.InternalCall | MethodImplOptions.PreserveSig)]
		new int getNextSibling([MarshalAs(UnmanagedType.Interface)] out nsIDOMNode ret);

		[MethodImpl(MethodImplOptions.InternalCall | MethodImplOptions.PreserveSig)]
		new int getAttributes([MarshalAs(UnmanagedType.Interface)] out nsIDOMNamedNodeMap ret);

		[MethodImpl(MethodImplOptions.InternalCall | MethodImplOptions.PreserveSig)]
		new int getOwnerDocument([MarshalAs(UnmanagedType.Interface)] out nsIDOMDocument ret);

		[MethodImpl(MethodImplOptions.InternalCall | MethodImplOptions.PreserveSig)]
		new int insertBefore([MarshalAs(UnmanagedType.Interface)] nsIDOMNode newChild, [MarshalAs(UnmanagedType.Interface)] nsIDOMNode refChild, [MarshalAs(UnmanagedType.Interface)] out nsIDOMNode ret);

		[MethodImpl(MethodImplOptions.InternalCall | MethodImplOptions.PreserveSig)]
		new int replaceChild([MarshalAs(UnmanagedType.Interface)] nsIDOMNode newChild, [MarshalAs(UnmanagedType.Interface)] nsIDOMNode oldChild, [MarshalAs(UnmanagedType.Interface)] out nsIDOMNode ret);

		[MethodImpl(MethodImplOptions.InternalCall | MethodImplOptions.PreserveSig)]
		new int removeChild([MarshalAs(UnmanagedType.Interface)] nsIDOMNode oldChild, [MarshalAs(UnmanagedType.Interface)] out nsIDOMNode ret);

		[MethodImpl(MethodImplOptions.InternalCall | MethodImplOptions.PreserveSig)]
		new int appendChild([MarshalAs(UnmanagedType.Interface)] nsIDOMNode newChild, [MarshalAs(UnmanagedType.Interface)] out nsIDOMNode ret);

		[MethodImpl(MethodImplOptions.InternalCall | MethodImplOptions.PreserveSig)]
		new int hasChildNodes(out bool ret);

		[MethodImpl(MethodImplOptions.InternalCall | MethodImplOptions.PreserveSig)]
		new int cloneNode(bool deep, [MarshalAs(UnmanagedType.Interface)] out nsIDOMNode ret);

		[MethodImpl(MethodImplOptions.InternalCall | MethodImplOptions.PreserveSig)]
		new int normalize();

		[MethodImpl(MethodImplOptions.InternalCall | MethodImplOptions.PreserveSig)]
		new int isSupported(HandleRef feature, HandleRef version, out bool ret);

		[MethodImpl(MethodImplOptions.InternalCall | MethodImplOptions.PreserveSig)]
		new int getNamespaceURI(HandleRef ret);

		[MethodImpl(MethodImplOptions.InternalCall | MethodImplOptions.PreserveSig)]
		new int getPrefix(HandleRef ret);

		[MethodImpl(MethodImplOptions.InternalCall | MethodImplOptions.PreserveSig)]
		new int setPrefix(HandleRef value);

		[MethodImpl(MethodImplOptions.InternalCall | MethodImplOptions.PreserveSig)]
		new int getLocalName(HandleRef ret);

		[MethodImpl(MethodImplOptions.InternalCall | MethodImplOptions.PreserveSig)]
		new int hasAttributes(out bool ret);

		[MethodImpl(MethodImplOptions.InternalCall | MethodImplOptions.PreserveSig)]
		int getTarget(HandleRef ret);

		[MethodImpl(MethodImplOptions.InternalCall | MethodImplOptions.PreserveSig)]
		int getData(HandleRef ret);

		[MethodImpl(MethodImplOptions.InternalCall | MethodImplOptions.PreserveSig)]
		int setData(HandleRef value);
	}
}
