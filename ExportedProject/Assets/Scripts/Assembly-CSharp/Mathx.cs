using System;
using UnityEngine;

public class Mathx
{
	public const float TWOPI = (float)Math.PI * 2f;

	public const float HALFPI = (float)Math.PI / 2f;

	public const float ONEOVER90 = 1f / 90f;

	public const float ONEOVER180 = 1f / 180f;

	public static float WrapAngle(float angle)
	{
		if (angle < -360f)
		{
			angle += 360f;
		}
		if (angle > 360f)
		{
			angle -= 360f;
		}
		return angle;
	}

	public static float ClampAngle(float angle, float min, float max, float wrapPoint)
	{
		if (angle < 0f - wrapPoint)
		{
			angle += 360f;
		}
		if (angle > wrapPoint)
		{
			angle -= 360f;
		}
		return Mathf.Clamp(angle, min, max);
	}

	public static float ClampUnitValue(float value)
	{
		if (value > 1f)
		{
			value = 1f;
		}
		if (value < -1f)
		{
			value = -1f;
		}
		return value;
	}

	public static int FastSqrModulo(int num, int mod)
	{
		return num & (mod - 1);
	}

	public static float AngleAroundAxis(Vector3 v1, Vector3 v2, Vector3 a)
	{
		return Mathf.Atan2(Vector3.Dot(a, Vector3.Cross(v1, v2)), Vector3.Dot(v1, v2)) * 57.29578f;
	}

	public static Vector3 ProjectOnPlane(Vector3 inVector, Vector3 planeNormal)
	{
		return Vector3.Cross(planeNormal, Vector3.Cross(inVector, planeNormal));
	}
}
