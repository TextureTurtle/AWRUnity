using System;
using UnityEngine;

public class Focusable : MonoBehaviour
{
	private Renderer[] _renderers;

	private float _intensityMultiplier = 3f;

	public event Action<Player> OnGainedFocus;

	public event Action<Player> OnLostFocus;

	public void Focus(Player player)
	{
		SetHightlight(true);
		if (this.OnGainedFocus != null)
		{
			this.OnGainedFocus(player);
		}
	}

	public void Unfocus(Player player)
	{
		SetHightlight(false);
		if (this.OnLostFocus != null)
		{
			this.OnLostFocus(player);
		}
	}

	private void Start()
	{
		_renderers = GetComponentsInChildren<MeshRenderer>();
	}

	private void SetHightlight(bool highlight)
	{
		Renderer[] renderers = _renderers;
		foreach (Renderer renderer in renderers)
		{
			Color color = renderer.material.color;
			if (highlight)
			{
				color.r *= _intensityMultiplier;
				color.g *= _intensityMultiplier;
				color.b *= _intensityMultiplier * 2f;
			}
			else
			{
				color.r /= _intensityMultiplier;
				color.g /= _intensityMultiplier;
				color.b /= _intensityMultiplier * 2f;
			}
			renderer.material.color = color;
		}
	}
}
