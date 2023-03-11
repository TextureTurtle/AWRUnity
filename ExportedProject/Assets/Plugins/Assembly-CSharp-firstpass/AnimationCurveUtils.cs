using System.Collections.Generic;
using UnityEngine;

public static class AnimationCurveUtils
{
	public static AnimationCurve Logarithmic(float timeStart, float timeEnd, float logBase)
	{
		List<Keyframe> list = new List<Keyframe>();
		float num = 2f;
		timeStart = Mathf.Max(timeStart, 0.0001f);
		float num2 = timeStart;
		while ((double)num2 < (double)timeEnd)
		{
			float value = LogarithmicValue(num2, timeStart, logBase);
			float num3 = num2 / 50f;
			float num4 = (float)(((double)LogarithmicValue(num2 + num3, timeStart, logBase) - (double)LogarithmicValue(num2 - num3, timeStart, logBase)) / ((double)num3 * 2.0));
			list.Add(new Keyframe(num2, value, num4, num4));
			num2 *= num;
		}
		float value2 = LogarithmicValue(timeEnd, timeStart, logBase);
		float num5 = timeEnd / 50f;
		float num6 = (float)(((double)LogarithmicValue(timeEnd + num5, timeStart, logBase) - (double)LogarithmicValue(timeEnd - num5, timeStart, logBase)) / ((double)num5 * 2.0));
		list.Add(new Keyframe(timeEnd, value2, num6, num6));
		return new AnimationCurve(list.ToArray());
	}

	public static float LogarithmicValue(float distance, float minDistance, float rolloffScale)
	{
		if ((double)distance > (double)minDistance && (double)rolloffScale != 1.0)
		{
			distance -= minDistance;
			distance *= rolloffScale;
			distance += minDistance;
		}
		if ((double)distance < 9.99999997475243E-07)
		{
			distance = 1E-06f;
		}
		return minDistance / distance;
	}

	public static AnimationCurve Constant(float value)
	{
		AnimationCurve animationCurve = new AnimationCurve(new Keyframe(0f, value));
		animationCurve.preWrapMode = WrapMode.ClampForever;
		animationCurve.postWrapMode = WrapMode.ClampForever;
		return animationCurve;
	}
}
