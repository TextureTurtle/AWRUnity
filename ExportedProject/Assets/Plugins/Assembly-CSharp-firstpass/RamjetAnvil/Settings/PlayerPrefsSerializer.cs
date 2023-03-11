using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

namespace RamjetAnvil.Settings
{
	public class PlayerPrefsSerializer : ISettingsSerializer
	{
		public void Save(object settings, string prefsKey)
		{
			Type type = settings.GetType();
			XmlSerializer xmlSerializer = new XmlSerializer(type);
			StringBuilder stringBuilder = new StringBuilder();
			XmlWriter xmlWriter = XmlWriter.Create(stringBuilder);
			xmlSerializer.Serialize(xmlWriter, settings);
			xmlWriter.Close();
			PlayerPrefs.SetString(prefsKey, stringBuilder.ToString());
		}

		public object Load(Type type, string prefsKey)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(type);
			object result = null;
			try
			{
				string @string = PlayerPrefs.GetString(prefsKey);
				StringReader textReader = new StringReader(@string);
				result = xmlSerializer.Deserialize(textReader);
				return result;
			}
			catch (Exception ex)
			{
				Debug.LogWarning(type.ToString() + " settings could not be loaded.\n" + ex.ToString());
				return result;
			}
		}
	}
}
