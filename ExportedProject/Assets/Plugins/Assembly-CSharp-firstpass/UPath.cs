using System.IO;
using UnityEngine;

public static class UPath
{
	private static string projectPath;

	private static string directorySeparatorUnity;

	private static string directorySeparatorSystem;

	static UPath()
	{
		directorySeparatorUnity = "/";
		projectPath = Application.dataPath.Remove(Application.dataPath.IndexOf("/Assets", 7));
		directorySeparatorSystem = Path.DirectorySeparatorChar.ToString();
	}

	public static string Combine(string path1, string path2)
	{
		return path1 + ((!path1.EndsWith("/")) ? "/" : string.Empty) + ((!path2.StartsWith("/")) ? path2 : path2.Substring(1));
	}

	public static string GetAbsolutePath(string path)
	{
		string path2 = Path.Combine(projectPath, path);
		return ConvertPathSeparatorsToSystem(path2);
	}

	public static string GetProjectPath(string path)
	{
		string text = ConvertPathSeparatorsToUnity(path);
		int count = path.IndexOf("Assets");
		text = text.Remove(0, count);
		if (text.StartsWith("/"))
		{
			text = text.Remove(0, 1);
		}
		return text;
	}

	public static string ConvertPathSeparatorsToSystem(string path)
	{
		string text = path.Replace("/", directorySeparatorSystem);
		return text.Replace("\\", directorySeparatorSystem);
	}

	public static string ConvertPathSeparatorsToUnity(string path)
	{
		string text = path.Replace("/", directorySeparatorUnity);
		return text.Replace("\\", directorySeparatorUnity);
	}

	public static bool ProjectFolderExists(string folderPath)
	{
		string absolutePath = GetAbsolutePath(folderPath);
		return Directory.Exists(absolutePath);
	}

	public static void CreateProjectFolder(string folderPath)
	{
		Directory.CreateDirectory(folderPath);
	}
}
