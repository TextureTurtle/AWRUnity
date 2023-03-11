using System;
using UnityEngine;

public static class GuiExtensions
{
	public static GUIStyle toggled_style;

	public static TEnum EnumToolbar<TEnum>(TEnum selected)
	{
		Type typeFromHandle = typeof(TEnum);
		if (!typeFromHandle.IsEnum)
		{
			throw new ArgumentException("Argument type must be enum");
		}
		string[] names = Enum.GetNames(typeFromHandle);
		Array values = Enum.GetValues(typeFromHandle);
		for (int i = 0; i < names.Length; i++)
		{
			string text = names[i];
			text = text.Replace("_", " ");
			names[i] = text;
		}
		int j;
		for (j = 0; j < values.Length && !(selected.ToString() == values.GetValue(j).ToString()); j++)
		{
		}
		j = GUILayout.Toolbar(j, names);
		return (TEnum)values.GetValue(j);
	}

	public static bool ToggleButton(bool state, string label)
	{
		BuildStyle();
		bool flag = false;
		if ((!state) ? GUILayout.Button(label) : GUILayout.Button(label, toggled_style))
		{
			return !state;
		}
		return state;
	}

	private static void BuildStyle()
	{
		if (toggled_style == null)
		{
			toggled_style = new GUIStyle(GUI.skin.button);
			toggled_style.normal.background = toggled_style.onActive.background;
			toggled_style.normal.textColor = toggled_style.onActive.textColor;
		}
	}
}
