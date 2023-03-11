using UnityEngine;

namespace RamjetAnvil.Unity.Impero
{
	public class AxisInput : IAxis, IInput
	{
		private IPeripheral _peripheral;

		private InputType _inputType;

		private int _index;

		private Polarity _polarity;

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

		public Polarity Polarity
		{
			get
			{
				return _polarity;
			}
			private set
			{
				_polarity = value;
			}
		}

		public AxisInput(IPeripheral peripheral, int index, Polarity polarity)
		{
			_peripheral = peripheral;
			_index = index;
			_polarity = polarity;
			InputType = InputType.Axis;
		}

		public float Get()
		{
			if (_peripheral != null)
			{
				float axis = _peripheral.GetAxis(_index);
				if (!(Mathf.Abs(Mathf.Sign(axis) - PolarityToFloat(_polarity)) < float.Epsilon))
				{
					return 0f;
				}
				return Mathf.Abs(axis);
			}
			return 0f;
		}

		protected bool Equals(AxisInput other)
		{
			if (object.Equals(_peripheral, other._peripheral) && _inputType == other._inputType && _index == other._index)
			{
				return _polarity == other._polarity;
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
			return Equals((AxisInput)obj);
		}

		public override int GetHashCode()
		{
			int num = ((_peripheral != null) ? _peripheral.GetHashCode() : 0);
			num = (num * 397) ^ (int)_inputType;
			num = (num * 397) ^ _index;
			return (num * 397) ^ (int)_polarity;
		}

		public static bool operator ==(AxisInput left, AxisInput right)
		{
			return object.Equals(left, right);
		}

		public static bool operator !=(AxisInput left, AxisInput right)
		{
			return !object.Equals(left, right);
		}

		public static float PolarityToFloat(Polarity polarity)
		{
			if (polarity != Polarity.Positive)
			{
				return -1f;
			}
			return 1f;
		}
	}
}
