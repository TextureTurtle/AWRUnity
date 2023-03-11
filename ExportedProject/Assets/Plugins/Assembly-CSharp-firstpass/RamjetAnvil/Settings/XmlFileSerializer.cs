using System;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace RamjetAnvil.Settings
{
	public class XmlFileSerializer : ISettingsSerializer
	{
		public void Save(object settings, string path)
		{
			Type type = settings.GetType();
			string directoryName = Path.GetDirectoryName(path);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			XmlSerializer xmlSerializer = new XmlSerializer(type);
			TextWriter textWriter = new StreamWriter(path);
			xmlSerializer.Serialize(textWriter, settings);
			textWriter.Close();
		}

		public object Load(Type type, string path)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(type);
			object result = null;
			try
			{
				TextReader textReader = new StreamReader(path);
				result = xmlSerializer.Deserialize(textReader);
				textReader.Close();
				return result;
			}
			catch (Exception ex)
			{
				Debug.LogWarning(type.ToString() + " settings could not be loaded.\n" + ex.Message);
				return result;
			}
		}
	}
}
