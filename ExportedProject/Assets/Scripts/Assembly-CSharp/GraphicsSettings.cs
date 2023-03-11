using UnityEngine;

public static class GraphicsSettings
{
	private static bool _isFullScreen;

	private static Resolution _selectedResolution;

	private static int _selectedTargetFramerate = 60;

	private static int _selectedQualityLevel;

	private static bool _isResolutionDirty;

	private static bool _isQualityDirty;

	public static bool IsFullscreen
	{
		get
		{
			return _isFullScreen;
		}
		set
		{
			if (value != _isFullScreen)
			{
				_isFullScreen = value;
				_isResolutionDirty = true;
			}
		}
	}

	public static Resolution SelectedResolution
	{
		get
		{
			return _selectedResolution;
		}
		set
		{
			if (!Equals(value, _selectedResolution))
			{
				_selectedResolution = value;
				_isResolutionDirty = true;
			}
		}
	}

	public static Resolution CurrentResolution
	{
		get
		{
			Resolution result = default(Resolution);
			result.width = Screen.width;
			result.height = Screen.height;
			return result;
		}
	}

	public static int SelectedQualityLevel
	{
		get
		{
			return _selectedQualityLevel;
		}
		set
		{
			if (value != _selectedQualityLevel)
			{
				_selectedQualityLevel = value;
				_isQualityDirty = true;
			}
		}
	}

	public static bool IsDirty
	{
		get
		{
			return _isResolutionDirty || _isQualityDirty;
		}
	}

	public static void Apply()
	{
		if (_isResolutionDirty)
		{
			Screen.SetResolution(_selectedResolution.width, _selectedResolution.height, _isFullScreen);
			_isResolutionDirty = false;
		}
		if (_isQualityDirty)
		{
			QualitySettings.SetQualityLevel(_selectedQualityLevel);
			_isQualityDirty = false;
		}
	}

	public static void Initialize()
	{
		_isFullScreen = Screen.fullScreen;
		Resolution selectedResolution = default(Resolution);
		selectedResolution.width = Screen.width;
		selectedResolution.height = Screen.height;
		_selectedResolution = selectedResolution;
		_selectedQualityLevel = QualitySettings.GetQualityLevel();
		Application.targetFrameRate = _selectedTargetFramerate;
		_isResolutionDirty = false;
		_isQualityDirty = false;
	}

	public static bool Equals(Resolution a, Resolution b)
	{
		return a.width == b.width && a.height == b.height;
	}
}
