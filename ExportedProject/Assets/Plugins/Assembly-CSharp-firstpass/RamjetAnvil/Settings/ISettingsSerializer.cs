using System;

namespace RamjetAnvil.Settings
{
	public interface ISettingsSerializer
	{
		void Save(object settings, string path);

		object Load(Type type, string path);
	}
}
