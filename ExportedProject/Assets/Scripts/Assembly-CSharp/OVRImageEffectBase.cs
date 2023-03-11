using UnityEngine;

[AddComponentMenu("")]
[RequireComponent(typeof(Camera))]
public class OVRImageEffectBase : MonoBehaviour
{
	public Material material;

	protected void Start()
	{
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
		}
	}
}
