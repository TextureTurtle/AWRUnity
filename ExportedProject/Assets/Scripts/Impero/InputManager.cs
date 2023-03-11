using System.Collections;
using System.Collections.Generic;
using RamjetAnvil.Unity.Impero;
using UnityEngine;

[AddComponentMenu("Impero/Input Manager")]
public class InputManager : MonoBehaviour
{
	public delegate void ActionRemapResultHandler(ActionMap map, string name, IInput input);

	private static InputManager _instance;

	[SerializeField]
	private float _maxRemapDuration = 5f;

	private List<IInputApi> _apis;

	private List<IPeripheral> _peripherals;

	private List<ActionMap> _actionMaps;

	private List<IAdapter> _adapters;

	private ButtonInput _escape;

	private bool _isRemapping;

	public static InputManager Instance
	{
		get
		{
			if (!_instance)
			{
				_instance = Object.FindObjectOfType(typeof(InputManager)) as InputManager;
				if (!_instance)
				{
					GameObject gameObject = new GameObject("_" + typeof(InputManager).Name);
					_instance = gameObject.AddComponent<InputManager>();
				}
			}
			if (!_instance)
			{
				return null;
			}
			return _instance;
		}
	}

	public bool IsRemapping
	{
		get
		{
			return _isRemapping;
		}
	}

	private void Awake()
	{
		_apis = new List<IInputApi>();
		_peripherals = new List<IPeripheral>();
		_actionMaps = new List<ActionMap>();
		_adapters = new List<IAdapter>();
		InitializePeripherals();
	}

	private void InitializePeripherals()
	{
		UnityInputApi unityInputApi = new UnityInputApi();
		_apis.Add(unityInputApi);
		List<IPeripheral> list = unityInputApi.Initialize();
		_peripherals.AddRange(list);
		string text = "InputManager: Initialized peripherals:\n";
		foreach (IPeripheral peripheral in _peripherals)
		{
			text = text + "\t" + peripheral.Identifier + "\n";
		}
		Debug.Log(text);
		UnityKeyboardPeripheral unityKeyboardPeripheral = list[0] as UnityKeyboardPeripheral;
		_escape = new ButtonInput(unityKeyboardPeripheral, unityKeyboardPeripheral.KeyCodeToButtonIndex(KeyCode.Escape));
	}

	public void AddActionMap(ActionMap map)
	{
		if (!_actionMaps.Contains(map))
		{
			_actionMaps.Add(map);
		}
	}

	public void RemoveActionMap(ActionMap map)
	{
		if (_actionMaps.Contains(map))
		{
			_actionMaps.Remove(map);
		}
	}

	public void AddAdapter(IAdapter adapter)
	{
		if (!_adapters.Contains(adapter))
		{
			_adapters.Add(adapter);
		}
	}

	public void RemoveAdapter(IAdapter adapter)
	{
		if (_adapters.Contains(adapter))
		{
			_adapters.Remove(adapter);
		}
	}

	public void RemapButton(ActionMap map, string actionName)
	{
		RemapButton(map, actionName, null);
	}

	public void RemapAxis(ActionMap map, string actionName, Polarity polarity)
	{
		RemapAxis(map, actionName, polarity, null);
	}

	public void CancelRemap()
	{
		_isRemapping = false;
	}

	public void RemapButton(ActionMap map, string actionName, ActionRemapResultHandler onActionRemapped)
	{
		StartCoroutine(RemapButtonAsync(map, actionName, onActionRemapped));
	}

	private IEnumerator RemapButtonAsync(ActionMap map, string actionName, ActionRemapResultHandler onActionRemapped)
	{
		_isRemapping = true;
		float timer = 0f;
		float lastTime = Time.realtimeSinceStartup;
		while (_isRemapping && timer < _maxRemapDuration)
		{
			foreach (IPeripheral peripheral in _peripherals)
			{
				ButtonInput activeButton = GetActiveButton(peripheral);
				if (activeButton == _escape)
				{
					_isRemapping = false;
					break;
				}
				if (activeButton != null)
				{
					map.MapButton(actionName, activeButton);
					_isRemapping = false;
					break;
				}
				AxisInput activeAxis = GetActiveAxis(peripheral);
				if (activeAxis != null)
				{
					map.MapButton(actionName, activeAxis);
					_isRemapping = false;
					break;
				}
			}
			timer += Time.realtimeSinceStartup - lastTime;
			lastTime = Time.realtimeSinceStartup;
			yield return new WaitForEndOfFrame();
		}
		_isRemapping = false;
		if (onActionRemapped != null)
		{
			onActionRemapped(map, actionName, null);
		}
	}

	public void RemapAxis(ActionMap map, string actionName, Polarity polarity, ActionRemapResultHandler onActionRemapped)
	{
		StartCoroutine(RemapAxisAsync(map, actionName, polarity, onActionRemapped));
	}

	private IEnumerator RemapAxisAsync(ActionMap map, string actionName, Polarity polarity, ActionRemapResultHandler onActionRemapped)
	{
		_isRemapping = true;
		float timer = 0f;
		float lastTime = Time.realtimeSinceStartup;
		while (_isRemapping && timer < _maxRemapDuration)
		{
			foreach (IPeripheral peripheral in _peripherals)
			{
				AxisInput activeAxis = GetActiveAxis(peripheral);
				if (activeAxis != null)
				{
					map.MapAxis(actionName, polarity, activeAxis);
					_isRemapping = false;
					break;
				}
				ButtonInput activeButton = GetActiveButton(peripheral);
				if (activeButton == _escape)
				{
					_isRemapping = false;
					break;
				}
				if (activeButton != null)
				{
					map.MapAxis(actionName, polarity, activeButton);
					_isRemapping = false;
					break;
				}
			}
			timer += Time.realtimeSinceStartup - lastTime;
			lastTime = Time.realtimeSinceStartup;
			yield return new WaitForEndOfFrame();
		}
		_isRemapping = false;
		if (onActionRemapped != null)
		{
			onActionRemapped(map, actionName, null);
		}
	}

	private static AxisInput GetActiveAxis(IPeripheral peripheral)
	{
		for (int i = 0; i < peripheral.NumAxes; i++)
		{
			float axis = peripheral.GetAxis(i);
			if (Mathf.Abs(axis) > peripheral.MappingThreshold)
			{
				return new AxisInput(peripheral, i, (axis > 0f) ? Polarity.Positive : Polarity.Negative);
			}
		}
		return null;
	}

	private static ButtonInput GetActiveButton(IPeripheral peripheral)
	{
		for (int i = 0; i < peripheral.NumButtons; i++)
		{
			if (peripheral.GetButton(i))
			{
				return new ButtonInput(peripheral, i);
			}
		}
		return null;
	}

	public IPeripheral FindUnusedPeripheralById(string name, int playerId)
	{
		foreach (IPeripheral peripheral in _peripherals)
		{
			if (peripheral.Identifier == name && (playerId == peripheral.PlayerId || peripheral.PlayerId == -1))
			{
				return peripheral;
			}
		}
		return null;
	}

	public string GetInputName(IInput input)
	{
		switch (input.InputType)
		{
		case InputType.Button:
			return input.Peripheral.GetHumanReadableButtonName(input.Index);
		case InputType.Axis:
		{
			IAxis axis = input as IAxis;
			return input.Peripheral.GetHumanReadableAxisName(input.Index) + ((axis.Polarity == Polarity.Negative) ? " -" : " +");
		}
		default:
			return "Undefined";
		}
	}

	private void Update()
	{
		foreach (IInputApi api in _apis)
		{
			api.Update();
		}
		foreach (IAdapter adapter in _adapters)
		{
			adapter.Update();
		}
	}
}
