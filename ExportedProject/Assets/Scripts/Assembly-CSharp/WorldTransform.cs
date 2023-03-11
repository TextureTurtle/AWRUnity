using UnityEngine;

[AddComponentMenu("Aurora/World/WorldTransform")]
public class WorldTransform : MonoBehaviour
{
	public DVector3 Position
	{
		get
		{
			return SingletonComponent<World>.Instance.LocalPointToWorld(base.transform.position);
		}
		set
		{
			base.transform.position = SingletonComponent<World>.Instance.WorldPointToLocal(value);
		}
	}

	private void Start()
	{
		World.Register(this);
	}

	private void OnDestroy()
	{
		World.Unregister(this);
	}

	public void OnSceneMove(Vector3 delta)
	{
		if (!(base.transform.parent != null))
		{
			if ((bool)base.rigidbody)
			{
				PhysicsUtils.TranslateRigidbody(base.rigidbody, delta, Space.World);
			}
			else
			{
				base.transform.Translate(delta);
			}
		}
	}
}
