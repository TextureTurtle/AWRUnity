namespace XInputDotNetPure
{
	public struct GamePadState
	{
		internal struct RawState
		{
			public struct GamePad
			{
				public ushort dwButtons;

				public byte bLeftTrigger;

				public byte bRightTrigger;

				public short sThumbLX;

				public short sThumbLY;

				public short sThumbRX;

				public short sThumbRY;
			}

			public uint dwPacketNumber;

			public GamePad Gamepad;
		}

		private enum ButtonsConstants
		{
			DPadUp = 1,
			DPadDown = 2,
			DPadLeft = 4,
			DPadRight = 8,
			Start = 0x10,
			Back = 0x20,
			LeftThumb = 0x40,
			RightThumb = 0x80,
			LeftShoulder = 0x100,
			RightShoulder = 0x200,
			A = 0x1000,
			B = 0x2000,
			X = 0x4000,
			Y = 0x8000
		}

		private bool isConnected;

		private uint packetNumber;

		private GamePadButtons buttons;

		private GamePadDPad dPad;

		private GamePadThumbSticks thumbSticks;

		private GamePadTriggers triggers;

		public uint PacketNumber
		{
			get
			{
				return packetNumber;
			}
		}

		public bool IsConnected
		{
			get
			{
				return isConnected;
			}
		}

		public GamePadButtons Buttons
		{
			get
			{
				return buttons;
			}
		}

		public GamePadDPad DPad
		{
			get
			{
				return dPad;
			}
		}

		public GamePadTriggers Triggers
		{
			get
			{
				return triggers;
			}
		}

		public GamePadThumbSticks ThumbSticks
		{
			get
			{
				return thumbSticks;
			}
		}

		internal GamePadState(bool isConnected, RawState rawState, GamePadDeadZone deadZone)
		{
			this.isConnected = isConnected;
			if (!isConnected)
			{
				rawState.dwPacketNumber = 0u;
				rawState.Gamepad.dwButtons = 0;
				rawState.Gamepad.bLeftTrigger = 0;
				rawState.Gamepad.bRightTrigger = 0;
				rawState.Gamepad.sThumbLX = 0;
				rawState.Gamepad.sThumbLY = 0;
				rawState.Gamepad.sThumbRX = 0;
				rawState.Gamepad.sThumbRY = 0;
			}
			packetNumber = rawState.dwPacketNumber;
			buttons = new GamePadButtons(((rawState.Gamepad.dwButtons & 0x10) == 0) ? ButtonState.Released : ButtonState.Pressed, ((rawState.Gamepad.dwButtons & 0x20) == 0) ? ButtonState.Released : ButtonState.Pressed, ((rawState.Gamepad.dwButtons & 0x40) == 0) ? ButtonState.Released : ButtonState.Pressed, ((rawState.Gamepad.dwButtons & 0x80) == 0) ? ButtonState.Released : ButtonState.Pressed, ((rawState.Gamepad.dwButtons & 0x100) == 0) ? ButtonState.Released : ButtonState.Pressed, ((rawState.Gamepad.dwButtons & 0x200) == 0) ? ButtonState.Released : ButtonState.Pressed, ((rawState.Gamepad.dwButtons & 0x1000) == 0) ? ButtonState.Released : ButtonState.Pressed, ((rawState.Gamepad.dwButtons & 0x2000) == 0) ? ButtonState.Released : ButtonState.Pressed, ((rawState.Gamepad.dwButtons & 0x4000) == 0) ? ButtonState.Released : ButtonState.Pressed, ((rawState.Gamepad.dwButtons & 0x8000) == 0) ? ButtonState.Released : ButtonState.Pressed);
			dPad = new GamePadDPad(((rawState.Gamepad.dwButtons & 1) == 0) ? ButtonState.Released : ButtonState.Pressed, ((rawState.Gamepad.dwButtons & 2) == 0) ? ButtonState.Released : ButtonState.Pressed, ((rawState.Gamepad.dwButtons & 4) == 0) ? ButtonState.Released : ButtonState.Pressed, ((rawState.Gamepad.dwButtons & 8) == 0) ? ButtonState.Released : ButtonState.Pressed);
			if (deadZone == GamePadDeadZone.IndependentAxes)
			{
				rawState.Gamepad.sThumbLX = ThumbStickDeadZoneIndependantAxes(rawState.Gamepad.sThumbLX, 7849);
				rawState.Gamepad.sThumbLY = ThumbStickDeadZoneIndependantAxes(rawState.Gamepad.sThumbLY, 7849);
				rawState.Gamepad.sThumbRX = ThumbStickDeadZoneIndependantAxes(rawState.Gamepad.sThumbRX, 8689);
				rawState.Gamepad.sThumbRY = ThumbStickDeadZoneIndependantAxes(rawState.Gamepad.sThumbRY, 8689);
			}
			thumbSticks = new GamePadThumbSticks(new GamePadThumbSticks.StickValue((rawState.Gamepad.sThumbLX < 0) ? ((float)rawState.Gamepad.sThumbLX / 32768f) : ((float)rawState.Gamepad.sThumbLX / 32767f), (rawState.Gamepad.sThumbLY < 0) ? ((float)rawState.Gamepad.sThumbLY / 32768f) : ((float)rawState.Gamepad.sThumbLY / 32767f)), new GamePadThumbSticks.StickValue((rawState.Gamepad.sThumbRX < 0) ? ((float)rawState.Gamepad.sThumbRX / 32768f) : ((float)rawState.Gamepad.sThumbRX / 32767f), (rawState.Gamepad.sThumbRY < 0) ? ((float)rawState.Gamepad.sThumbRY / 32768f) : ((float)rawState.Gamepad.sThumbRY / 32767f)));
			triggers = new GamePadTriggers((float)(int)rawState.Gamepad.bLeftTrigger / 255f, (float)(int)rawState.Gamepad.bRightTrigger / 255f);
		}

		public static short ThumbStickDeadZoneIndependantAxes(short value, short deadZone)
		{
			if ((value < 0 && value > -deadZone) || (value > 0 && value < deadZone))
			{
				return 0;
			}
			return value;
		}
	}
}
