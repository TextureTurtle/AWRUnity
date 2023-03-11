using UnityEngine;

public class AeroManagerPerformance : MonoBehaviour
{
	private void Update()
	{
		AeroManager instance = SingletonComponent<AeroManager>.Instance;
		for (int i = 0; i < 128; i++)
		{
			instance.GetWindVelocity(Vector3.zero);
		}
	}
}
