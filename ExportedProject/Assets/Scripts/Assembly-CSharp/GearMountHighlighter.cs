using UnityEngine;

[RequireComponent(typeof(Focusable))]
public class GearMountHighlighter : MonoBehaviour
{
	private Focusable _focusable;

	private static float focusedAlpha = 0.5f;

	private static float unfocusedAlpha = 0.1f;

	private void Start()
	{
		_focusable = GetComponent<Focusable>();
		_focusable.OnGainedFocus += OnFocusGained;
		_focusable.OnLostFocus += OnFocusLost;
		SetShader(unfocusedAlpha);
	}

	private void OnFocusGained(Player player)
	{
		SetShader(focusedAlpha);
	}

	private void OnFocusLost(Player player)
	{
		SetShader(unfocusedAlpha);
	}

	private void SetShader(float alpha)
	{
		Color color = base.renderer.material.color;
		color.a = alpha;
		base.renderer.material.color = color;
	}
}
