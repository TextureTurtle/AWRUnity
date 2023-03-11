using UnityEngine;

public class StreamedObject : MonoBehaviour
{
	[SerializeField]
	private Vector3 _size;

	public Vector3 Size
	{
		get
		{
			return _size;
		}
		set
		{
			_size = value;
		}
	}
}
