using UnityEngine;

public static class GuiChangeMonitor
{
	public static bool HasChanged()
	{
		bool changed = GUI.changed;
		GUI.changed = false;
		return changed;
	}

	public static void Reset()
	{
		GUI.changed = false;
	}
}
