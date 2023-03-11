public static class Mathd
{
	public static double Clamp(double value, double min, double max)
	{
		if (value < min)
		{
			value = min;
		}
		else if (value > max)
		{
			value = max;
		}
		return value;
	}

	public static double Clamp01(double value)
	{
		if (value < 0.0)
		{
			return 0.0;
		}
		if (value > 1.0)
		{
			return 1.0;
		}
		return value;
	}

	public static double Min(double a, double b)
	{
		if (a < b)
		{
			return a;
		}
		return b;
	}

	public static double Max(double a, double b)
	{
		if (a > b)
		{
			return a;
		}
		return b;
	}
}
