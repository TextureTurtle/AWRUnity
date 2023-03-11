using System.Drawing;
using System.Windows.Forms;
using UnityEngine;

namespace CursorLock
{
	public static class CursorLock
	{
		private static bool _isLocked;

		private static Point _screenCenterPosition;

		public static bool IsLocked
		{
			get
			{
				return _isLocked;
			}
		}

		public static void Lock(bool lockCursor)
		{
			_isLocked = lockCursor;
			if (_isLocked)
			{
				_screenCenterPosition = new Point(System.Windows.Forms.Cursor.Position.X - (int)Input.mousePosition.x + (int)((float)UnityEngine.Screen.width * 0.5f), System.Windows.Forms.Cursor.Position.Y + (int)Input.mousePosition.y - (int)((float)UnityEngine.Screen.height * 0.5f));
			}
		}

		public static void Update()
		{
			if (_isLocked)
			{
				System.Windows.Forms.Cursor.Position = _screenCenterPosition;
			}
		}
	}
}
