namespace XInputDotNetPure
{
	public struct GamePadThumbSticks
	{
		public struct StickValue
		{
			private float x;

			private float y;

			public float X
			{
				get
				{
					return x;
				}
			}

			public float Y
			{
				get
				{
					return y;
				}
			}

			internal StickValue(float x, float y)
			{
				this.x = x;
				this.y = y;
			}
		}

		private StickValue left;

		private StickValue right;

		public StickValue Left
		{
			get
			{
				return left;
			}
		}

		public StickValue Right
		{
			get
			{
				return right;
			}
		}

		internal GamePadThumbSticks(StickValue left, StickValue right)
		{
			this.left = left;
			this.right = right;
		}
	}
}
