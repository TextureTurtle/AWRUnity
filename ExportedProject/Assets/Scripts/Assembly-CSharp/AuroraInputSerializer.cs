#define RAMJET_DEBUG_ERROR
#define RAMJET_DEBUG_LOG
using System;
using System.IO;
using RamjetAnvil.Unity.Debug;
using RamjetAnvil.Unity.Impero;
using UnityEngine;

public class AuroraInputSerializer : SingletonComponent<AuroraInputSerializer>
{
	[SerializeField]
	private string _myGamesFolder = "My Games";

	[SerializeField]
	private string _gameName = "Game Name";

	[SerializeField]
	private string _templateFileName = "InputTemplate";

	[SerializeField]
	private string _configFileNamePrefix = "InputConfig_";

	private string _configFileRelativeFolderPath = "Config" + Path.DirectorySeparatorChar + "Input";

	private string _saveFolderPath;

	private string _defaultConfigPath;

	public string SaveFolderPathPath
	{
		get
		{
			return _saveFolderPath;
		}
	}

	public override void OnInitializeInPlayMode()
	{
		_defaultConfigPath = GetDefaultConfigFolderPath();
		_saveFolderPath = GetConfigFolderPath();
		EnsureDirectoryExists(_saveFolderPath);
	}

	public ActionMap Create(int playerId)
	{
		string defaultConfigFilePath = GetDefaultConfigFilePath(_templateFileName + ".xml");
		DebugUtils.Log("Creating from: " + defaultConfigFilePath);
		return ActionMapXmlSerializer.CreateFromTemplate(defaultConfigFilePath, playerId);
	}

	public bool Load(ActionMap map)
	{
		string configFilePath = GetConfigFilePath(GetConfigFileName(map));
		Debug.Log("Loading from: " + configFilePath);
		if (!ActionMapXmlSerializer.Deserialize(configFilePath, map))
		{
			DebugUtils.Log("No config file found, loading default Action Map");
			return false;
		}
		return true;
	}

	public bool LoadDefault(ActionMap map)
	{
		string defaultConfigFilePath = GetDefaultConfigFilePath(_configFileNamePrefix + "Default.xml");
		DebugUtils.Log("Loading default from: " + defaultConfigFilePath);
		if (ActionMapXmlSerializer.Deserialize(defaultConfigFilePath, map))
		{
			DebugUtils.Log("Loaded default action map");
			return true;
		}
		DebugUtils.LogError("Failed to load both custom and default actionMaps");
		return false;
	}

	public bool Save(ActionMap map)
	{
		string configFilePath = GetConfigFilePath(GetConfigFileName(map));
		DebugUtils.Log("Saving to: " + configFilePath);
		return ActionMapXmlSerializer.Serialize(configFilePath, map);
	}

	private string GetConfigFileName(ActionMap map)
	{
		return _configFileNamePrefix + "Player_" + map.PlayerId + ".xml";
	}

	private string GetConfigFolderPath()
	{
		string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
		string path = Path.Combine(folderPath, Path.Combine(_myGamesFolder, _gameName));
		return Path.Combine(path, _configFileRelativeFolderPath);
	}

	private string GetDefaultConfigFolderPath()
	{
		bool flag = Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXPlayer;
		string path = Path.Combine(Application.dataPath, (!flag) ? "../" : string.Empty);
		return Path.GetFullPath(Path.Combine(path, _configFileRelativeFolderPath));
	}

	private string GetConfigFilePath(string fileName)
	{
		return Path.Combine(_saveFolderPath, fileName);
	}

	private string GetDefaultConfigFilePath(string fileName)
	{
		return Path.Combine(_defaultConfigPath, fileName);
	}

	private void EnsureDirectoryExists(string directoryPath)
	{
		if (!Directory.Exists(directoryPath))
		{
			Directory.CreateDirectory(directoryPath);
		}
	}
}
