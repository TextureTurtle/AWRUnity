namespace RamjetAnvil.Unity.Impero
{
	public static class AdapterFactory
	{
		public static IButtonAdapter CreateButtonAdapter(IInput input)
		{
			if (input == null)
			{
				return null;
			}
			switch (input.InputType)
			{
			case InputType.Button:
				return new ButtonAdapter(input as IButton);
			case InputType.Axis:
				return new AxisToButtonAdapter(input as IAxis);
			default:
				return null;
			}
		}

		public static IAxisAdapter CreateAxisAdapter(IInput input)
		{
			if (input == null)
			{
				return null;
			}
			switch (input.InputType)
			{
			case InputType.Button:
				return new ButtonToAxisAdapter(input as IButton);
			case InputType.Axis:
				return new AxisAdapter(input as IAxis);
			default:
				return null;
			}
		}
	}
}
