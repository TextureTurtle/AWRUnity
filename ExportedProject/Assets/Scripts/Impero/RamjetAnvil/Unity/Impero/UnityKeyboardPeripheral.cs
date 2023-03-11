using UnityEngine;

namespace RamjetAnvil.Unity.Impero
{
	public class UnityKeyboardPeripheral : UnityPeripheral
	{
		public UnityKeyboardPeripheral(string identifier)
			: base(identifier, KeyCode.Backspace, KeyCode.Break, 311, 0, 0f, 0.5f)
		{
		}
	}
}
