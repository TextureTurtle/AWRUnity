using System;
using System.Xml.Serialization;
using UnityEngine;

namespace LibNoise.Unity
{
	public abstract class ModuleBase : IDisposable
	{
		protected ModuleBase[] m_modules;

		[NonSerialized]
		[XmlIgnore]
		private bool m_disposed;

		public virtual ModuleBase this[int index]
		{
			get
			{
				if (index < 0 || index >= m_modules.Length)
				{
					throw new ArgumentOutOfRangeException("Index out of valid module range");
				}
				if (m_modules[index] == null)
				{
					throw new ArgumentNullException("Desired element is null");
				}
				return m_modules[index];
			}
			set
			{
				if (index < 0 || index >= m_modules.Length)
				{
					throw new ArgumentOutOfRangeException("Index out of valid module range");
				}
				if (value == null)
				{
					throw new ArgumentNullException("Value should not be null");
				}
				m_modules[index] = value;
			}
		}

		public int SourceModuleCount
		{
			get
			{
				if (m_modules != null)
				{
					return m_modules.Length;
				}
				return 0;
			}
		}

		public bool IsDisposed
		{
			get
			{
				return m_disposed;
			}
		}

		protected ModuleBase(int count)
		{
			if (count > 0)
			{
				m_modules = new ModuleBase[count];
			}
		}

		public abstract double GetValue(double x, double y, double z);

		public double GetValue(Vector3 coordinate)
		{
			return GetValue(coordinate.x, coordinate.y, coordinate.z);
		}

		public double GetValue(ref Vector3 coordinate)
		{
			return GetValue(coordinate.x, coordinate.y, coordinate.z);
		}

		public void Dispose()
		{
			if (!m_disposed)
			{
				m_disposed = Disposing();
			}
			GC.SuppressFinalize(this);
		}

		protected virtual bool Disposing()
		{
			if (m_modules != null)
			{
				for (int i = 0; i < m_modules.Length; i++)
				{
					m_modules[i].Dispose();
					m_modules[i] = null;
				}
				m_modules = null;
			}
			return true;
		}
	}
}
