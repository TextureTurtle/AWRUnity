using UnityEngine;

public class LinuxInputTest : MonoBehaviour
{
	private float _mouseX;

	private void Update()
	{
		_mouseX = Input.GetAxisRaw("unity_mouse_0");
	}

	private void OnGUI()
	{
		GUILayout.BeginVertical(GUI.skin.box, GUILayout.Width(350f));
		GUILayout.Label(string.Format("MouseX: {0:00.00}", _mouseX));
		GUILayout.EndVertical();
	}
}
