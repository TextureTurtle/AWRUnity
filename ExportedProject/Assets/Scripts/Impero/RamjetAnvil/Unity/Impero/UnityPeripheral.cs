using UnityEngine;

namespace RamjetAnvil.Unity.Impero
{
	public abstract class UnityPeripheral : IPeripheral
	{
		private int _playerId = -1;

		private string _identifier;

		private int _numButtons;

		private int _numAxes;

		private float _deadzone;

		private float _mappingThreshold;

		private int _keyCodeFirst;

		private int _keyCodeLast;

		public int PlayerId
		{
			get
			{
				return _playerId;
			}
			set
			{
				_playerId = value;
			}
		}

		public string Identifier
		{
			get
			{
				return _identifier;
			}
		}

		public int NumButtons
		{
			get
			{
				return _numButtons;
			}
		}

		public int NumAxes
		{
			get
			{
				return _numAxes;
			}
		}

		public float Deadzone
		{
			get
			{
				return _deadzone;
			}
		}

		public float MappingThreshold
		{
			get
			{
				return _mappingThreshold;
			}
		}

		public int KeyCodeLast
		{
			get
			{
				return _keyCodeLast;
			}
		}

		public int KeyCodeFirst
		{
			get
			{
				return _keyCodeFirst;
			}
		}

		public UnityPeripheral(string identifier, KeyCode firstKeycode, KeyCode lastKeycode, int numButtons, int numAxes, float deadzone, float mappingThreshold)
		{
			_identifier = identifier;
			_keyCodeFirst = (int)firstKeycode;
			_keyCodeLast = (int)lastKeycode;
			_numButtons = numButtons;
			_numAxes = numAxes;
			_deadzone = deadzone;
			_mappingThreshold = mappingThreshold;
		}

		public static void Initialize()
		{
		}

		public void Update()
		{
		}

		public virtual float GetAxis(int axis)
		{
			string axisName = AxisIndex2AxisName(axis);
			float axisRaw = Input.GetAxisRaw(axisName);
			return (Mathf.Abs(axisRaw) - Deadzone) * Mathf.Sign(axisRaw);
		}

		public virtual bool GetButton(int button)
		{
			return Input.GetKey(ButtonIndexToKeyCode(button));
		}

		public virtual bool GetButtonUp(int button)
		{
			return Input.GetKeyUp(ButtonIndexToKeyCode(button));
		}

		public virtual bool GetButtonDown(int button)
		{
			return Input.GetKeyDown(ButtonIndexToKeyCode(button));
		}

		public virtual string GetHumanReadableAxisName(int axis)
		{
			if (axis < 0 || axis >= NumAxes)
			{
				return "Undefined";
			}
			return _identifier + " Axis " + axis;
		}

		public virtual string GetHumanReadableButtonName(int button)
		{
			if (button < 0 || button >= NumButtons)
			{
				return "Undefined";
			}
			return "Button " + ButtonIndexToKeyCode(button);
		}

		public virtual KeyCode ButtonIndexToKeyCode(int buttonIndex)
		{
			return (KeyCode)(_keyCodeFirst + buttonIndex);
		}

		public virtual int KeyCodeToButtonIndex(KeyCode keyCode)
		{
			int num = (int)(keyCode - _keyCodeFirst);
			if (num < _keyCodeFirst || num > _keyCodeLast)
			{
				num = -1;
			}
			return num;
		}

		protected virtual string AxisIndex2AxisName(int axisIndex)
		{
			return _identifier + "_" + axisIndex;
		}
	}
}
