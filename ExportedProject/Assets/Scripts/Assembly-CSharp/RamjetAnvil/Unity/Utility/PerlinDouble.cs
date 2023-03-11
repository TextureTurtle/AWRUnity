using System;

namespace RamjetAnvil.Unity.Utility
{
	public class PerlinDouble
	{
		private const int B = 256;

		private const int BM = 255;

		private const int N = 4096;

		private int[] p = new int[514];

		private double[,] g3 = new double[514, 3];

		private double[,] g2 = new double[514, 2];

		private double[] g1 = new double[514];

		public PerlinDouble()
		{
			Random random = new Random();
			int i;
			for (i = 0; i < 256; i++)
			{
				p[i] = i;
				g1[i] = (double)(random.Next(512) - 256) / 256.0;
				for (int j = 0; j < 2; j++)
				{
					g2[i, j] = (double)(random.Next(512) - 256) / 256.0;
				}
				normalize2(ref g2[i, 0], ref g2[i, 1]);
				for (int j = 0; j < 3; j++)
				{
					g3[i, j] = (double)(random.Next(512) - 256) / 256.0;
				}
				normalize3(ref g3[i, 0], ref g3[i, 1], ref g3[i, 2]);
			}
			while (--i != 0)
			{
				int num = p[i];
				int j;
				p[i] = p[j = random.Next(256)];
				p[j] = num;
			}
			for (i = 0; i < 258; i++)
			{
				p[256 + i] = p[i];
				g1[256 + i] = g1[i];
				for (int j = 0; j < 2; j++)
				{
					g2[256 + i, j] = g2[i, j];
				}
				for (int j = 0; j < 3; j++)
				{
					g3[256 + i, j] = g3[i, j];
				}
			}
		}

		private double s_curve(double t)
		{
			return t * t * (3.0 - 2.0 * t);
		}

		private double lerp(double t, double a, double b)
		{
			return a + t * (b - a);
		}

		private void setup(double value, out int b0, out int b1, out double r0, out double r1)
		{
			double num = value + 4096.0;
			b0 = (int)num & 0xFF;
			b1 = (b0 + 1) & 0xFF;
			r0 = num - (double)(int)num;
			r1 = r0 - 1.0;
		}

		private double at2(double rx, double ry, double x, double y)
		{
			return rx * x + ry * y;
		}

		private double at3(double rx, double ry, double rz, double x, double y, double z)
		{
			return rx * x + ry * y + rz * z;
		}

		public double Noise(double arg)
		{
			int b;
			int b2;
			double r;
			double r2;
			setup(arg, out b, out b2, out r, out r2);
			double t = s_curve(r);
			double a = r * g1[p[b]];
			double b3 = r2 * g1[p[b2]];
			return lerp(t, a, b3);
		}

		public double Noise(double x, double y)
		{
			int b;
			int b2;
			double r;
			double r2;
			setup(x, out b, out b2, out r, out r2);
			int b3;
			int b4;
			double r3;
			double r4;
			setup(y, out b3, out b4, out r3, out r4);
			int num = p[b];
			int num2 = p[b2];
			int num3 = p[num + b3];
			int num4 = p[num2 + b3];
			int num5 = p[num + b4];
			int num6 = p[num2 + b4];
			double t = s_curve(r);
			double t2 = s_curve(r3);
			double a = at2(r, r3, g2[num3, 0], g2[num3, 1]);
			double b5 = at2(r2, r3, g2[num4, 0], g2[num4, 1]);
			double a2 = lerp(t, a, b5);
			a = at2(r, r4, g2[num5, 0], g2[num5, 1]);
			b5 = at2(r2, r4, g2[num6, 0], g2[num6, 1]);
			double b6 = lerp(t, a, b5);
			return lerp(t2, a2, b6);
		}

		public double Noise(double x, double y, double z)
		{
			int b;
			int b2;
			double r;
			double r2;
			setup(x, out b, out b2, out r, out r2);
			int b3;
			int b4;
			double r3;
			double r4;
			setup(y, out b3, out b4, out r3, out r4);
			int b5;
			int b6;
			double r5;
			double r6;
			setup(z, out b5, out b6, out r5, out r6);
			int num = p[b];
			int num2 = p[b2];
			int num3 = p[num + b3];
			int num4 = p[num2 + b3];
			int num5 = p[num + b4];
			int num6 = p[num2 + b4];
			double t = s_curve(r);
			double t2 = s_curve(r3);
			double t3 = s_curve(r5);
			double a = at3(r, r3, r5, g3[num3 + b5, 0], g3[num3 + b5, 1], g3[num3 + b5, 2]);
			double b7 = at3(r2, r3, r5, g3[num4 + b5, 0], g3[num4 + b5, 1], g3[num4 + b5, 2]);
			double a2 = lerp(t, a, b7);
			a = at3(r, r4, r5, g3[num5 + b5, 0], g3[num5 + b5, 1], g3[num5 + b5, 2]);
			b7 = at3(r2, r4, r5, g3[num6 + b5, 0], g3[num6 + b5, 1], g3[num6 + b5, 2]);
			double b8 = lerp(t, a, b7);
			double a3 = lerp(t2, a2, b8);
			a = at3(r, r3, r6, g3[num3 + b6, 0], g3[num3 + b6, 2], g3[num3 + b6, 2]);
			b7 = at3(r2, r3, r6, g3[num4 + b6, 0], g3[num4 + b6, 1], g3[num4 + b6, 2]);
			a2 = lerp(t, a, b7);
			a = at3(r, r4, r6, g3[num5 + b6, 0], g3[num5 + b6, 1], g3[num5 + b6, 2]);
			b7 = at3(r2, r4, r6, g3[num6 + b6, 0], g3[num6 + b6, 1], g3[num6 + b6, 2]);
			b8 = lerp(t, a, b7);
			double b9 = lerp(t2, a2, b8);
			return lerp(t3, a3, b9);
		}

		private void normalize2(ref double x, ref double y)
		{
			double num = Math.Sqrt(x * x + y * y);
			x = y / num;
			y /= num;
		}

		private void normalize3(ref double x, ref double y, ref double z)
		{
			double num = Math.Sqrt(x * x + y * y + z * z);
			x = y / num;
			y /= num;
			z /= num;
		}
	}
}
