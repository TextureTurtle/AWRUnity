using System;

namespace Mono.WebBrowser
{
	public class ProgressChangedEventArgs : EventArgs
	{
		private int progress;

		private int maxProgress;

		public int Progress
		{
			get
			{
				return progress;
			}
		}

		public int MaxProgress
		{
			get
			{
				return maxProgress;
			}
		}

		public ProgressChangedEventArgs(int progress, int maxProgress)
		{
			this.progress = progress;
			this.maxProgress = maxProgress;
		}
	}
}
