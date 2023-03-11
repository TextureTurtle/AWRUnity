using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class OVRDevice : MonoBehaviour
{
	public float InitialPredictionTime = 0.05f;

	public float InitialAccelGain = 0.05f;

	private static bool OVRInit;

	public static int SensorCount;

	public static string DisplayDeviceName;

	public static int HResolution;

	public static int VResolution;

	public static float HScreenSize;

	public static float VScreenSize;

	public static float EyeToScreenDistance;

	public static float LensSeparationDistance;

	public static float LeftEyeOffset;

	public static float RightEyeOffset;

	public static float ScreenVCenter;

	public static float DistK0;

	public static float DistK1;

	public static float DistK2;

	public static float DistK3;

	private static float LensOffsetLeft;

	private static float LensOffsetRight;

	private static float DistortionFitX;

	private static float DistortionFitY = 1f;

	private static float PredictionTime;

	private static float AccelGain;

	private static float DistortionFitScale = 0.7f;

	[DllImport("OculusPlugin")]
	private static extern bool OVR_Initialize();

	[DllImport("OculusPlugin")]
	private static extern bool OVR_Destroy();

	[DllImport("OculusPlugin")]
	private static extern int OVR_GetSensorCount();

	[DllImport("OculusPlugin")]
	private static extern bool OVR_IsHMDPresent();

	[DllImport("OculusPlugin")]
	private static extern bool OVR_IsSensorPresent(int sensor);

	[DllImport("OculusPlugin")]
	private static extern bool OVR_GetSensorOrientation(int sensorID, ref float w, ref float x, ref float y, ref float z);

	[DllImport("OculusPlugin")]
	private static extern bool OVR_GetSensorPredictedOrientation(int sensorID, ref float w, ref float x, ref float y, ref float z);

	[DllImport("OculusPlugin")]
	private static extern bool OVR_GetSensorPredictionTime(int sensorID, ref float predictionTime);

	[DllImport("OculusPlugin")]
	private static extern bool OVR_SetSensorPredictionTime(int sensorID, float predictionTime);

	[DllImport("OculusPlugin")]
	private static extern bool OVR_GetSensorAccelGain(int sensorID, ref float accelGain);

	[DllImport("OculusPlugin")]
	private static extern bool OVR_SetSensorAccelGain(int sensorID, float accelGain);

	[DllImport("OculusPlugin")]
	private static extern bool OVR_EnableYawCorrection(int sensorID, float enable);

	[DllImport("OculusPlugin")]
	private static extern bool OVR_ResetSensorOrientation(int sensorID);

	[DllImport("OculusPlugin")]
	private static extern IntPtr OVR_GetDisplayDeviceName();

	[DllImport("OculusPlugin")]
	private static extern bool OVR_GetScreenResolution(ref int hResolution, ref int vResolution);

	[DllImport("OculusPlugin")]
	private static extern bool OVR_GetScreenSize(ref float hSize, ref float vSize);

	[DllImport("OculusPlugin")]
	private static extern bool OVR_GetEyeToScreenDistance(ref float eyeToScreenDistance);

	[DllImport("OculusPlugin")]
	private static extern bool OVR_GetInterpupillaryDistance(ref float interpupillaryDistance);

	[DllImport("OculusPlugin")]
	private static extern bool OVR_GetLensSeparationDistance(ref float lensSeparationDistance);

	[DllImport("OculusPlugin")]
	private static extern bool OVR_GetEyeOffset(ref float leftEye, ref float rightEye);

	[DllImport("OculusPlugin")]
	private static extern bool OVR_GetScreenVCenter(ref float vCenter);

	[DllImport("OculusPlugin")]
	private static extern bool OVR_GetDistortionCoefficients(ref float k0, ref float k1, ref float k2, ref float k3);

	[DllImport("OculusPlugin")]
	private static extern void OVR_ProcessLatencyInputs();

	[DllImport("OculusPlugin")]
	private static extern bool OVR_DisplayLatencyScreenColor(ref byte r, ref byte g, ref byte b);

	[DllImport("OculusPlugin")]
	private static extern IntPtr OVR_GetLatencyResultsString();

	private void Awake()
	{
		OVRInit = OVR_Initialize();
		if (OVRInit)
		{
			SensorCount = OVR_GetSensorCount();
			DisplayDeviceName += Marshal.PtrToStringAnsi(OVR_GetDisplayDeviceName());
			OVR_GetScreenResolution(ref HResolution, ref VResolution);
			OVR_GetScreenSize(ref HScreenSize, ref VScreenSize);
			OVR_GetEyeToScreenDistance(ref EyeToScreenDistance);
			OVR_GetLensSeparationDistance(ref LensSeparationDistance);
			OVR_GetEyeOffset(ref LeftEyeOffset, ref RightEyeOffset);
			OVR_GetScreenVCenter(ref ScreenVCenter);
			OVR_GetDistortionCoefficients(ref DistK0, ref DistK1, ref DistK2, ref DistK3);
			if (HScreenSize < 0.14f)
			{
				DistortionFitX = 0f;
				DistortionFitY = 1f;
				DistortionFitScale = 1f;
			}
			else
			{
				DistortionFitX = -1f;
				DistortionFitY = 0f;
			}
			CalculatePhysicalLensOffsets(ref LensOffsetLeft, ref LensOffsetRight);
			if (PredictionTime > 0f)
			{
				OVR_SetSensorPredictionTime(0, PredictionTime);
			}
			else
			{
				SetPredictionTime(0, InitialPredictionTime);
			}
			if (AccelGain > 0f)
			{
				OVR_SetSensorAccelGain(0, AccelGain);
			}
			else
			{
				SetAccelGain(0, InitialAccelGain);
			}
		}
	}

	private void Start()
	{
	}

	private void OnDestroy()
	{
		OVR_Destroy();
		OVRInit = false;
	}

	public static bool IsInitialized()
	{
		return OVRInit;
	}

	public static bool IsHMDPresent()
	{
		return OVR_IsHMDPresent();
	}

	public static bool IsSensorPresent(int sensor)
	{
		return OVR_IsSensorPresent(sensor);
	}

	public static bool GetOrientation(ref Quaternion q)
	{
		float w = 0f;
		float x = 0f;
		float y = 0f;
		float z = 0f;
		if (OVR_GetSensorOrientation(0, ref w, ref x, ref y, ref z))
		{
			q.w = w;
			q.x = 0f - x;
			q.y = 0f - y;
			q.z = z;
			return true;
		}
		return false;
	}

	public static bool GetPredictedOrientation(ref Quaternion q)
	{
		float w = 0f;
		float x = 0f;
		float y = 0f;
		float z = 0f;
		if (OVR_GetSensorPredictedOrientation(0, ref w, ref x, ref y, ref z))
		{
			q.w = w;
			q.x = 0f - x;
			q.y = 0f - y;
			q.z = z;
			return true;
		}
		return false;
	}

	public static bool ResetOrientation(int sensor)
	{
		return OVR_ResetSensorOrientation(sensor);
	}

	public static float GetPredictionTime(int sensor)
	{
		return PredictionTime;
	}

	public static bool SetPredictionTime(int sensor, float predictionTime)
	{
		if (predictionTime > 0f && OVR_SetSensorPredictionTime(sensor, predictionTime))
		{
			PredictionTime = predictionTime;
			return true;
		}
		return false;
	}

	public static float GetAccelGain(int sensor)
	{
		return AccelGain;
	}

	public static bool SetAccelGain(int sensor, float accelGain)
	{
		if (accelGain > 0f && OVR_SetSensorAccelGain(sensor, accelGain))
		{
			AccelGain = accelGain;
			return true;
		}
		return false;
	}

	public static bool GetDistortionCorrectionCoefficients(ref float k0, ref float k1, ref float k2, ref float k3)
	{
		if (!OVRInit)
		{
			return false;
		}
		k0 = DistK0;
		k1 = DistK1;
		k2 = DistK2;
		k3 = DistK3;
		return true;
	}

	public static bool SetDistortionCorrectionCoefficients(float k0, float k1, float k2, float k3)
	{
		if (!OVRInit)
		{
			return false;
		}
		DistK0 = k0;
		DistK1 = k1;
		DistK2 = k2;
		DistK3 = k3;
		return true;
	}

	public static bool GetPhysicalLensOffsets(ref float lensOffsetLeft, ref float lensOffsetRight)
	{
		if (!OVRInit)
		{
			return false;
		}
		lensOffsetLeft = LensOffsetLeft;
		lensOffsetRight = LensOffsetRight;
		return true;
	}

	public static bool GetIPD(ref float IPD)
	{
		if (!OVRInit)
		{
			return false;
		}
		OVR_GetInterpupillaryDistance(ref IPD);
		return true;
	}

	public static float CalculateAspectRatio()
	{
		if (Application.isEditor)
		{
			return (float)Screen.width * 0.5f / (float)Screen.height;
		}
		return (float)HResolution * 0.5f / (float)VResolution;
	}

	public static float VerticalFOV()
	{
		if (!OVRInit)
		{
			return 90f;
		}
		float num = VScreenSize / 2f * DistortionScale();
		return 114.59156f * Mathf.Atan(num / EyeToScreenDistance);
	}

	public static float DistortionScale()
	{
		if (OVRInit)
		{
			float num = 0f;
			if (Mathf.Abs(DistortionFitX) < 0.0001f && Mathf.Abs(DistortionFitY) < 0.0001f)
			{
				num = 1f;
			}
			else
			{
				float num2 = 0.5f * (float)Screen.width / (float)Screen.height;
				float num3 = DistortionFitX * DistortionFitScale - LensOffsetLeft;
				float num4 = DistortionFitY * DistortionFitScale / num2;
				float fitRadius = Mathf.Sqrt(num3 * num3 + num4 * num4);
				num = CalcScale(fitRadius);
			}
			if (num != 0f)
			{
				return num;
			}
		}
		return 1f;
	}

	public static void ProcessLatencyInputs()
	{
		OVR_ProcessLatencyInputs();
	}

	public static bool DisplayLatencyScreenColor(ref byte r, ref byte g, ref byte b)
	{
		return OVR_DisplayLatencyScreenColor(ref r, ref g, ref b);
	}

	public static IntPtr GetLatencyResultsString()
	{
		return OVR_GetLatencyResultsString();
	}

	public static float CalcScale(float fitRadius)
	{
		float num = fitRadius * fitRadius;
		float num2 = fitRadius * (DistK0 + DistK1 * num + DistK2 * num * num + DistK3 * num * num * num);
		return num2 / fitRadius;
	}

	public static bool CalculatePhysicalLensOffsets(ref float leftOffset, ref float rightOffset)
	{
		leftOffset = 0f;
		rightOffset = 0f;
		if (!OVRInit)
		{
			return false;
		}
		float num = HScreenSize * 0.5f;
		float num2 = LensSeparationDistance * 0.5f;
		leftOffset = (num - num2) / num * 2f - 1f;
		rightOffset = num2 / num * 2f - 1f;
		return true;
	}
}
