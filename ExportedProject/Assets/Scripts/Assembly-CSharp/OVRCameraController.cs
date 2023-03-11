using UnityEngine;

public class OVRCameraController : MonoBehaviour
{
	[SerializeField]
	private Camera[] _cameraLeft;

	[SerializeField]
	private Camera[] _cameraRight;

	[SerializeField]
	private Transform _parentTransform;

	[SerializeField]
	private Vector3 _eyeOffset = new Vector3(0f, 0.15f, 0.9f);

	private bool _isDirty;

	[HideInInspector]
	[SerializeField]
	private float _ipd = 0.064f;

	private float _lensOffsetLeft;

	private float _lensOffsetRight;

	private float _verticalFov = 90f;

	private float _aspectRatio = 1f;

	private float _distK0;

	private float _distK1;

	private float _distK2;

	private float _distK3;

	private Quaternion _baseRotation = Quaternion.identity;

	public bool PredictionOn = true;

	public bool UpdateOrientationInPreRender;

	public bool WireMode;

	public Color BackgroundColor = new Color(0.192f, 0.302f, 0.475f, 1f);

	public float NearClipPlane = 0.15f;

	public float FarClipPlane = 1000f;

	public Transform ParentTransform
	{
		get
		{
			return _parentTransform;
		}
		set
		{
			_parentTransform = value;
		}
	}

	public float Ipd
	{
		get
		{
			return _ipd;
		}
		set
		{
			_ipd = value;
			_isDirty = true;
		}
	}

	public float VerticalFov
	{
		get
		{
			return _verticalFov;
		}
		set
		{
			_verticalFov = value;
			_isDirty = true;
		}
	}

	public float AspectRatio
	{
		get
		{
			return _aspectRatio;
		}
		set
		{
			_aspectRatio = value;
			_isDirty = true;
		}
	}

	public Quaternion BaseRotation
	{
		get
		{
			return _baseRotation;
		}
		set
		{
			_baseRotation = value;
		}
	}

	private void Start()
	{
		if (_cameraLeft.Length == 0 || _cameraRight.Length == 0)
		{
			Debug.LogWarning("One or more cameras not assigned to OVRCameraController");
		}
		Initialize();
		UpdateCameras();
	}

	private void LateUpdate()
	{
		if (_isDirty)
		{
			UpdateCameras();
		}
	}

	public void Initialize()
	{
		_ipd = 0.07f;
		OVRDevice.CalculatePhysicalLensOffsets(ref _lensOffsetLeft, ref _lensOffsetRight);
		_verticalFov = OVRDevice.VerticalFOV();
		_aspectRatio = OVRDevice.CalculateAspectRatio();
		OVRDevice.GetDistortionCorrectionCoefficients(ref _distK0, ref _distK1, ref _distK2, ref _distK3);
		_baseRotation = base.transform.localRotation;
	}

	public void UpdateCameras()
	{
		float distanceOffset = 0.5f + _lensOffsetLeft * 0.5f;
		float lensOffsetLeft = _lensOffsetLeft;
		float eyeHorizontalOffset = (0f - _ipd) * 0.5f;
		for (int i = 0; i < _cameraLeft.Length; i++)
		{
			UpdateCamera(_cameraLeft[i], distanceOffset, lensOffsetLeft, eyeHorizontalOffset);
		}
		distanceOffset = 0.5f + _lensOffsetRight * 0.5f;
		lensOffsetLeft = _lensOffsetRight;
		eyeHorizontalOffset = _ipd * 0.5f;
		for (int j = 0; j < _cameraRight.Length; j++)
		{
			UpdateCamera(_cameraRight[j], distanceOffset, lensOffsetLeft, eyeHorizontalOffset);
		}
		_isDirty = false;
	}

	private void UpdateCamera(Camera stereoCamera, float distanceOffset, float perspOffset, float eyeHorizontalOffset)
	{
		stereoCamera.fov = _verticalFov;
		stereoCamera.aspect = _aspectRatio;
		OVRLensCorrection component = stereoCamera.GetComponent<OVRLensCorrection>();
		component._Center.x = distanceOffset;
		ConfigureCameraLensCorrection(component);
		Vector3 perspectiveOffset = Vector3.right * perspOffset;
		stereoCamera.GetComponent<OVRCamera>().SetPerspectiveOffset(perspectiveOffset);
		stereoCamera.transform.localPosition = _eyeOffset + Vector3.right * eyeHorizontalOffset;
		stereoCamera.backgroundColor = BackgroundColor;
		stereoCamera.nearClipPlane = NearClipPlane;
		stereoCamera.farClipPlane = FarClipPlane;
	}

	private void ConfigureCameraLensCorrection(OVRLensCorrection lensCorrection)
	{
		float num = 1f / OVRDevice.DistortionScale();
		float num2 = OVRDevice.CalculateAspectRatio();
		float num3 = 1f;
		float num4 = 1f;
		lensCorrection._Scale.x = num3 / 2f * num;
		lensCorrection._Scale.y = num4 / 2f * num * num2;
		lensCorrection._ScaleIn.x = 2f / num3;
		lensCorrection._ScaleIn.y = 2f / num4 / num2;
		lensCorrection._HmdWarpParam.x = _distK0;
		lensCorrection._HmdWarpParam.y = _distK1;
		lensCorrection._HmdWarpParam.z = _distK2;
	}

	public void UpdateOrientation()
	{
		Quaternion q = Quaternion.identity;
		if (!PredictionOn)
		{
			OVRDevice.GetOrientation(ref q);
		}
		else
		{
			OVRDevice.GetPredictedOrientation(ref q);
		}
		OVRDevice.ProcessLatencyInputs();
		Quaternion quaternion = _baseRotation * q;
		if ((bool)_parentTransform)
		{
			base.transform.position = _parentTransform.position;
			base.transform.rotation = _parentTransform.rotation * quaternion;
		}
		else
		{
			base.transform.localRotation = quaternion;
		}
	}

	public void GetDistortionCoefs(ref float distK0, ref float distK1, ref float distK2, ref float distK3)
	{
		distK0 = _distK0;
		distK1 = _distK1;
		distK2 = _distK2;
		distK3 = _distK3;
	}

	public void SetDistortionCoefs(float distK0, float distK1, float distK2, float distK3)
	{
		_distK0 = distK0;
		_distK1 = distK1;
		_distK2 = distK2;
		_distK3 = distK3;
		_isDirty = true;
	}
}
