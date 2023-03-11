namespace RamjetAnvil.Unity.Impero
{
	public class ButtonAdapter : IButtonAdapter, IAdapter
	{
		private InputType _inputType;

		private IButton _input;

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

		public ButtonAdapter(IButton input)
		{
			_inputType = InputType.Button;
			_input = input;
		}

		public bool Get()
		{
			return _input.Get();
		}

		public bool GetDown()
		{
			return _input.GetDown();
		}

		public bool GetUp()
		{
			return _input.GetUp();
		}

		public void Update()
		{
		}
	}
}
