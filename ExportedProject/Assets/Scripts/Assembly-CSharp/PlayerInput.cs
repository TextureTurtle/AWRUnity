using RamjetAnvil.Unity.Impero;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
	private Player _player;

	[SerializeField]
	private float _lookHorizontalSensitivity = 5f;

	[SerializeField]
	private float _lookVerticalSensitivity = 5f;

	[SerializeField]
	private bool _smoothMouse;

	[SerializeField]
	private float _mouseSmoothing = 0.04f;

	private float _lookHorizontal;

	private float _lookVertical;

	private ActionMap _actionMap;

	public ActionMap ActionMap
	{
		get
		{
			return _actionMap;
		}
	}

	public void Initialize(Player player)
	{
		_player = player;
		_actionMap = SingletonComponent<AuroraInputSerializer>.Instance.Create(player.PlayerId);
		if (!SingletonComponent<AuroraInputSerializer>.Instance.Load(_actionMap))
		{
			SingletonComponent<AuroraInputSerializer>.Instance.LoadDefault(_actionMap);
		}
		InputManager.Instance.AddActionMap(_actionMap);
	}

	private void Update()
	{
		float axis = _actionMap.GetAxis("strafe");
		float axis2 = _actionMap.GetAxis("walk");
		float num = _actionMap.GetAxis("lookHorizontal") * _lookHorizontalSensitivity;
		float num2 = _actionMap.GetAxis("lookVertical") * _lookVerticalSensitivity;
		_lookHorizontal = ((!_smoothMouse) ? num : Mathf.Lerp(_lookHorizontal, num, 1f / _mouseSmoothing * Time.deltaTime));
		_lookVertical = ((!_smoothMouse) ? num2 : Mathf.Lerp(_lookVertical, num2, 1f / _mouseSmoothing * Time.deltaTime));
		bool buttonDown = _actionMap.GetButtonDown("use");
		bool buttonUp = _actionMap.GetButtonUp("use");
		bool buttonDown2 = _actionMap.GetButtonDown("cancel");
		bool buttonDown3 = _actionMap.GetButtonDown("pickup");
		bool button = _actionMap.GetButton("sprint");
		bool buttonDown4 = _actionMap.GetButtonDown("jump");
		_player.BodyController.Walk(axis, axis2, button);
		_player.BodyController.Look(_lookHorizontal);
		if (buttonDown4)
		{
			_player.BodyController.Jump();
		}
		_player.Camera.Look(_lookVertical);
		if (buttonDown)
		{
			_player.Use();
		}
		if (buttonUp)
		{
			_player.StopUse();
		}
		if (buttonDown2)
		{
			_player.Cancel();
		}
		if (buttonDown3)
		{
			_player.Pickup();
		}
	}
}
