namespace RamjetAnvil.Unity.Impero
{
	public interface IInput
	{
		InputType InputType { get; }

		IPeripheral Peripheral { get; }

		int Index { get; }
	}
}
