using System;

namespace RamjetAnvil.Unity.Utility
{
	public class FractalNoiseDouble
	{
		private PerlinDouble m_Noise;

		private double[] m_Exponent;

		private int m_IntOctaves;

		private double m_Octaves;

		private double m_Lacunarity;

		public FractalNoiseDouble(double inH, double inLacunarity, double inOctaves)
			: this(inH, inLacunarity, inOctaves, null)
		{
		}

		public FractalNoiseDouble(double inH, double inLacunarity, double inOctaves, PerlinDouble noise)
		{
			m_Lacunarity = inLacunarity;
			m_Octaves = inOctaves;
			m_IntOctaves = (int)inOctaves;
			m_Exponent = new double[m_IntOctaves + 1];
			double num = 1.0;
			for (int i = 0; i < m_IntOctaves + 1; i++)
			{
				m_Exponent[i] = Math.Pow(m_Lacunarity, 0.0 - inH);
				num *= m_Lacunarity;
			}
			if (noise == null)
			{
				m_Noise = new PerlinDouble();
			}
			else
			{
				m_Noise = noise;
			}
		}

		public double HybridMultifractal(double x, double y, double offset)
		{
			double num = (m_Noise.Noise(x, y) + offset) * m_Exponent[0];
			double num2 = num;
			x *= m_Lacunarity;
			y *= m_Lacunarity;
			int i;
			for (i = 1; i < m_IntOctaves; i++)
			{
				if (num2 > 1.0)
				{
					num2 = 1.0;
				}
				double num3 = (m_Noise.Noise(x, y) + offset) * m_Exponent[i];
				num += num2 * num3;
				num2 *= num3;
				x *= m_Lacunarity;
				y *= m_Lacunarity;
			}
			double num4 = m_Octaves - (double)m_IntOctaves;
			return num + num4 * m_Noise.Noise(x, y) * m_Exponent[i];
		}

		public double RidgedMultifractal(double x, double y, double offset, double gain)
		{
			double num = Math.Abs(m_Noise.Noise(x, y));
			num = offset - num;
			num *= num;
			double num2 = num;
			double num3 = 1.0;
			for (int i = 1; i < m_IntOctaves; i++)
			{
				x *= m_Lacunarity;
				y *= m_Lacunarity;
				num3 = num * gain;
				num3 = Mathd.Clamp01(num3);
				num = Math.Abs(m_Noise.Noise(x, y));
				num = offset - num;
				num *= num;
				num *= num3;
				num2 += num * m_Exponent[i];
			}
			return num2;
		}

		public double BrownianMotion(double x, double y)
		{
			double num = 0.0;
			long num2;
			for (num2 = 0L; num2 < m_IntOctaves; num2++)
			{
				num = m_Noise.Noise(x, y) * m_Exponent[num2];
				x *= m_Lacunarity;
				y *= m_Lacunarity;
			}
			double num3 = m_Octaves - (double)m_IntOctaves;
			return num + num3 * m_Noise.Noise(x, y) * m_Exponent[num2];
		}
	}
}
