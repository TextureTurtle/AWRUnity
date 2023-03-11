using UnityEngine;

namespace RamjetAnvil.Unity.Impero
{
	public class AxisAction : Action
	{
		private readonly SubAxisAction _positive;

		private readonly SubAxisAction _negative;

		private readonly float _range;

		public SubAxisAction Positive
		{
			get
			{
				return _positive;
			}
		}

		public SubAxisAction Negative
		{
			get
			{
				return _negative;
			}
		}

		public SubAxisAction this[Polarity polarity]
		{
			get
			{
				if (polarity != Polarity.Positive)
				{
					return _negative;
				}
				return _positive;
			}
		}

		public AxisAction(string identifier, string name, float range, SubAxisAction positiveAction, SubAxisAction negativeAction)
			: base(identifier, name, InputType.Axis)
		{
			_range = range;
			_negative = negativeAction;
			_positive = positiveAction;
		}

		public float Get()
		{
			return Mathf.Clamp(_positive.Get() + _negative.Get(), 0f - _range, _range);
		}
	}
}
