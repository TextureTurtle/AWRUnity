using System.Collections.Generic;
using RamjetAnvil.Unity.Utility;
using UnityEngine;

namespace RamjetAnvil.Unity.VirtualAudio
{
	[RequireComponent(typeof(VirtualAudioObject))]
	public abstract class VirtualAudioComponent<TR> : VirtualAudioComponentBase where TR : Behaviour
	{
		private VirtualAudioObject _virtualObject;

		private bool _initializedInPlayMode;

		private Dictionary<VirtualAudioListener, TR> _realComponents;

		private TR _propertyHelper;

		public bool InitializedInPlayMode
		{
			get
			{
				return _initializedInPlayMode;
			}
		}

		public IEnumerable<TR> ListenedComponents
		{
			get
			{
				return (_realComponents == null) ? null : _realComponents.Values;
			}
		}

		public TR this[VirtualAudioListener listener]
		{
			get
			{
				return _realComponents[listener];
			}
		}

		public int RealComponentCount
		{
			get
			{
				return (_realComponents != null) ? _realComponents.Count : 0;
			}
		}

		public VirtualAudioObject VirtualObject
		{
			get
			{
				return _virtualObject;
			}
		}

		protected TR PropertyHelper
		{
			get
			{
				return _propertyHelper;
			}
		}

		private void Reset()
		{
			if (!Application.isPlaying)
			{
			}
		}

		protected virtual void Awake()
		{
			if (!_initializedInPlayMode)
			{
				InitializeInPlayMode();
			}
		}

		protected virtual void OnDestroy()
		{
			if ((bool)_virtualObject)
			{
				_virtualObject.UnregisterVirtualComponent(this);
			}
			if (Application.isPlaying)
			{
				DestroyRealComponents();
			}
		}

		protected virtual void OnEnable()
		{
			SetRealComponentsEnabled(true);
		}

		protected virtual void OnDisable()
		{
			SetRealComponentsEnabled(false);
		}

		public virtual void InitializeInPlayMode()
		{
			if (!_initializedInPlayMode)
			{
				_realComponents = new Dictionary<VirtualAudioListener, TR>();
				_virtualObject = GetComponent<VirtualAudioObject>();
				if (!_virtualObject.InitializedInPlayMode)
				{
					_virtualObject.InitializeInPlayMode();
				}
				_virtualObject.RegisterVirtualComponent(this);
				_initializedInPlayMode = true;
			}
		}

		public override void OnListenerAdded(VirtualAudioListener listener, GameObject realObject)
		{
			TR val = realObject.AddComponent<TR>();
			SynchronizeComponentProperties(val);
			_realComponents.Add(listener, val);
			if ((Object)_propertyHelper == (Object)null)
			{
				_propertyHelper = val;
			}
		}

		public override void OnListenerRemoved(VirtualAudioListener listener)
		{
			TR val = _realComponents[listener];
			if ((Object)_propertyHelper == (Object)val)
			{
				_propertyHelper = GetAvailablePropertyHelper();
			}
			_realComponents.Remove(listener);
			Object.Destroy(val);
		}

		private void SetRealComponentsEnabled(bool enabled)
		{
			if (_realComponents == null)
			{
				return;
			}
			foreach (KeyValuePair<VirtualAudioListener, TR> realComponent in _realComponents)
			{
				if ((bool)(Object)realComponent.Value)
				{
					TR value = realComponent.Value;
					value.enabled = enabled;
				}
			}
		}

		public override void DestroyRealComponents()
		{
			foreach (KeyValuePair<VirtualAudioListener, TR> realComponent in _realComponents)
			{
				if ((bool)(Object)realComponent.Value)
				{
					MonoBehaviourExtensions.DestroyAgnostic(realComponent.Value);
				}
			}
			_realComponents.Clear();
		}

		private TR GetAvailablePropertyHelper()
		{
			if (_realComponents.Count > 0)
			{
				return _realComponents.Values.GetEnumerator().Current;
			}
			return (TR)null;
		}

		public abstract void InitializeWithRealComponentProperties(TR component);

		protected abstract void SynchronizeComponentProperties(TR component);
	}
}
