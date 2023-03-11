using System.Collections.Generic;
using UnityEngine;

namespace RamjetAnvil.Unity.Impero
{
	public class UnityInputApi : IInputApi
	{
		public const string MouseId = "unity_mouse";

		public const string KeyboardId = "unity_keyboard";

		public const string JoystickIdPrefix = "unity_";

		private bool _useKeyboard = true;

		private bool _useMouse = true;

		private bool _useJoysticks = true;

		private List<IPeripheral> _peripherals;

		public List<IPeripheral> Initialize()
		{
			_peripherals = new List<IPeripheral>();
			if (_useKeyboard)
			{
				_peripherals.Add(new UnityKeyboardPeripheral("unity_keyboard"));
			}
			if (_useMouse)
			{
				_peripherals.Add(new UnityMousePeripheral("unity_mouse"));
			}
			if (_useJoysticks)
			{
				string[] joystickNames = Input.GetJoystickNames();
				for (int i = 0; i < joystickNames.Length; i++)
				{
					_peripherals.Add(new UnityJoystickPeripheral("unity_" + joystickNames[i], i));
				}
			}
			return _peripherals;
		}

		public void Update()
		{
			foreach (IPeripheral peripheral in _peripherals)
			{
				peripheral.Update();
			}
		}
	}
}
