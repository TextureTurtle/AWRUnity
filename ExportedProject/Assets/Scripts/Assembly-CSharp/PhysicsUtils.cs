using UnityEngine;

public static class PhysicsUtils
{
	public static Rigidbody FindRigidBodyUpwards(Transform transform)
	{
		Rigidbody component = transform.GetComponent<Rigidbody>();
		if ((bool)component)
		{
			return component;
		}
		if ((bool)transform.parent)
		{
			return FindRigidBodyUpwards(transform.parent);
		}
		return null;
	}

	public static void ReorientRigidbody(Rigidbody rigidbody, Vector3 position, Quaternion rotation)
	{
		bool isKinematic = rigidbody.isKinematic;
		rigidbody.isKinematic = true;
		rigidbody.transform.position = position;
		rigidbody.transform.rotation = rotation;
		rigidbody.isKinematic = isKinematic;
	}

	public static void TranslateRigidbody(Rigidbody rigidbody, Vector3 delta, Space space)
	{
		bool isKinematic = rigidbody.isKinematic;
		rigidbody.isKinematic = true;
		rigidbody.transform.Translate(delta, space);
		rigidbody.isKinematic = isKinematic;
	}
}
