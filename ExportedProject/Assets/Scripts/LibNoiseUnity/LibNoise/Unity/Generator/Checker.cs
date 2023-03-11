using System;

namespace LibNoise.Unity.Generator
{
	public class Checker : ModuleBase
	{
		public Checker()
			: base(0)
		{
		}

		public override double GetValue(double x, double y, double z)
		{
			int num = (int)Math.Floor(Utils.MakeInt32Range(x));
			int num2 = (int)Math.Floor(Utils.MakeInt32Range(y));
			int num3 = (int)Math.Floor(Utils.MakeInt32Range(z));
			if (((num & 1) ^ (num2 & 1) ^ (num3 & 1)) == 0)
			{
				return 1.0;
			}
			return -1.0;
		}
	}
}
