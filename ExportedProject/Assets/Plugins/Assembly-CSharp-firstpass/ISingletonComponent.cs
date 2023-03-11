namespace RamjetAnvil.Unity.Utility
{
	public interface ISingletonComponent
	{
		bool IsInitialized { get; }

		void Initialize();
	}
}
