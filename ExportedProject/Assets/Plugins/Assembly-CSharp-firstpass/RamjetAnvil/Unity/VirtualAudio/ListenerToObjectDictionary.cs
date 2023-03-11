using System;
using RamjetAnvil.Unity.Collections;
using UnityEngine;

namespace RamjetAnvil.Unity.VirtualAudio
{
	[Serializable]
	public class ListenerToObjectDictionary : SerializableDictionary<VirtualAudioListener, GameObject>
	{
	}
}
