namespace LibNoise.Unity.Operator
{
	public class Select : ModuleBase
	{
		private double m_fallOff;

		private double m_raw;

		private double m_min = -1.0;

		private double m_max = 1.0;

		public ModuleBase Controller
		{
			get
			{
				return m_modules[2];
			}
			set
			{
				m_modules[2] = value;
			}
		}

		public double FallOff
		{
			get
			{
				return m_fallOff;
			}
			set
			{
				double num = m_max - m_min;
				m_raw = value;
				m_fallOff = ((value > num / 2.0) ? (num / 2.0) : value);
			}
		}

		public double Maximum
		{
			get
			{
				return m_max;
			}
			set
			{
				m_max = value;
				FallOff = m_raw;
			}
		}

		public double Minimum
		{
			get
			{
				return m_min;
			}
			set
			{
				m_min = value;
				FallOff = m_raw;
			}
		}

		public Select()
			: base(3)
		{
		}

		public Select(ModuleBase inputA, ModuleBase inputB, ModuleBase controller)
			: base(3)
		{
			m_modules[0] = inputA;
			m_modules[1] = inputB;
			m_modules[2] = controller;
		}

		public Select(double min, double max, double fallOff, ModuleBase inputA, ModuleBase inputB)
			: this(inputA, inputB, null)
		{
			m_min = min;
			m_max = max;
			FallOff = fallOff;
		}

		public void SetBounds(double min, double max)
		{
			m_min = min;
			m_max = max;
			FallOff = m_fallOff;
		}

		public override double GetValue(double x, double y, double z)
		{
			double value = m_modules[2].GetValue(x, y, z);
			if (m_fallOff > 0.0)
			{
				if (value < m_min - m_fallOff)
				{
					return m_modules[0].GetValue(x, y, z);
				}
				if (value < m_min + m_fallOff)
				{
					double num = m_min - m_fallOff;
					double num2 = m_min + m_fallOff;
					double position = Utils.MapCubicSCurve((value - num) / (num2 - num));
					return Utils.InterpolateLinear(m_modules[0].GetValue(x, y, z), m_modules[1].GetValue(x, y, z), position);
				}
				if (value < m_max - m_fallOff)
				{
					return m_modules[1].GetValue(x, y, z);
				}
				if (value < m_max + m_fallOff)
				{
					double num3 = m_max - m_fallOff;
					double num4 = m_max + m_fallOff;
					double position = Utils.MapCubicSCurve((value - num3) / (num4 - num3));
					return Utils.InterpolateLinear(m_modules[1].GetValue(x, y, z), m_modules[0].GetValue(x, y, z), position);
				}
				return m_modules[0].GetValue(x, y, z);
			}
			if (value < m_min || value > m_max)
			{
				return m_modules[0].GetValue(x, y, z);
			}
			return m_modules[1].GetValue(x, y, z);
		}
	}
}
