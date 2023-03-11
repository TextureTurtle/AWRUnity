using UnityEngine;

public class RigidbodyTest : MonoBehaviour
{
	private void Update()
	{
		Debug.Log(base.rigidbody.IsSleeping());
	}
}
