using System.Diagnostics;
using UnityEngine;

namespace RamjetAnvil.Unity.Debug
{
	public static class DebugUtils
	{
		[Conditional("RAMJET_DEBUG_LOG")]
		public static void Log(string message, params object[] args)
		{
			UnityEngine.Debug.Log(string.Format(message, args));
		}

		[Conditional("RAMJET_DEBUG_LOG")]
		public static void Log(Object source, string message, params object[] args)
		{
			UnityEngine.Debug.Log(FormatStringWithSource(source, message, args));
		}

		[Conditional("RAMJET_DEBUG_WARNING")]
		public static void LogWarning(string message, params object[] args)
		{
			UnityEngine.Debug.LogWarning(string.Format(message, args));
		}

		[Conditional("RAMJET_DEBUG_WARNING")]
		public static void LogWarning(Object source, string message, params object[] args)
		{
			UnityEngine.Debug.LogWarning(FormatStringWithSource(source, message, args));
		}

		[Conditional("RAMJET_DEBUG_ERROR")]
		public static void LogError(string message, params object[] args)
		{
			UnityEngine.Debug.LogError(string.Format(message, args));
		}

		[Conditional("RAMJET_DEBUG_ERROR")]
		public static void LogError(Object source, string message, params object[] args)
		{
			UnityEngine.Debug.LogError(FormatStringWithSource(source, message, args));
		}

		[Conditional("RAMJET_DEBUG_ASSERT")]
		public static void Assert(bool condition, string message, params object[] args)
		{
			if (!condition)
			{
				UnityEngine.Debug.Break();
				throw new DebugAssertException(string.Format(message, args));
			}
		}

		[Conditional("RAMJET_DEBUG_ASSERT")]
		public static void Assert(Object source, bool condition, string message, params object[] args)
		{
			if (!condition)
			{
				UnityEngine.Debug.Break();
				throw new DebugAssertException(FormatStringWithSource(source, message, args));
			}
		}

		private static string FormatStringWithSource(Object source, string message, params object[] args)
		{
			string arg = string.Format("{0} [{1}]:", source.name, source.GetType().Name);
			return string.Format("{0} {1}", arg, string.Format(message, args));
		}
	}
}
