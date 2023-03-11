using UnityEngine;

public class CircularBuffer<T> where T : new()
{
	private T[] _items;

	private int _maxCount;

	private int _startIndex;

	private int _count;

	public CircularBuffer(int count)
	{
		_maxCount = Mathf.ClosestPowerOfTwo(count);
		_items = new T[_maxCount];
		for (int i = 0; i < _maxCount; i++)
		{
			_items[i] = new T();
		}
	}

	public int ToCircularIndex(int index)
	{
		return Mathx.FastSqrModulo(_startIndex + index, _maxCount);
	}

	public void AddAtHead()
	{
	}

	public void RemoveAtTail()
	{
	}
}
