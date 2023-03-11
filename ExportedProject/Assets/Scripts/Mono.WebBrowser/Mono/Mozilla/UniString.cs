using System;
using System.Runtime.InteropServices;

namespace Mono.Mozilla
{
	internal class UniString : IDisposable
	{
		[StructLayout(LayoutKind.Sequential)]
		private class nsStringContainer
		{
			private IntPtr v;

			private IntPtr d1;

			private uint d2;

			private IntPtr d3;
		}

		private bool disposed;

		private nsStringContainer unmanagedContainer;

		private HandleRef handle;

		private string str = string.Empty;

		private bool dirty;

		public HandleRef Handle
		{
			get
			{
				dirty = true;
				return handle;
			}
		}

		public string String
		{
			get
			{
				if (dirty)
				{
					IntPtr aBuf;
					bool aTerm;
					Base.gluezilla_StringGetData(handle, out aBuf, out aTerm);
					str = Marshal.PtrToStringUni(aBuf);
					dirty = false;
				}
				return str;
			}
			set
			{
				if (str != value)
				{
					str = value;
					Base.gluezilla_StringSetData(handle, str, (uint)str.Length);
				}
			}
		}

		public int Length
		{
			get
			{
				return String.Length;
			}
		}

		public UniString(string value)
		{
			unmanagedContainer = new nsStringContainer();
			IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(nsStringContainer)));
			Marshal.StructureToPtr(unmanagedContainer, ptr, false);
			handle = new HandleRef(typeof(nsStringContainer), ptr);
			uint num = Base.gluezilla_StringContainerInit(handle);
			String = value;
		}

		~UniString()
		{
			Dispose(false);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (disposing)
				{
					Base.gluezilla_StringContainerFinish(handle);
					Marshal.FreeHGlobal(handle.Handle);
				}
				disposed = true;
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public override string ToString()
		{
			return String;
		}
	}
}
