using UnityEngine;

namespace RamjetAnvil.Unity.Impero
{
	public class UnityMousePeripheral : UnityPeripheral
	{
		public UnityMousePeripheral(string identifier)
			: base(identifier, KeyCode.Mouse0, KeyCode.Mouse6, 7, 2, 0f, 20f)
		{
		}
	}
}
