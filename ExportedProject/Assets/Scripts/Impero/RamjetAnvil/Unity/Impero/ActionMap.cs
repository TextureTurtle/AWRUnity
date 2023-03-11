using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RamjetAnvil.Unity.Impero
{
	public class ActionMap
	{
		private int _playerId;

		private Dictionary<string, AxisAction> _axisMap;

		private Dictionary<string, ButtonAction> _buttonMap;

		public int PlayerId
		{
			get
			{
				return _playerId;
			}
		}

		public Dictionary<string, ButtonAction> ButtonMap
		{
			get
			{
				return _buttonMap;
			}
		}

		public Dictionary<string, AxisAction> AxisMap
		{
			get
			{
				return _axisMap;
			}
		}

		public List<ButtonAction> ButtonActions
		{
			get
			{
				return _buttonMap.Values.ToList();
			}
		}

		public List<AxisAction> AxisActions
		{
			get
			{
				return _axisMap.Values.ToList();
			}
		}

		public ActionMap(int playerId)
		{
			_playerId = playerId;
			_axisMap = new Dictionary<string, AxisAction>();
			_buttonMap = new Dictionary<string, ButtonAction>();
		}

		public void MapButton(string name, IInput input)
		{
			if (ButtonActionExists(name))
			{
				IButtonAdapter adapter = _buttonMap[name].Adapter;
				if (adapter != null)
				{
					InputManager.Instance.RemoveAdapter(adapter);
				}
				IButtonAdapter adapter2 = AdapterFactory.CreateButtonAdapter(input);
				InputManager.Instance.AddAdapter(adapter2);
				_buttonMap[name].Adapter = adapter2;
			}
		}

		public void MapAxis(string name, Polarity polarity, IInput input)
		{
			if (!AxisActionExists(name))
			{
				UnityEngine.Debug.Log("No suitable action with name: " + name + " found for input: " + input);
				return;
			}
			IAxisAdapter axisAdapter = AdapterFactory.CreateAxisAdapter(input);
			IAxisAdapter adapter = _axisMap[name][polarity].Adapter;
			if (adapter != null)
			{
				InputManager.Instance.RemoveAdapter(adapter);
				axisAdapter.Sensitivity = adapter.Sensitivity;
			}
			InputManager.Instance.AddAdapter(axisAdapter);
			_axisMap[name][polarity].Adapter = axisAdapter;
		}

		public float GetAxis(string name)
		{
			if (AxisActionExists(name))
			{
				return _axisMap[name].Get();
			}
			return 0f;
		}

		public bool GetButton(string name)
		{
			if (ButtonActionExists(name))
			{
				return _buttonMap[name].Get();
			}
			return false;
		}

		public bool GetButtonDown(string name)
		{
			if (ButtonActionExists(name))
			{
				return _buttonMap[name].GetDown();
			}
			return false;
		}

		public bool GetButtonUp(string name)
		{
			if (ButtonActionExists(name))
			{
				return _buttonMap[name].GetUp();
			}
			return false;
		}

		private bool AxisActionExists(string name)
		{
			if (!_axisMap.ContainsKey(name))
			{
				return false;
			}
			return true;
		}

		private bool ButtonActionExists(string name)
		{
			if (!_buttonMap.ContainsKey(name))
			{
				return false;
			}
			return true;
		}
	}
}
