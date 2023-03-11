#define RAMJET_DEBUG_WARNING
#define RAMJET_DEBUG_ERROR
using System.IO;
using System.Xml;
using RamjetAnvil.Unity.Debug;

namespace RamjetAnvil.Unity.Impero
{
	public class ActionMapXmlSerializer
	{
		public static ActionMap CreateFromTemplate(string filePath, int playerId)
		{
			ActionMap actionMap = new ActionMap(playerId);
			if (!File.Exists(filePath))
			{
				DebugUtils.LogError("Template file does not exist");
				return actionMap;
			}
			try
			{
				TextReader textReader = new StreamReader(filePath);
				using (XmlReader xmlReader = new XmlTextReader(textReader))
				{
					ReadFromTemplate(xmlReader, actionMap);
					xmlReader.Close();
				}
				textReader.Close();
				return actionMap;
			}
			catch (IOException ex)
			{
				DebugUtils.LogError(ex.Message);
				return actionMap;
			}
		}

		public static bool Deserialize(string filePath, ActionMap actionMap)
		{
			if (!File.Exists(filePath))
			{
				DebugUtils.LogWarning("Config file does not exist");
				return false;
			}
			try
			{
				TextReader textReader = new StreamReader(filePath);
				using (XmlReader xmlReader = new XmlTextReader(textReader))
				{
					Read(xmlReader, actionMap);
					xmlReader.Close();
				}
				textReader.Close();
				textReader.Dispose();
				return true;
			}
			catch (IOException ex)
			{
				DebugUtils.LogError(ex.Message);
				return false;
			}
		}

		public static bool Serialize(string filePath, ActionMap actionMap)
		{
			try
			{
				TextWriter textWriter = new StreamWriter(filePath, false);
				XmlTextWriter xmlTextWriter = new XmlTextWriter(textWriter);
				xmlTextWriter.Formatting = Formatting.Indented;
				xmlTextWriter.WriteStartDocument();
				Write(xmlTextWriter, actionMap);
				xmlTextWriter.WriteEndDocument();
				xmlTextWriter.Close();
				textWriter.Close();
				textWriter.Dispose();
				return true;
			}
			catch (IOException ex)
			{
				DebugUtils.LogError(ex.Message);
				return false;
			}
		}

		private static void ReadFromTemplate(XmlReader reader, ActionMap map)
		{
			while (!reader.EOF && reader.ReadToFollowing("Action"))
			{
				Action action = ReadActionFromTemplate(reader);
				if (action == null)
				{
					DebugUtils.LogError("Couldn't create action");
					break;
				}
				switch (action.ActionType)
				{
				case InputType.Button:
					map.ButtonMap.Add(action.Identifier, action as ButtonAction);
					break;
				case InputType.Axis:
					map.AxisMap.Add(action.Identifier, action as AxisAction);
					break;
				}
			}
		}

		private static Action ReadActionFromTemplate(XmlReader reader)
		{
			InputType inputType = EnumHelper.FromString<InputType>(reader.GetAttribute("Type"));
			string attribute = reader.GetAttribute("ID");
			string attribute2 = reader.GetAttribute("Name");
			switch (inputType)
			{
			case InputType.Button:
				return new ButtonAction(attribute, attribute2);
			case InputType.Axis:
			{
				float range = float.Parse(reader.GetAttribute("Range"));
				SubAxisAction positiveAction = ReadSubAxisFromTemplate(reader);
				SubAxisAction negativeAction = ReadSubAxisFromTemplate(reader);
				return new AxisAction(attribute, attribute2, range, positiveAction, negativeAction);
			}
			default:
				return null;
			}
		}

		private static SubAxisAction ReadSubAxisFromTemplate(XmlReader reader)
		{
			if (!reader.ReadToFollowing("SubAction"))
			{
				return null;
			}
			string attribute = reader.GetAttribute("ID");
			string attribute2 = reader.GetAttribute("Name");
			Polarity polarity = EnumHelper.FromString<Polarity>(reader.GetAttribute("Polarity"));
			return new SubAxisAction(attribute, attribute2, polarity);
		}

		private static void Read(XmlReader reader, ActionMap map)
		{
			while (!reader.EOF && reader.ReadToFollowing("Action"))
			{
				ReadAction(reader, map);
			}
		}

		private static void ReadAction(XmlReader reader, ActionMap map)
		{
			string attribute = reader.GetAttribute("ID");
			switch (EnumHelper.FromString<InputType>(reader.GetAttribute("Type")))
			{
			case InputType.Button:
			{
				if (!map.ButtonMap.ContainsKey(attribute))
				{
					DebugUtils.LogWarning("Ignoring invalid ButtonAction '{0}", attribute);
					break;
				}
				IButtonAdapter buttonAdapter = ReadAdapter(reader, map) as IButtonAdapter;
				if (buttonAdapter != null)
				{
					InputManager.Instance.AddAdapter(buttonAdapter);
					map.ButtonMap[attribute].Adapter = buttonAdapter;
				}
				break;
			}
			case InputType.Axis:
				if (!map.AxisMap.ContainsKey(attribute))
				{
					DebugUtils.LogWarning("Ignoring invalid AxisAction '{0}", attribute);
				}
				else
				{
					ReadSubAxis(reader, map, map.AxisMap[attribute].Positive);
					ReadSubAxis(reader, map, map.AxisMap[attribute].Negative);
				}
				break;
			}
		}

		private static void ReadSubAxis(XmlReader reader, ActionMap map, SubAxisAction action)
		{
			if (!reader.ReadToFollowing("SubAction"))
			{
				return;
			}
			string attribute = reader.GetAttribute("ID");
			Polarity polarity = EnumHelper.FromString<Polarity>(reader.GetAttribute("Polarity"));
			if (attribute != action.Identifier || polarity != action.Polarity)
			{
				DebugUtils.LogWarning("Ignoring invalid subaction '{0}' {1}", attribute, polarity);
				return;
			}
			IAxisAdapter axisAdapter = ReadAdapter(reader, map) as IAxisAdapter;
			if (axisAdapter != null)
			{
				InputManager.Instance.AddAdapter(axisAdapter);
				action.Adapter = axisAdapter;
			}
			else
			{
				DebugUtils.LogError("Failed to get adapter for subaxis '{0}'", attribute);
			}
		}

		private static IAdapter ReadAdapter(XmlReader reader, ActionMap map)
		{
			if (!TryReadToDescendant(reader, "Adapter"))
			{
				return null;
			}
			switch (EnumHelper.FromString<InputType>(reader.GetAttribute("Type")))
			{
			case InputType.Button:
			{
				IInput input = ReadInput(reader, map);
				return AdapterFactory.CreateButtonAdapter(input);
			}
			case InputType.Axis:
			{
				float sensitivity = float.Parse(reader.GetAttribute("Sensitivity"));
				IInput input = ReadInput(reader, map);
				IAxisAdapter axisAdapter = AdapterFactory.CreateAxisAdapter(input);
				if (axisAdapter != null)
				{
					axisAdapter.Sensitivity = sensitivity;
				}
				return axisAdapter;
			}
			default:
				return null;
			}
		}

		private static IInput ReadInput(XmlReader reader, ActionMap map)
		{
			if (!TryReadToDescendant(reader, "Input"))
			{
				return null;
			}
			InputType inputType = EnumHelper.FromString<InputType>(reader.GetAttribute("Type"));
			reader.ReadToDescendant("Peripheral");
			string text = reader.ReadElementContentAsString();
			IPeripheral peripheral = InputManager.Instance.FindUnusedPeripheralById(text, map.PlayerId);
			if (peripheral == null)
			{
				DebugUtils.LogError("Couldn't find available peripheral with ID " + text);
				return null;
			}
			reader.ReadToFollowing("Index");
			int index = reader.ReadElementContentAsInt();
			switch (inputType)
			{
			case InputType.Button:
				return new ButtonInput(peripheral, index);
			case InputType.Axis:
			{
				reader.ReadToFollowing("Polarity");
				Polarity polarity = EnumHelper.FromString<Polarity>(reader.ReadElementContentAsString());
				return new AxisInput(peripheral, index, polarity);
			}
			default:
				return null;
			}
		}

		private static bool TryReadToDescendant(XmlReader reader, string elementName)
		{
			if (!reader.ReadToDescendant(elementName))
			{
				DebugUtils.LogError("Couldn't find element '{0}'", elementName);
				return false;
			}
			return true;
		}

		private static void Write(XmlWriter writer, ActionMap map)
		{
			writer.WriteStartElement("ActionMap");
			foreach (ButtonAction buttonAction in map.ButtonActions)
			{
				WriterButtonAction(writer, buttonAction);
			}
			foreach (AxisAction axisAction in map.AxisActions)
			{
				WriteAxisAction(writer, axisAction);
			}
			writer.WriteEndElement();
		}

		private static void WriterButtonAction(XmlWriter writer, ButtonAction action)
		{
			writer.WriteStartElement("Action");
			writer.WriteAttributeString("ID", action.Identifier);
			writer.WriteAttributeString("Type", action.ActionType.ToString());
			WriteAdapter(writer, action.Adapter);
			writer.WriteEndElement();
		}

		private static void WriteAxisAction(XmlWriter writer, AxisAction action)
		{
			writer.WriteStartElement("Action");
			writer.WriteAttributeString("ID", action.Identifier);
			writer.WriteAttributeString("Type", action.ActionType.ToString());
			WriteSubAxisAction(writer, action.Positive);
			WriteSubAxisAction(writer, action.Negative);
			writer.WriteEndElement();
		}

		private static void WriteSubAxisAction(XmlWriter writer, SubAxisAction action)
		{
			writer.WriteStartElement("SubAction");
			writer.WriteAttributeString("ID", action.Identifier);
			writer.WriteAttributeString("Polarity", action.Polarity.ToString());
			WriteAdapter(writer, action.Adapter);
			writer.WriteEndElement();
		}

		private static void WriteAdapter(XmlWriter writer, IAdapter adapter)
		{
			writer.WriteStartElement("Adapter");
			writer.WriteAttributeString("Type", adapter.InputType.ToString());
			switch (adapter.InputType)
			{
			case InputType.Axis:
			{
				IAxisAdapter axisAdapter = adapter as IAxisAdapter;
				writer.WriteAttributeString("Sensitivity", axisAdapter.Sensitivity.ToString());
				break;
			}
			}
			WriteInput(writer, adapter.Input);
			writer.WriteEndElement();
		}

		private static void WriteInput(XmlWriter writer, IInput input)
		{
			writer.WriteStartElement("Input");
			writer.WriteAttributeString("Type", input.InputType.ToString());
			writer.WriteElementString("Peripheral", input.Peripheral.Identifier);
			switch (input.InputType)
			{
			case InputType.Button:
				writer.WriteElementString("Index", input.Index.ToString());
				break;
			case InputType.Axis:
			{
				writer.WriteElementString("Index", input.Index.ToString());
				IAxis axis = input as IAxis;
				writer.WriteElementString("Polarity", axis.Polarity.ToString());
				break;
			}
			}
			writer.WriteEndElement();
		}
	}
}
