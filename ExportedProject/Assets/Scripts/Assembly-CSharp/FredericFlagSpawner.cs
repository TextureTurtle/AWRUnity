using System.Collections;
using UnityEngine;

public class FredericFlagSpawner : MonoBehaviour
{
	[SerializeField]
	private float _frederikWinTime = 5f;

	[SerializeField]
	private GameObject _frederikFlag;

	[SerializeField]
	private GearMount _victoryMount;

	private bool _flagSpawned;

	private void Start()
	{
		StartCoroutine(WaitAndSpawnAsync());
	}

	private IEnumerator WaitAndSpawnAsync()
	{
		while (!_flagSpawned)
		{
			if (Time.timeSinceLevelLoad > _frederikWinTime)
			{
				SpawnFrederikFlag();
				_flagSpawned = true;
				break;
			}
			yield return new WaitForSeconds(1f);
		}
	}

	private void SpawnFrederikFlag()
	{
		if (_victoryMount.MountedItem == null)
		{
			GameObject gameObject = Object.Instantiate(_frederikFlag, _victoryMount.MountPoint.position, _victoryMount.MountPoint.rotation) as GameObject;
			Pickup component = gameObject.GetComponent<Pickup>();
			component.MountTo(_victoryMount);
		}
	}
}
