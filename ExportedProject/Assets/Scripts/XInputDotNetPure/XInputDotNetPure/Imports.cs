using System;
using System.Runtime.InteropServices;

namespace XInputDotNetPure
{
	internal class Imports
	{
		public enum Constants
		{
			Success = 0,
			NotConnected = 1167,
			LeftStickDeadZone = 7849,
			RightStickDeadZone = 8689
		}

		internal const string DLLName = "XInputInterface";

		[DllImport("XInputInterface")]
		public static extern uint XInputGamePadGetState(uint playerIndex, IntPtr state);

		[DllImport("XInputInterface")]
		public static extern void XInputGamePadSetState(uint playerIndex, float leftMotor, float rightMotor);
	}
}
