using System;

namespace Mono.WebBrowser
{
	public class CreateNewWindowEventArgs : EventArgs
	{
		private bool isModal;

		public bool IsModal
		{
			get
			{
				return isModal;
			}
		}

		public CreateNewWindowEventArgs(bool isModal)
		{
			this.isModal = isModal;
		}
	}
}
