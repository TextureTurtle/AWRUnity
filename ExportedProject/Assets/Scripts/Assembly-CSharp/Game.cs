using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : SingletonComponent<Game>
{
	public enum GameState
	{
		Ready = 0,
		Playing = 1,
		Paused = 2,
		GameOver = 3
	}

	[SerializeField]
	private WorldStreamer _streamer;

	[SerializeField]
	private Transform _balloonHolder;

	[SerializeField]
	private KillZone _killzone;

	[SerializeField]
	private GameObject[] _playerPrefabs;

	[SerializeField]
	private Transform[] _spawnPoints;

	[SerializeField]
	private bool _startInstantly = true;

	[SerializeField]
	private bool _forceRegularGarbageCollection = true;

	private int _numPlayers;

	private List<Player> _players;

	private GameState _state;

	public Player[] Players
	{
		get
		{
			return _players.ToArray();
		}
	}

	public GameState State
	{
		get
		{
			return _state;
		}
	}

	public event EscapeCallback OnEscapePressed;

	public event Action<Player> OnPlayerSpawned;

	public event Action<Player> OnPlayerDespawned;

	public event Action OnGameStarted;

	public event Action OnGameOver;

	private void Awake()
	{
		GraphicsSettings.Initialize();
		_players = new List<Player>();
	}

	private void Start()
	{
		_killzone.OnPlayerKilled += OnPlayerKilled;
		_state = GameState.Ready;
		if (_startInstantly)
		{
			StartNewGame(1);
		}
	}

	public void StartNewGame(int numPlayers)
	{
		_numPlayers = numPlayers;
		_streamer.OnReady += OnLoadingDone;
		_streamer.LoadInitialState();
		LockCursor(true);
	}

	public void PauseGame()
	{
		Time.timeScale = 0f;
		AudioListener.volume = 0f;
		_state = GameState.Paused;
		LockCursor(false);
		foreach (Player player in _players)
		{
			player.Input.enabled = false;
		}
	}

	public void ResumeGame()
	{
		Time.timeScale = 1f;
		AudioListener.volume = 1f;
		_state = GameState.Playing;
		LockCursor(true);
		foreach (Player player in _players)
		{
			player.Input.enabled = true;
		}
	}

	public void EndGame()
	{
		_state = GameState.GameOver;
		LockCursor(false);
	}

	private void LockCursor(bool lockCursor)
	{
		Screen.lockCursor = lockCursor;
	}

	public void QuitGame()
	{
		Application.Quit();
	}

	private void SpawnPlayers(int numPlayers)
	{
		for (int i = 0; i < numPlayers; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(_playerPrefabs[i], _spawnPoints[i].position, _spawnPoints[i].rotation) as GameObject;
			Player component = gameObject.GetComponent<Player>();
			component.Initialize(i);
			_players.Add(component);
			if (this.OnPlayerSpawned != null)
			{
				this.OnPlayerSpawned(component);
			}
		}
	}

	private void OnLoadingDone()
	{
		_streamer.StartStreaming();
		GameObject gameObject = GameObject.FindGameObjectWithTag("Spawnpoint");
		Debug.Log((!gameObject) ? "Couldn't find spawnpoint" : "Spawnpoint found");
		_balloonHolder.position = gameObject.transform.position;
		for (int num = _balloonHolder.childCount - 1; num >= 0; num--)
		{
			Transform child = _balloonHolder.GetChild(num);
			child.parent = null;
		}
		_balloonHolder.gameObject.SetActive(true);
		UnityEngine.Object.Destroy(_balloonHolder.gameObject);
		SpawnPlayers(_numPlayers);
		if (this.OnGameStarted != null)
		{
			this.OnGameStarted();
		}
		_state = GameState.Playing;
	}

	private void OnPlayerKilled(Player player)
	{
		Debug.Log("Player died: " + player.PlayerId);
		if (this.OnPlayerDespawned != null)
		{
			this.OnPlayerDespawned(player);
		}
		UnityEngine.Object.Destroy(player.gameObject, 0.5f);
		if (this.OnGameOver != null)
		{
			this.OnGameOver();
		}
		StartCoroutine(Wait(1f, delegate
		{
			_state = GameState.GameOver;
		}));
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (this.OnEscapePressed != null)
			{
				EscapeEvent approval = new EscapeEvent();
				this.OnEscapePressed(ref approval);
				if (approval.IsHandled)
				{
					return;
				}
			}
			switch (_state)
			{
			case GameState.Playing:
				PauseGame();
				break;
			case GameState.Paused:
				ResumeGame();
				break;
			}
		}
		if (_forceRegularGarbageCollection && Time.frameCount % 30 == 0)
		{
			GC.Collect();
		}
	}

	private IEnumerator Wait(float fadeTime, Action onWaitDone)
	{
		float lastTime = Time.realtimeSinceStartup;
		float timer = 0f;
		while (timer < fadeTime)
		{
			timer += Time.realtimeSinceStartup - lastTime;
			lastTime = Time.realtimeSinceStartup;
			yield return new WaitForEndOfFrame();
		}
		if (onWaitDone != null)
		{
			onWaitDone();
		}
	}
}
