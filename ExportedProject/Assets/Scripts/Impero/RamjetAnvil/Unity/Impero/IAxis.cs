namespace RamjetAnvil.Unity.Impero
{
	public interface IAxis : IInput
	{
		Polarity Polarity { get; }

		float Get();
	}
}
