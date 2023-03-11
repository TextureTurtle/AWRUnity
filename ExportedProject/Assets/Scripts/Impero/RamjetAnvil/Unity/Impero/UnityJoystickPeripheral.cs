using UnityEngine;

namespace RamjetAnvil.Unity.Impero
{
	public class UnityJoystickPeripheral : UnityPeripheral
	{
		private int joystickNum;

		public UnityJoystickPeripheral(string identifier, int joystickNum)
			: base(identifier, KeyCode.Joystick1Button0, KeyCode.Joystick4Button19, 20, 8, 0.17f, 0.5f)
		{
			this.joystickNum = joystickNum;
		}

		public override float GetAxis(int axis)
		{
			string axisName = "joy_" + joystickNum + "_" + axis;
			float axisRaw = Input.GetAxisRaw(axisName);
			return Mathf.Clamp01(Mathf.Abs(axisRaw) - base.Deadzone) / (1f - base.Deadzone) * Mathf.Sign(axisRaw);
		}

		public override string GetHumanReadableButtonName(int button)
		{
			return "Joystick " + (joystickNum + 1) + " Button" + (button + 1);
		}

		public override string GetHumanReadableAxisName(int axisIndex)
		{
			return "Joystick " + (joystickNum + 1) + " Axis " + (axisIndex + 1);
		}

		public override KeyCode ButtonIndexToKeyCode(int buttonIndex)
		{
			return (KeyCode)(base.KeyCodeFirst + joystickNum * base.NumButtons + buttonIndex);
		}

		protected override string AxisIndex2AxisName(int axisIndex)
		{
			return "joy" + joystickNum + "_" + axisIndex;
		}
	}
}
