namespace RamjetAnvil.Unity.Impero
{
	public class SubAxisAction : Action
	{
		private IAxisAdapter _adapter;

		private readonly Polarity _polarity;

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

		public IAxisAdapter Adapter
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

		public Polarity Polarity
		{
			get
			{
				return _polarity;
			}
		}

		public SubAxisAction(string identifier, string name, Polarity polarity)
			: base(identifier, name, InputType.Axis)
		{
			_polarity = polarity;
		}

		public float Get()
		{
			if (_adapter == null)
			{
				return 0f;
			}
			return _adapter.Get() * AxisInput.PolarityToFloat(_polarity);
		}
	}
}
