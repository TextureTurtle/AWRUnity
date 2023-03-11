using UnityEngine;

[RequireComponent(typeof(Focusable))]
public class ItemDescriptor : MonoBehaviour
{
	[SerializeField]
	private string _itemName = "Unidentified";

	public string ItemName
	{
		get
		{
			return _itemName;
		}
	}
}
