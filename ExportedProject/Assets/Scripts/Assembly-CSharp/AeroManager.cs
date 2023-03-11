using System;
using LibNoise.Unity;
using LibNoise.Unity.Generator;
using UnityEngine;

public class AeroManager : SingletonComponent<AeroManager>
{
	[SerializeField]
	private float airDensity = 1.293f;

	[SerializeField]
	private float _seaTemp = 21f;

	[SerializeField]
	private float _stratosTemp = -50f;

	[SerializeField]
	private float _seaAltitude;

	[SerializeField]
	private float _stratosAltitude = 2000f;

	[SerializeField]
	private AnimationCurve _globalHeightMix = AnimationCurve.Linear(0f, 0.1f, 1f, 1f);

	[SerializeField]
	private AnimationCurve _windAmplitude = AnimationCurve.Linear(0f, 0.5f, 1f, 1f);

	[SerializeField]
	private double _minWindAmplitudeFactor = 0.1;

	[SerializeField]
	private AnimationCurve _poleWindFilter = AnimationCurve.Linear(0f, 1f, 1f, 0.3f);

	[SerializeField]
	private double _zoneSize = 2000.0;

	[SerializeField]
	private double _windHorizontalFrequency = 0.0003;

	[SerializeField]
	private double _windVerticalFrequency = 0.001;

	[SerializeField]
	private double _worldOuterRadius = 28000.0;

	[SerializeField]
	private double _maxAltitude = 3000.0;

	[SerializeField]
	private double _maxWindHorAmplitude = 120.0;

	[SerializeField]
	private double _maxWindVertAmplitude = 7.0;

	[SerializeField]
	private float _windChangeFrequency = 0.001f;

	[SerializeField]
	private bool _forceCustomSeed = true;

	[SerializeField]
	private int _customSeed = 12345;

	private DVector3 _northPolePosition;

	private Perlin _perlinX;

	private Perlin _perlinY;

	private Perlin _perlinZ;

	private void Awake()
	{
		double num = _zoneSize * 0.5;
		_northPolePosition = new DVector3(num, 0.0, num);
		_perlinX = new Perlin();
		_perlinY = new Perlin();
		_perlinZ = new Perlin();
		int num2 = ((!_forceCustomSeed) ? new System.Random().Next() : _customSeed);
		_perlinX.Quality = QualityMode.Low;
		_perlinX.Seed = num2 - 1;
		_perlinX.OctaveCount = 1;
		_perlinX.Frequency = _windHorizontalFrequency;
		_perlinY.Quality = QualityMode.Low;
		_perlinY.Seed = num2;
		_perlinY.OctaveCount = 1;
		_perlinY.Frequency = _windVerticalFrequency;
		_perlinZ.Quality = QualityMode.Low;
		_perlinZ.Seed = num2 + 1;
		_perlinZ.OctaveCount = 1;
		_perlinZ.Frequency = _windHorizontalFrequency;
	}

	public float GetAirDensity(Vector3 position)
	{
		return airDensity;
	}

	public float GetTemperatureAtPoint(Vector3 position)
	{
		float num = _stratosAltitude - _seaAltitude;
		return Mathf.Lerp(_seaTemp, _stratosTemp, position.y / num);
	}

	public Vector3 GetWindVelocity(Vector3 position)
	{
		return GetWindVelocity(position, true);
	}

	public Vector3 GetWindVelocity(Vector3 position, bool includeTurbulence)
	{
		DVector3 dVector = SingletonComponent<World>.Instance.LocalPointToWorld(position);
		double num = dVector.Y / _maxAltitude;
		double num2 = _globalHeightMix.Evaluate((float)num);
		DVector3 dVector2 = _northPolePosition - dVector;
		dVector2.Y = 0.0;
		double getMagnitude = dVector2.GetMagnitude;
		DVector3 dVector3 = dVector2 / getMagnitude;
		double num3 = Mathd.Clamp01(getMagnitude / _worldOuterRadius);
		float num4 = Time.time * _windChangeFrequency;
		double num5 = _windAmplitude.Evaluate(1f - (float)num3);
		double num6 = _maxWindHorAmplitude * num5 * num2;
		double num7 = _maxWindVertAmplitude * num5 * num2;
		double x = dVector.X + (double)num4;
		double y = dVector.Y + (double)num4;
		double z = dVector.Z + (double)num4;
		double x2 = _perlinX.GetValue(x, y, z) * num6;
		double y2 = _perlinY.GetValue(x, y, z) * num7;
		double z2 = _perlinZ.GetValue(x, y, z) * num6;
		DVector3 dVector4 = new DVector3(x2, y2, z2);
		double num8 = _poleWindFilter.Evaluate(1f - (float)num3);
		double value = DVector3.Angle(dVector4, dVector3) / 180.0 * num8;
		dVector4 -= DVector3.Project(dVector4, dVector3) * Mathd.Clamp01(value);
		dVector4 += dVector3 * Mathd.Clamp(value, 0.0, num6);
		double val = num6 * _minWindAmplitudeFactor;
		dVector4 = dVector4.GetNormalized * Math.Max(dVector4.GetMagnitude, val);
		return DVector3.ToVector3(dVector4);
	}
}
