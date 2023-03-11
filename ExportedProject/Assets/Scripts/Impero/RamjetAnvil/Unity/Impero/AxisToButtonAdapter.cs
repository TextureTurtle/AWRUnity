namespace RamjetAnvil.Unity.Impero
{
	public class AxisToButtonAdapter : IButtonAdapter, IAdapter
	{
		private InputType _inputType;

		private IAxis _input;

		private float _threshold;

		private bool _pressedThisFrame;

		private bool _pressedLastFrame;

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

		public AxisToButtonAdapter(IAxis input)
		{
			_inputType = InputType.Button;
			_input = input;
			_threshold = 0.5f;
			_pressedThisFrame = false;
			_pressedLastFrame = false;
		}

		public bool Get()
		{
			return _pressedThisFrame;
		}

		public bool GetDown()
		{
			if (!_pressedLastFrame)
			{
				return _pressedThisFrame;
			}
			return false;
		}

		public bool GetUp()
		{
			if (_pressedLastFrame)
			{
				return !_pressedThisFrame;
			}
			return false;
		}

		public void Update()
		{
			_pressedLastFrame = _pressedThisFrame;
			float num = _input.Get();
			_pressedThisFrame = num > _threshold;
		}
	}
}
