namespace RamjetAnvil.Unity.Impero
{
	public interface IAdapter
	{
		InputType InputType { get; }

		IInput Input { get; }

		void Update();
	}
}
