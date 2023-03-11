using System.Collections.Generic;

namespace RamjetAnvil.Unity.Impero
{
	public interface IInputApi
	{
		List<IPeripheral> Initialize();

		void Update();
	}
}
