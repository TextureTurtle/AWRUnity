using System;
using System.Collections.Generic;
using UnityEngine;

public class WorldStreamer : SingletonComponent<WorldStreamer>
{
	[SerializeField]
	private WorldTransform _perspective;

	[SerializeField]
	private double _zoneSize = 1000.0;

	[SerializeField]
	private int _loadRange = 3;

	[SerializeField]
	private GameObject _startIslandPrefab;

	[SerializeField]
	private GameObject _northPolePrefab;

	[SerializeField]
	private string _biomeBundleName = "TestBiome";

	[SerializeField]
	private string[] _islandNames;

	[SerializeField]
	private bool _forceCustomSeed;

	[SerializeField]
	private int _customSeed = 12345;

	[SerializeField]
	private bool _spawnItems = true;

	[SerializeField]
	private int _maxAttempts = 32;

	[SerializeField]
	private float _minHeight = 60f;

	[SerializeField]
	private float _maxSlope = 25f;

	[SerializeField]
	private float _height = 0.2f;

	[SerializeField]
	private SpawnableItem[] _spawnableItems;

	private bool _stream;

	private ZoneIndex _lastZoneIndex;

	private ZoneIndex _currentZoneIndex;

	private Dictionary<ZoneIndex, GameObject> _loadedZones;

	private BundleLoader _biomeTerrainLoader;

	private List<GameObject> _islands;

	private ZoneIndex _startIndex;

	public double ZoneSize
	{
		get
		{
			return _zoneSize;
		}
	}

	public int LoadRange
	{
		get
		{
			return _loadRange;
		}
	}

	public event Action OnReady;

	public void LoadInitialState()
	{
		_biomeTerrainLoader = new BundleLoader();
		StartCoroutine(_biomeTerrainLoader.Load(_biomeBundleName, delegate
		{
			StartCoroutine(_biomeTerrainLoader.LoadAllObjectsAsync(this, _islandNames, typeof(Terrain), OnFirstIslandsLoaded));
		}));
	}

	private void OnFirstIslandsLoaded(IEnumerable<UnityEngine.Object> objects)
	{
		foreach (UnityEngine.Object @object in objects)
		{
			OnIslandLoaded(@object);
		}
		if (this.OnReady != null)
		{
			this.OnReady();
		}
	}

	public void StartStreaming()
	{
		_stream = true;
		DVector3 position = _perspective.Position;
		_currentZoneIndex = ZoneIndex.FromPosition(position, _zoneSize);
		_lastZoneIndex = _currentZoneIndex;
		_startIndex = _currentZoneIndex;
		Stream(position);
	}

	public void StopStreaming()
	{
		_stream = false;
	}

	private void Awake()
	{
		if (!IsOdd(_loadRange))
		{
			_loadRange++;
		}
		_loadedZones = new Dictionary<ZoneIndex, GameObject>();
		_islands = new List<GameObject>();
		if (!_forceCustomSeed)
		{
			System.Random random = new System.Random();
			_customSeed = random.Next(4096);
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (_biomeTerrainLoader != null)
		{
			_biomeTerrainLoader.Unload();
		}
	}

	private void OnBundleLoaded()
	{
		_biomeTerrainLoader.LoadObjectAync("bla", typeof(Terrain), OnIslandLoaded);
	}

	private void OnIslandLoaded(UnityEngine.Object loadedObject)
	{
		if (loadedObject == null)
		{
			Debug.LogError("Failed to properly load island");
			return;
		}
		Debug.Log("Island loaded: " + loadedObject.name);
		Terrain terrain = loadedObject as Terrain;
		if (!terrain)
		{
			Debug.LogError("Loaded object is not a Terrain");
		}
		else
		{
			_islands.Add(terrain.gameObject);
		}
	}

	private void Update()
	{
		DVector3 position = _perspective.Position;
		_currentZoneIndex = ZoneIndex.FromPosition(position, _zoneSize);
		if (_stream && _currentZoneIndex != _lastZoneIndex)
		{
			Stream(position);
		}
		_lastZoneIndex = _currentZoneIndex;
	}

	private void Stream(DVector3 center)
	{
		ZoneIndex zoneIndex = ZoneIndex.FromPosition(center, _zoneSize);
		List<ZoneIndex> list = new List<ZoneIndex>(9);
		for (int i = -1; i < 2; i++)
		{
			for (int j = -1; j < 2; j++)
			{
				ZoneIndex zoneIndex2 = new ZoneIndex(zoneIndex.X + i, zoneIndex.Z + j);
				list.Add(zoneIndex2);
				if (!_loadedZones.ContainsKey(zoneIndex2))
				{
					LoadZone(zoneIndex2);
				}
			}
		}
		List<ZoneIndex> list2 = new List<ZoneIndex>();
		Dictionary<ZoneIndex, GameObject>.KeyCollection keys = _loadedZones.Keys;
		foreach (ZoneIndex item in keys)
		{
			if (!list.Contains(item))
			{
				list2.Add(item);
			}
		}
		foreach (ZoneIndex item2 in list2)
		{
			UnloadZone(item2);
		}
	}

	private static bool IsOdd(int number)
	{
		return number % 2 != 0;
	}

	private void LoadZone(ZoneIndex index)
	{
		System.Random random = new System.Random(_customSeed + index.GetHashCode());
		GameObject gameObject;
		if (index == _startIndex)
		{
			gameObject = _startIslandPrefab;
		}
		else if (index == ZoneIndex.Zero)
		{
			gameObject = _northPolePrefab;
		}
		else
		{
			int index2 = random.Next(_islands.Count);
			gameObject = _islands[index2];
		}
		StreamedObject component = gameObject.GetComponent<StreamedObject>();
		double num = component.Size.x;
		DVector3 dVector = ZoneIndex.ToPosition(index, _zoneSize);
		DVector3 dVector2;
		if (index == ZoneIndex.Zero)
		{
			double num2 = _zoneSize * 0.5 - num * 0.5;
			dVector2 = new DVector3(num2, 0.0, num2);
		}
		else
		{
			double num3 = _zoneSize - num;
			dVector2 = new DVector3(random.NextDouble() * num3, 0.0, random.NextDouble() * num3);
		}
		DVector3 position = dVector + dVector2;
		GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject) as GameObject;
		if ((bool)gameObject2)
		{
			Debug.Log(string.Format("Succesfully populated zone {0}", index));
			WorldTransform component2 = gameObject2.GetComponent<WorldTransform>();
			component2.Position = position;
			_loadedZones.Add(index, gameObject2);
			Terrain component3 = gameObject2.GetComponent<Terrain>();
			component3.castShadows = false;
			component3.heightmapPixelError = 8f;
			if (_spawnItems && index != _startIndex)
			{
				PlaceItemsOnTerrain(_spawnableItems, component3.terrainData, _maxAttempts, _minHeight, _maxSlope, _height);
			}
		}
		else
		{
			Debug.Log(string.Format("Failed to populate zone {0}", index));
		}
	}

	private void UnloadZone(ZoneIndex index)
	{
		Debug.Log(string.Format("Unloading zone {0}", index));
		GameObject obj = _loadedZones[index];
		UnityEngine.Object.Destroy(obj);
		_loadedZones.Remove(index);
	}

	public void PlaceItemsOnTerrain(SpawnableItem[] items, TerrainData terrainData, int maxAttempts, float minAltitude, float maxSlope, float localHeight)
	{
		Debug.Log(" -- Spawning for: " + terrainData.name);
		Vector3 size = terrainData.size;
		for (int i = 0; i < maxAttempts; i++)
		{
			Vector3 origin = Terrain.activeTerrain.transform.position + size * 0.5f;
			float num = UnityEngine.Random.Range(0f, size.x);
			float num2 = UnityEngine.Random.Range(0f, size.z);
			origin.x += num;
			origin.z += num2;
			origin.y += size.y + localHeight;
			RaycastHit hitInfo;
			if (!Physics.Raycast(origin, -Vector3.up, out hitInfo, float.PositiveInfinity))
			{
				continue;
			}
			float num3 = Vector3.Angle(hitInfo.normal, Vector3.up);
			float y = hitInfo.point.y;
			if (!(y > minAltitude) || !(num3 < maxSlope))
			{
				continue;
			}
			foreach (SpawnableItem spawnableItem in items)
			{
				float num4 = UnityEngine.Random.Range(0f, 1f);
				if (num4 < spawnableItem.Chance)
				{
					Debug.Log("Spawning: " + spawnableItem.ItemPrefab.name);
					Vector3 position = hitInfo.point + Vector3.up * localHeight;
					UnityEngine.Object.Instantiate(spawnableItem.ItemPrefab, position, Quaternion.identity);
				}
			}
		}
	}

	private void OnDrawGizmos()
	{
		if (Application.isPlaying)
		{
			Gizmos.color = Color.magenta;
			DVector3 center = ZoneIndex.GetCenter(_currentZoneIndex, _zoneSize);
			Vector3 center2 = SingletonComponent<World>.Instance.WorldPointToLocal(center);
			Gizmos.DrawWireCube(center2, Vector3.one * (float)_zoneSize);
			double num = ZoneSize * 0.5;
			DVector3 dVector = new DVector3(num, 0.0, num);
			if (DVector3.Distance(_perspective.Position, dVector) < 4000.0)
			{
				Vector3 center3 = SingletonComponent<World>.Instance.WorldPointToLocal(dVector);
				Gizmos.color = Color.red;
				Gizmos.DrawSphere(center3, 100f);
			}
		}
	}
}
