namespace RamjetAnvil.Unity.Impero
{
	public abstract class Action
	{
		private string _identifier;

		private string _name;

		private InputType _actionType;

		public string Identifier
		{
			get
			{
				return _identifier;
			}
			protected set
			{
				_identifier = value;
			}
		}

		public string Name
		{
			get
			{
				return _name;
			}
			protected set
			{
				_name = value;
			}
		}

		public InputType ActionType
		{
			get
			{
				return _actionType;
			}
		}

		public Action(string identifier, string name, InputType actionType)
		{
			_identifier = identifier;
			_name = name;
			_actionType = actionType;
		}
	}
}
