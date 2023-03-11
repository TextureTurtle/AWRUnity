using System;
using UnityEngine;

public class KillZone : MonoBehaviour
{
	public event Action<Player> OnPlayerKilled;

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			Player component = other.GetComponent<Player>();
			if (this.OnPlayerKilled != null)
			{
				this.OnPlayerKilled(component);
			}
			UnityEngine.Object.Destroy(component, 1f);
		}
		if (other.tag == "Killable")
		{
			UnityEngine.Object.Destroy(other.gameObject);
		}
	}
}
