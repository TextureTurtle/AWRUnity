namespace RamjetAnvil.Unity.Impero
{
	public class AxisAdapter : IAxisAdapter, IAdapter
	{
		private InputType _inputType;

		private IAxis _input;

		private float _sensitivity;

		public InputType InputType
		{
			get
			{
				return _inputType;
			}
		}

		public IInput Input
		{
			get
			{
				return _input;
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

		public AxisAdapter(IAxis input)
		{
			_inputType = InputType.Axis;
			_input = input;
		}

		public float Get()
		{
			return _input.Get() * _sensitivity;
		}

		public void Update()
		{
		}
	}
}
