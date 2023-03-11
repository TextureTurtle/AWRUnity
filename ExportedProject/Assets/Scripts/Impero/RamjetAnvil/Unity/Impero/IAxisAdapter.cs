namespace RamjetAnvil.Unity.Impero
{
	public interface IAxisAdapter : IAdapter
	{
		float Sensitivity { get; set; }

		float Get();
	}
}
