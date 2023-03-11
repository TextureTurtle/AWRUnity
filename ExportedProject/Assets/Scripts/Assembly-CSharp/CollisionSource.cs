using System;
using UnityEngine;

public class CollisionSource : MonoBehaviour
{
	public event Action<Collision> OnCollisionEntered;

	public event Action<Collision> OnCollisionStayed;

	public event Action<Collision> OnCollisionExited;

	public event Action<Collider> OnTriggerEntered;

	public event Action<Collider> OnTriggerStayed;

	public event Action<Collider> OnTriggerExited;

	private void OnCollisionEnter(Collision collision)
	{
		if (this.OnCollisionEntered != null)
		{
			this.OnCollisionEntered(collision);
		}
	}

	private void OnCollisionStay(Collision collision)
	{
		if (this.OnCollisionStayed != null)
		{
			this.OnCollisionStayed(collision);
		}
	}

	private void OnCollisionExit(Collision collision)
	{
		if (this.OnCollisionExited != null)
		{
			this.OnCollisionExited(collision);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (this.OnTriggerEntered != null)
		{
			this.OnTriggerEntered(other);
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (this.OnTriggerStayed != null)
		{
			this.OnTriggerStayed(other);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (this.OnTriggerExited != null)
		{
			this.OnTriggerExited(other);
		}
	}
}
