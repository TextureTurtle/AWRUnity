using System.Globalization;
using RamjetAnvil.Unity.Impero;
using UnityEngine;

public class MenuGui : MonoBehaviour
{
	private enum MenuTab
	{
		Main = 0,
		Controls = 1,
		Graphics = 2
	}

	private const float MenuWidth = 300f;

	[SerializeField]
	private Game _game;

	[SerializeField]
	private ScreenFader _screenFader;

	[SerializeField]
	private Texture2D _letterTexture;

	private MenuTab _currentTab;

	private Player _selectedPlayer;

	private float _menuWidth = 500f;

	private Vector2 _inputScrollPosition;

	private Vector2 _resolutionScrollPosition;

	private Vector2 _qualityScrollPosition;

	private int _numPlayers = 1;

	private static readonly Color HighlightColor = new Color(0.66f, 0.66f, 1f, 1f);

	private static readonly NumberFormatInfo NumberFormat = new NumberFormatInfo();

	private void Start()
	{
		_game.OnEscapePressed += OnEscapePressed;
		_game.OnGameStarted += OnGameStarted;
		_game.OnGameOver += OnGameOver;
	}

	private void OnEscapePressed(ref EscapeEvent escapeEvent)
	{
		if (InputManager.Instance.IsRemapping)
		{
			escapeEvent.Handle();
			InputManager.Instance.CancelRemap();
		}
	}

	private void OnGameStarted()
	{
		_screenFader.FadeIn(null);
	}

	private void OnGameOver()
	{
		_screenFader.FadeOut(null);
	}

	private void OnGUI()
	{
		_screenFader.DrawFade();
		switch (_game.State)
		{
		case Game.GameState.Ready:
			DrawStartMenu();
			break;
		case Game.GameState.Playing:
			break;
		case Game.GameState.Paused:
			DrawPauseMenu();
			break;
		case Game.GameState.GameOver:
			DrawGameOverMenu();
			break;
		}
	}

	private void DrawStartMenu()
	{
		float num = (float)Screen.height / (float)_letterTexture.height;
		Rect position = new Rect((float)Screen.width * 0.5f - (float)_letterTexture.width * num * 0.5f, 0f, (float)_letterTexture.width * num, (float)_letterTexture.height * num);
		GUI.DrawTexture(position, _letterTexture);
		Rect screenRect = new Rect((float)Screen.width - 300f - 16f, 16f, 300f, (float)Screen.height - 32f);
		GUILayout.BeginArea(screenRect);
		GUILayout.BeginVertical();
		GUILayout.FlexibleSpace();
		GUILayout.EndVertical();
		GUILayout.BeginVertical(GUI.skin.box);
		ShowHorizontallyCenteredLabel("Who will accept?");
		_numPlayers = GUILayout.Toolbar(_numPlayers - 1, new string[2] { "Only Jebe", "Jebe and Frits" }) + 1;
		GUILayout.Space(32f);
		if (GUILayout.Button("Onwards and upwards!", GUILayout.MinHeight(50f)))
		{
			_game.StartNewGame(_numPlayers);
		}
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}

	private void DrawPauseMenu()
	{
		float num = (float)Screen.height * 0.66f;
		Rect screenRect = new Rect((float)Screen.width * 0.5f - _menuWidth * 0.5f, (float)Screen.height * 0.5f - num * 0.5f, _menuWidth, num);
		GUILayout.BeginArea(screenRect);
		GUILayout.BeginVertical(GUI.skin.box, GUILayout.MinHeight(num));
		GUILayout.Space(16f);
		_currentTab = GuiExtensions.EnumToolbar(_currentTab);
		GUILayout.Space(16f);
		switch (_currentTab)
		{
		case MenuTab.Main:
			DrawMainMenu();
			break;
		case MenuTab.Controls:
			DrawInputConfigMenu();
			break;
		case MenuTab.Graphics:
			DrawGraphicsConfigMenu();
			break;
		}
		GUILayout.EndVertical();
		GUILayout.EndArea();
		Rect screenRect2 = new Rect((float)Screen.width - 300f - 16f, 16f, 300f, (float)Screen.height - 32f);
		GUILayout.BeginArea(screenRect2);
		GUILayout.BeginVertical();
		if (GUILayout.Button("I will beat Frederik!", GUILayout.MinHeight(50f)))
		{
			_game.ResumeGame();
		}
		GUILayout.FlexibleSpace();
		if (GUILayout.Button("You win, Frederik", GUILayout.MinHeight(50f)))
		{
			_screenFader.FadeOut(delegate
			{
				_game.EndGame();
			});
		}
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}

	private void DrawGameOverMenu()
	{
		Rect screenRect = new Rect((float)Screen.width - 300f - 16f, 16f, 300f, (float)Screen.height - 32f);
		GUILayout.BeginArea(screenRect);
		GUILayout.BeginVertical();
		GUILayout.FlexibleSpace();
		if (GUILayout.Button("I hate you, Frederik!", GUILayout.MinHeight(50f)))
		{
			_game.QuitGame();
		}
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}

	private void DrawMainMenu()
	{
		ShowHorizontallyCenteredLabel("Game Paused");
	}

	private void DrawInputConfigMenu()
	{
		if (!_selectedPlayer)
		{
			_selectedPlayer = _game.Players[0];
		}
		ShowPlayerSelector();
		if (InputManager.Instance.IsRemapping)
		{
			ShowControlRemappingMessage();
		}
		else
		{
			ShowActionMap(_selectedPlayer.Input.ActionMap);
		}
		ShowControlSaveButtons();
	}

	private void DrawGraphicsConfigMenu()
	{
		GUILayout.BeginHorizontal();
		GUILayout.BeginVertical();
		ShowHorizontallyCenteredLabel("Resolution");
		GraphicsSettings.IsFullscreen = GUILayout.Toggle(GraphicsSettings.IsFullscreen, "Fullscreen");
		GUILayout.Space(8f);
		_resolutionScrollPosition = GUILayout.BeginScrollView(_resolutionScrollPosition, GUI.skin.box);
		for (int i = 0; i < Screen.resolutions.Length; i++)
		{
			Resolution resolution = Screen.resolutions[i];
			GUI.enabled = !GraphicsSettings.Equals(resolution, GraphicsSettings.CurrentResolution);
			GUI.color = ((!GraphicsSettings.Equals(resolution, GraphicsSettings.SelectedResolution)) ? Color.white : HighlightColor);
			if (GUILayout.Button(resolution.width + ", " + resolution.height))
			{
				GraphicsSettings.SelectedResolution = resolution;
			}
		}
		GUILayout.FlexibleSpace();
		GUILayout.EndScrollView();
		GUI.enabled = true;
		GUI.color = Color.white;
		GUILayout.EndVertical();
		GUILayout.BeginVertical();
		ShowHorizontallyCenteredLabel("Quality Level");
		string[] names = QualitySettings.names;
		_qualityScrollPosition = GUILayout.BeginScrollView(_qualityScrollPosition, GUI.skin.box);
		for (int j = 0; j < names.Length; j++)
		{
			GUI.enabled = j != QualitySettings.GetQualityLevel();
			GUI.color = ((j != GraphicsSettings.SelectedQualityLevel) ? Color.white : HighlightColor);
			if (GUILayout.Button(names[j]))
			{
				GraphicsSettings.SelectedQualityLevel = j;
			}
		}
		GUILayout.FlexibleSpace();
		GUILayout.EndScrollView();
		GUI.enabled = true;
		GUI.color = Color.white;
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.Space(16f);
		GUILayout.BeginHorizontal();
		if (GraphicsSettings.IsDirty)
		{
			if (GUILayout.Button("Reset"))
			{
				GraphicsSettings.Initialize();
			}
			if (GUILayout.Button("Apply"))
			{
				GraphicsSettings.Apply();
			}
		}
		GUILayout.EndHorizontal();
	}

	private void ShowPlayerSelector()
	{
		string[] array = new string[_game.Players.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = "Player " + (_game.Players[i].PlayerId + 1);
		}
		_selectedPlayer = _game.Players[GUILayout.Toolbar(_selectedPlayer.PlayerId, array)];
	}

	private void ShowActionMap(ActionMap map)
	{
		GUI.enabled = !InputManager.Instance.IsRemapping;
		_inputScrollPosition = GUILayout.BeginScrollView(_inputScrollPosition, GUI.skin.box);
		ShowHorizontallyCenteredLabel("Buttons");
		GUILayout.Space(8f);
		foreach (ButtonAction buttonAction in map.ButtonActions)
		{
			ShowButtonAction(buttonAction, map);
		}
		GUILayout.Space(8f);
		ShowHorizontallyCenteredLabel("Axes");
		GUILayout.Space(8f);
		foreach (AxisAction axisAction in map.AxisActions)
		{
			ShowAxisAction(axisAction, map);
		}
		GUILayout.FlexibleSpace();
		GUILayout.Space(16f);
		if (GUILayout.Button("Reset to Defaults"))
		{
			SingletonComponent<AuroraInputSerializer>.Instance.LoadDefault(map);
		}
		GUILayout.FlexibleSpace();
		GUILayout.EndScrollView();
	}

	private void ShowHorizontallyCenteredLabel(string label)
	{
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.Label(label);
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
	}

	private void ShowButtonAction(ButtonAction action, ActionMap map)
	{
		GUILayout.BeginHorizontal(GUI.skin.box);
		GUILayout.Label(action.Name, GUILayout.Width(100f));
		GUILayout.Space(8f);
		if (GUILayout.Button(action.InputName, GUILayout.Width(200f)))
		{
			InputManager.Instance.RemapButton(map, action.Identifier, OnActionRemapped);
		}
		GUILayout.EndHorizontal();
	}

	private void ShowAxisAction(AxisAction action, ActionMap map)
	{
		GUILayout.BeginVertical(GUI.skin.box);
		GUILayout.Label(action.Name, GUILayout.Width(100f));
		ShowSubAxisAction(action, action.Negative, map);
		ShowSubAxisAction(action, action.Positive, map);
		GUILayout.EndVertical();
	}

	private void ShowSubAxisAction(AxisAction action, SubAxisAction subAction, ActionMap map)
	{
		GUILayout.BeginHorizontal();
		GUILayout.Space(16f);
		GUILayout.Label(subAction.Name, GUILayout.Width(100f));
		if (GUILayout.Button(subAction.InputName, GUILayout.Width(200f)))
		{
			InputManager.Instance.RemapAxis(map, action.Identifier, subAction.Polarity, OnActionRemapped);
		}
		IAxisAdapter adapter = subAction.Adapter;
		if (adapter != null)
		{
			GUI.enabled = subAction.Adapter.Input.InputType == InputType.Axis;
			GuiChangeMonitor.Reset();
			adapter.Sensitivity = GUILayout.HorizontalSlider(adapter.Sensitivity, 1f, 30f);
			if (GuiChangeMonitor.HasChanged())
			{
				adapter.Sensitivity = Mathf.Floor(adapter.Sensitivity);
			}
			else
			{
				string s = GUILayout.TextField(adapter.Sensitivity.ToString("0.0"), GUILayout.Width(32f));
				float result;
				if (float.TryParse(s, NumberStyles.AllowDecimalPoint, NumberFormat, out result))
				{
					adapter.Sensitivity = result;
				}
			}
			GUI.enabled = true;
		}
		GUILayout.EndHorizontal();
	}

	private void ShowControlRemappingMessage()
	{
		GUILayout.Space(16f);
		GUILayout.FlexibleSpace();
		GUILayout.Label("Press any key or move any axis...");
		GUILayout.Space(16f);
		GUILayout.Label("Press Escape to Cancel");
	}

	private void ShowControlSaveButtons()
	{
		GUILayout.Space(8f);
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Load"))
		{
			Player[] players = _game.Players;
			foreach (Player player in players)
			{
				SingletonComponent<AuroraInputSerializer>.Instance.Load(player.Input.ActionMap);
			}
		}
		if (GUILayout.Button("Save"))
		{
			Player[] players2 = _game.Players;
			foreach (Player player2 in players2)
			{
				SingletonComponent<AuroraInputSerializer>.Instance.Save(player2.Input.ActionMap);
			}
		}
		GUILayout.EndHorizontal();
	}

	private void OnActionRemapped(ActionMap map, string actionName, IInput input)
	{
		Debug.Log("OnActionRemapped");
	}
}
