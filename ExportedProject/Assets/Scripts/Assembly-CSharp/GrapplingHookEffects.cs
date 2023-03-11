using UnityEngine;

public class GrapplingHookEffects : MonoBehaviour
{
	[SerializeField]
	private GrapplingHookLauncher _launcher;

	[SerializeField]
	private GrapplingHook _hook;

	[SerializeField]
	private VirtualAudioSource _reelSource;

	[SerializeField]
	private VirtualAudioSource _controlSource;

	[SerializeField]
	private AudioClip _controlClip;

	[SerializeField]
	private AudioClip _shootClip;

	[SerializeField]
	private AudioClip _reelClip;

	[SerializeField]
	private AudioClip _doneReelingClip;

	[SerializeField]
	private AudioClip _latchClip;

	[SerializeField]
	private AudioClip _releaseClip;

	[SerializeField]
	private Transform _laserPointer;

	[SerializeField]
	private LineRenderer _lineRenderer;

	private void Start()
	{
		_launcher.OnShoot += OnShoot;
		_launcher.OnStartReel += OnStartReel;
		_launcher.OnStopReel += OnStopReel;
		_launcher.OnDoneReeling += OnDoneReeling;
		_hook.OnLatched += OnLatched;
		_hook.OnReleased += OnReleased;
		Controllable component = _launcher.GetComponent<Controllable>();
		component.OnControlTaken += OnControlTaken;
		component.OnControlReleased += OnControlReleased;
		_controlSource.Clip = _controlClip;
		_controlSource.Loop = true;
		_lineRenderer.SetVertexCount(2);
		RenderLaser();
	}

	private void OnControlTaken(Player player)
	{
		_lineRenderer.enabled = true;
		_controlSource.Play(player.Listener);
	}

	private void OnControlReleased(Player player)
	{
		_lineRenderer.enabled = false;
		_controlSource.Stop(player.Listener);
	}

	private void OnShoot()
	{
		_reelSource.PlayOneShot(_shootClip);
	}

	private void OnStartReel()
	{
		_reelSource.Clip = _reelClip;
		_reelSource.Loop = true;
		_reelSource.Play();
	}

	private void OnStopReel()
	{
		_reelSource.Stop();
	}

	private void OnDoneReeling()
	{
		_reelSource.Stop();
		_reelSource.PlayOneShot(_doneReelingClip);
	}

	private void OnLatched(GameObject latchedObject)
	{
		_reelSource.PlayOneShot(_latchClip);
	}

	private void OnReleased()
	{
		_reelSource.Stop();
		_reelSource.PlayOneShot(_releaseClip);
	}

	private void Update()
	{
		RenderLaser();
	}

	private void RenderLaser()
	{
		_lineRenderer.SetPosition(0, _laserPointer.transform.position);
		_lineRenderer.SetPosition(1, _launcher.TurretAimPoint);
	}
}
