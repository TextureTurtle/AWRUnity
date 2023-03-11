using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BundleLoader
{
	private bool _isDone = true;

	private AssetBundle _bundle;

	public bool IsDone
	{
		get
		{
			return _isDone;
		}
	}

	public IEnumerator Load(string bundleName, Action onLoaded)
	{
		_isDone = false;
		string assetBundleUrl = GetAssetBundleBaseUrl() + bundleName + ".unity3d";
		WWW download = WWW.LoadFromCacheOrDownload(assetBundleUrl, 0);
		download.threadPriority = ThreadPriority.BelowNormal;
		yield return download;
		if (download.error != null)
		{
			Debug.LogError(download.error);
			download.Dispose();
			_isDone = true;
			yield break;
		}
		_bundle = download.assetBundle;
		if (!_bundle)
		{
			download.Dispose();
			Debug.LogWarning("No bundle found");
			_isDone = true;
		}
		if (onLoaded != null)
		{
			onLoaded();
		}
	}

	public IEnumerator LoadAllObjectsAsync(MonoBehaviour coroutineOwner, IEnumerable<string> names, Type type, Action<IEnumerable<UnityEngine.Object>> onObjectsLoaded)
	{
		Debug.Log("LoadAllObjectsAsync");
		List<UnityEngine.Object> loadedObjects = new List<UnityEngine.Object>();
		foreach (string name in names)
		{
			yield return coroutineOwner.StartCoroutine(LoadObjectAync(name, type, delegate(UnityEngine.Object obj)
			{
				loadedObjects.Add(obj);
			}));
		}
		if (onObjectsLoaded != null)
		{
			onObjectsLoaded(loadedObjects);
		}
	}

	public IEnumerator LoadObjectAync(string name, Type type, Action<UnityEngine.Object> onObjectLoaded)
	{
		if (!_bundle)
		{
			Debug.LogError("No bundle load, can't load object");
			yield break;
		}
		AssetBundleRequest request = _bundle.LoadAsync(name, type);
		yield return request;
		if (onObjectLoaded != null)
		{
			onObjectLoaded(request.asset);
		}
	}

	public void Unload()
	{
		Debug.Log("Unloading");
		_isDone = false;
		if (_bundle != null)
		{
			_bundle.Unload(true);
		}
		_isDone = true;
	}

	private static string GetAssetBundleBaseUrl()
	{
		if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXPlayer)
		{
			return "file:///" + Application.dataPath + "/AssetBundles/";
		}
		return "file:///" + Application.dataPath + "/../AssetBundles/";
	}
}
