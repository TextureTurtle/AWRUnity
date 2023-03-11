namespace RamjetAnvil.Unity.Impero
{
	public class ButtonInput : IButton, IInput
	{
		private IPeripheral _peripheral;

		private InputType _inputType;

		private int _index;

		public IPeripheral Peripheral
		{
			get
			{
				return _peripheral;
			}
			protected set
			{
				_peripheral = value;
			}
		}

		public InputType InputType
		{
			get
			{
				return _inputType;
			}
			protected set
			{
				_inputType = value;
			}
		}

		public int Index
		{
			get
			{
				return _index;
			}
			protected set
			{
				_index = value;
			}
		}

		public ButtonInput(IPeripheral peripheral, int index)
		{
			_peripheral = peripheral;
			_index = index;
			_inputType = InputType.Button;
		}

		public bool Get()
		{
			if (_peripheral != null)
			{
				return _peripheral.GetButton(_index);
			}
			return false;
		}

		public bool GetDown()
		{
			if (_peripheral != null)
			{
				return _peripheral.GetButtonDown(_index);
			}
			return false;
		}

		public bool GetUp()
		{
			if (_peripheral != null)
			{
				return _peripheral.GetButtonUp(_index);
			}
			return false;
		}

		protected bool Equals(ButtonInput other)
		{
			if (object.Equals(_peripheral, other._peripheral) && _inputType == other._inputType)
			{
				return _index == other._index;
			}
			return false;
		}

		public override bool Equals(object obj)
		{
			if (object.ReferenceEquals(null, obj))
			{
				return false;
			}
			if (object.ReferenceEquals(this, obj))
			{
				return true;
			}
			if (obj.GetType() != GetType())
			{
				return false;
			}
			return Equals((ButtonInput)obj);
		}

		public override int GetHashCode()
		{
			int num = ((_peripheral != null) ? _peripheral.GetHashCode() : 0);
			num = (num * 397) ^ (int)_inputType;
			return (num * 397) ^ _index;
		}

		public static bool operator ==(ButtonInput left, ButtonInput right)
		{
			return object.Equals(left, right);
		}

		public static bool operator !=(ButtonInput left, ButtonInput right)
		{
			return !object.Equals(left, right);
		}

		public override string ToString()
		{
			if (Peripheral == null)
			{
				return "none";
			}
			return Peripheral.GetHumanReadableButtonName(Index);
		}
	}
}
