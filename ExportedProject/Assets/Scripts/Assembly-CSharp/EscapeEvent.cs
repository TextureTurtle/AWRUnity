public class EscapeEvent
{
	private bool _isHandled;

	public bool IsHandled
	{
		get
		{
			return _isHandled;
		}
	}

	public EscapeEvent()
	{
		_isHandled = false;
	}

	public void Handle()
	{
		_isHandled = true;
	}
}
