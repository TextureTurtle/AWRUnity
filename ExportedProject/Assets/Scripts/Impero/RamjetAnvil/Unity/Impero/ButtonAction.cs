namespace RamjetAnvil.Unity.Impero
{
	public class ButtonAction : Action
	{
		private IButtonAdapter _adapter;

		public string InputName
		{
			get
			{
				if (_adapter == null)
				{
					return "Undefined";
				}
				return InputManager.Instance.GetInputName(_adapter.Input);
			}
		}

		public IButtonAdapter Adapter
		{
			get
			{
				return _adapter;
			}
			set
			{
				_adapter = value;
			}
		}

		public ButtonAction(string identifier, string name)
			: base(identifier, name, InputType.Button)
		{
		}

		public bool Get()
		{
			if (_adapter == null)
			{
				return false;
			}
			return _adapter.Get();
		}

		public bool GetUp()
		{
			if (_adapter == null)
			{
				return false;
			}
			return _adapter.GetUp();
		}

		public bool GetDown()
		{
			if (_adapter == null)
			{
				return false;
			}
			return _adapter.GetDown();
		}
	}
}
