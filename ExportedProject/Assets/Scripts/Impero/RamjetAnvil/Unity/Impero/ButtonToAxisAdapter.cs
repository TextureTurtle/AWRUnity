using UnityEngine;

namespace RamjetAnvil.Unity.Impero
{
	public class ButtonToAxisAdapter : IAxisAdapter, IAdapter
	{
		private InputType _inputType;

		private IButton _input;

		private float _sensitivity;

		private float _currentValue;

		private float _speed;

		private float _gravity;

		public IInput Input
		{
			get
			{
				return _input;
			}
		}

		public InputType InputType
		{
			get
			{
				return _inputType;
			}
		}

		public float Sensitivity
		{
			get
			{
				return _sensitivity;
			}
			set
			{
				_sensitivity = value;
			}
		}

		public ButtonToAxisAdapter(IButton input)
		{
			_inputType = InputType.Axis;
			_input = input;
			_currentValue = 0f;
			_speed = 10f;
			_gravity = 10f;
		}

		public float Get()
		{
			return _currentValue * _sensitivity;
		}

		public void Update()
		{
			bool flag = _input.Get();
			_currentValue += (flag ? _speed : (0f - Mathf.Abs(_gravity))) * Time.deltaTime;
			_currentValue = Mathf.Clamp01(_currentValue);
		}
	}
}
