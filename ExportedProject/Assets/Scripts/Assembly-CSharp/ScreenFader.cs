using System;
using System.Collections;
using UnityEngine;

public class ScreenFader : MonoBehaviour
{
	[SerializeField]
	private Texture _texture;

	[SerializeField]
	private float _startAlpha = 1f;

	private Rect _screenRect;

	private Color _fadeColor;

	public void FadeIn(Action onFadeDone)
	{
		StopAllCoroutines();
		StartCoroutine(Fade(0f, 1f, onFadeDone));
	}

	public void FadeOut(Action onFadeDone)
	{
		StopAllCoroutines();
		StartCoroutine(Fade(1f, 1f, onFadeDone));
	}

	private void Start()
	{
		_fadeColor = new Color(1f, 1f, 1f, _startAlpha);
	}

	public void DrawFade()
	{
		GUI.color = _fadeColor;
		_screenRect = new Rect(0f, 0f, Screen.width, Screen.height);
		GUI.DrawTexture(_screenRect, _texture);
		GUI.color = Color.white;
	}

	private IEnumerator Fade(float alpha, float fadeTime, Action onFadeDone)
	{
		float startAlpha = _fadeColor.a;
		float lastTime = Time.realtimeSinceStartup;
		float timer = 0f;
		while (timer < fadeTime)
		{
			_fadeColor.a = Mathf.Lerp(startAlpha, alpha, timer / fadeTime);
			timer += Time.realtimeSinceStartup - lastTime;
			lastTime = Time.realtimeSinceStartup;
			yield return new WaitForEndOfFrame();
		}
		_fadeColor.a = alpha;
		if (onFadeDone != null)
		{
			onFadeDone();
		}
	}
}
