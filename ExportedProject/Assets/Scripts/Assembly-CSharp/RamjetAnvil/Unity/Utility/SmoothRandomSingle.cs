using UnityEngine;

namespace RamjetAnvil.Unity.Utility
{
	public class SmoothRandomSingle
	{
		private static FractalNoiseSingle _sNoiseSingle;

		public static Vector3 GetVector3(float speed)
		{
			float x = Time.time * 0.01f * speed;
			return new Vector3(Get().HybridMultifractal(x, 15.73f, 0.58f), Get().HybridMultifractal(x, 63.94f, 0.58f), Get().HybridMultifractal(x, 0.2f, 0.58f));
		}

		public static Vector3 GetSignedVector3(float speed)
		{
			float x = Time.time * 0.01f * speed;
			return new Vector3(Get().HybridMultifractal(x, 15.73f, 0.58f) * 2f - 1f, Get().HybridMultifractal(x, 63.94f, 0.58f) * 2f - 1f, Get().HybridMultifractal(x, 0.2f, 0.58f) * 2f - 1f);
		}

		public static float Get(float speed)
		{
			float num = Time.time * 0.01f * speed;
			return Get().HybridMultifractal(num * 0.01f, 15.7f, 0.65f);
		}

		private static FractalNoiseSingle Get()
		{
			if (_sNoiseSingle == null)
			{
				_sNoiseSingle = new FractalNoiseSingle(1.27f, 2.04f, 8.36f);
			}
			return _sNoiseSingle;
		}
	}
}
