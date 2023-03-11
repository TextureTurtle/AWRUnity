using System;
using UnityEngine;

public class Balloon : MonoBehaviour
{
	[SerializeField]
	private Drag _drag;

	[SerializeField]
	private float _minDrag = 0.1f;

	[SerializeField]
	private float _maxDrag = 0.5f;

	[SerializeField]
	private bool _overrideStartTemperature = true;

	[SerializeField]
	private float _startTemperature = 120f;

	[SerializeField]
	private float _maxTemperature = 250f;

	[SerializeField]
	private float _heatToForce = 5f;

	[SerializeField]
	private float _coolDownRate = 0.1f;

	[SerializeField]
	private float _depressurizeCoolDownRate = 1f;

	[SerializeField]
	private float _liftDropoffStartAltitude = 2500f;

	[SerializeField]
	private float _liftDropoffEndAltitude = 3500f;

	private float _temperature;

	private float _ambientTemperature;

	private float _liftForce;

	private bool _depressurizeInput;

	public float Temperature
	{
		get
		{
			return _temperature;
		}
	}

	public float MaxTemperature
	{
		get
		{
			return _maxTemperature;
		}
	}

	public float AmbientTemperature
	{
		get
		{
			return _ambientTemperature;
		}
	}

	public float LiftForce
	{
		get
		{
			return _liftForce;
		}
	}

	public event Action<bool> OnOpenReleaseValve;

	public void AddEnergy(float energy)
	{
		float num = energy * Mathf.Clamp01(1f - Mathf.Pow(_temperature / _maxTemperature, 2f));
		_temperature += num;
	}

	public void OpenReleaseValve(bool open)
	{
		_depressurizeInput = open;
		if (this.OnOpenReleaseValve != null)
		{
			this.OnOpenReleaseValve(open);
		}
	}

	private void Start()
	{
		_temperature = ((!_overrideStartTemperature) ? SingletonComponent<AeroManager>.Instance.GetTemperatureAtPoint(base.transform.position) : _startTemperature);
	}

	private void Update()
	{
		_ambientTemperature = SingletonComponent<AeroManager>.Instance.GetTemperatureAtPoint(base.transform.position);
		float num = ((!_depressurizeInput) ? _coolDownRate : _depressurizeCoolDownRate);
		_temperature = Mathf.Lerp(_temperature, _ambientTemperature, num * Time.deltaTime);
		_drag.CDrag = Mathf.Clamp(_temperature / (_maxTemperature * 2f), _minDrag, _maxDrag);
		float num2 = 1f - Mathf.Clamp01((base.transform.position.y - _liftDropoffStartAltitude) / (_liftDropoffEndAltitude - _liftDropoffStartAltitude));
		_liftForce = Mathf.Clamp(_temperature - _ambientTemperature, 0f, float.MaxValue) * _heatToForce * num2;
	}

	private void FixedUpdate()
	{
		base.rigidbody.AddForce(0f, _liftForce, 0f);
	}
}
