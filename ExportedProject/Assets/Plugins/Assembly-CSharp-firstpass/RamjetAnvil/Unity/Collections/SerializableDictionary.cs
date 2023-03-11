using System;
using System.Collections.Generic;
using UnityEngine;

namespace RamjetAnvil.Unity.Collections
{
	[Serializable]
	public class SerializableDictionary<TKey, TValue>
	{
		[SerializeField]
		private List<TKey> _keys;

		[SerializeField]
		private List<TValue> _values;

		public int Count
		{
			get
			{
				return _keys.Count;
			}
		}

		public TValue this[TKey key]
		{
			get
			{
				if (!_keys.Contains(key))
				{
					throw new Exception("Dictionary does not contain entry for item");
				}
				int index = _keys.IndexOf(key);
				return _values[index];
			}
			set
			{
				if (!_keys.Contains(key))
				{
					throw new Exception("Dictionary does not contain entry for item");
				}
				int index = _keys.IndexOf(key);
				_values[index] = value;
			}
		}

		public IList<TValue> Values
		{
			get
			{
				return _values;
			}
		}

		public IList<TKey> Keys
		{
			get
			{
				return _keys;
			}
		}

		public SerializableDictionary()
		{
			if (_keys == null)
			{
				_keys = new List<TKey>();
			}
			if (_values == null)
			{
				_values = new List<TValue>();
			}
		}

		public void Add(TKey key, TValue value)
		{
			if (_keys.Contains(key))
			{
				throw new Exception("Dictionary already contains entry for item");
			}
			_keys.Add(key);
			_values.Add(value);
		}

		public bool ContainsKey(TKey key)
		{
			return _keys.Contains(key);
		}

		public bool Remove(TKey key)
		{
			int index = _keys.IndexOf(key);
			_values.RemoveAt(index);
			return _keys.Remove(key);
		}
	}
}
