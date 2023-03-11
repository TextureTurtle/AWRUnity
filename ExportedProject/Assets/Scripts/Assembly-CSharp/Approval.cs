public class Approval
{
	private Player _player;

	private bool _approved;

	public Player Player
	{
		get
		{
			return _player;
		}
	}

	public bool Approved
	{
		get
		{
			return _approved;
		}
	}

	public Approval(Player player)
	{
		_player = player;
		_approved = true;
	}

	public void Approve(bool approve)
	{
		_approved &= approve;
	}
}
