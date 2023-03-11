using UnityEngine;

public class PlayerCursorGui : MonoBehaviour
{
	[SerializeField]
	private Player _player;

	[SerializeField]
	private Camera _camera;

	[SerializeField]
	private Texture2D _crosshairTexture;

	private PlayerCursor _cursor;

	private ItemDescriptor _focusedObjectDescriptor;

	private Rect _crosshairRect;

	private Rect _itemLabelRect;

	private void Start()
	{
		_cursor = _player.Cursor;
		_cursor.OnFocusChanged += OnFocusChange;
		_crosshairRect = default(Rect);
		_itemLabelRect = default(Rect);
	}

	private void OnGUI()
	{
		Rect pixelRect = _camera.pixelRect;
		_crosshairRect.Set(pixelRect.width * 0.5f - 8f, (float)Screen.height - pixelRect.center.y - 8f, 16f, 16f);
		GUI.DrawTexture(_crosshairRect, _crosshairTexture);
		string text = ((!_focusedObjectDescriptor) ? string.Empty : _focusedObjectDescriptor.ItemName);
		_itemLabelRect.Set(pixelRect.width * 0.5f + 16f + 16f, (float)Screen.height - pixelRect.center.y - 25f + 12.5f, 200f, 50f);
		GUI.Label(_itemLabelRect, text);
	}

	private void OnFocusChange(GameObject go)
	{
		if (go != null)
		{
			_focusedObjectDescriptor = go.GetComponent<ItemDescriptor>();
		}
		else
		{
			_focusedObjectDescriptor = null;
		}
	}
}
