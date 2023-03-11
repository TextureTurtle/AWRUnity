using UnityEngine;

namespace RamjetAnvil.Unity.Utility
{
	public class SmoothRandomDouble
	{
		private static FractalNoiseDouble s_Noise;

		public static DVector3 GetVector3(double speed)
		{
			double x = (double)Time.time * 0.01 * speed;
			return new DVector3(Get().HybridMultifractal(x, 15.73, 0.58), Get().HybridMultifractal(x, 63.94, 0.58), Get().HybridMultifractal(x, 0.2, 0.58));
		}

		public static DVector3 GetSignedVector3(double speed)
		{
			double x = (double)Time.time * 0.01 * speed;
			return new DVector3(Get().HybridMultifractal(x, 15.73, 0.58) * 2.0 - 1.0, Get().HybridMultifractal(x, 63.94, 0.58) * 2.0 - 1.0, Get().HybridMultifractal(x, 0.2, 0.58) * 2.0 - 1.0);
		}

		public static double Get(double speed)
		{
			double num = (double)Time.time * 0.01 * speed;
			return Get().HybridMultifractal(num * 0.01, 15.7, 0.65);
		}

		private static FractalNoiseDouble Get()
		{
			if (s_Noise == null)
			{
				s_Noise = new FractalNoiseDouble(1.27, 2.04, 8.36);
			}
			return s_Noise;
		}
	}
}
