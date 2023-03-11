using System;

namespace RamjetAnvil.Unity.Debug
{
	public class DebugAssertException : Exception
	{
		public DebugAssertException(string message)
			: base(message)
		{
		}
	}
}
