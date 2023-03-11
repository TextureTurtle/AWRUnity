using System.Collections.Generic;
using UnityEngine;

namespace RamjetAnvil.Unity.Utility
{
	public static class MonoBehaviourExtensions
	{
		public static void SetActive(this GameObject me, bool active)
		{
			me.SetActive(active);
		}

		public static bool IsActive(this GameObject me)
		{
			return me.activeSelf;
		}

		public static List<T> GetComponentsOfInterface<T>(this GameObject me) where T : class
		{
			MonoBehaviour[] components = me.GetComponents<MonoBehaviour>();
			return FilterForInterface<T>(components);
		}

		public static List<T> GetComponentsOfInterfaceInChildren<T>(this GameObject me) where T : class
		{
			MonoBehaviour[] componentsInChildren = me.GetComponentsInChildren<MonoBehaviour>();
			return FilterForInterface<T>(componentsInChildren);
		}

		public static List<T> GetComponentsOfInterface<T>(this Behaviour me) where T : class
		{
			MonoBehaviour[] components = me.GetComponents<MonoBehaviour>();
			return FilterForInterface<T>(components);
		}

		public static List<T> GetComponentsOfInterfaceInChildren<T>(this Behaviour me) where T : class
		{
			MonoBehaviour[] componentsInChildren = me.GetComponentsInChildren<MonoBehaviour>();
			return FilterForInterface<T>(componentsInChildren);
		}

		private static List<T> FilterForInterface<T>(IEnumerable<MonoBehaviour> behaviours) where T : class
		{
			List<T> list = new List<T>();
			foreach (MonoBehaviour behaviour in behaviours)
			{
				T val = behaviour as T;
				if (val != null)
				{
					list.Add(val);
				}
			}
			return list;
		}

		public static T GetComponentInParents<T>(Transform transform) where T : Behaviour
		{
			T component = transform.GetComponent<T>();
			if ((bool)(Object)component)
			{
				return component;
			}
			if ((bool)transform.parent)
			{
				return GetComponentInParents<T>(transform.parent);
			}
			return (T)null;
		}

		public static List<T> FindComponentsOfInterface<T>() where T : class
		{
			MonoBehaviour[] behaviours = Object.FindObjectsOfType(typeof(MonoBehaviour)) as MonoBehaviour[];
			return FilterForInterface<T>(behaviours);
		}

		public static List<T> FindComponentsOfInterfaceIncludingAssets<T>() where T : class
		{
			MonoBehaviour[] behaviours = Object.FindObjectsOfTypeIncludingAssets(typeof(MonoBehaviour)) as MonoBehaviour[];
			return FilterForInterface<T>(behaviours);
		}

		public static void DestroyAgnostic(Object obj)
		{
			DestroyAgnostic(obj, false);
		}

		public static void DestroyAgnostic(Object obj, bool allowDestroyingAssets)
		{
			if (!(obj == null))
			{
				if (Application.isPlaying)
				{
					Object.Destroy(obj);
				}
				else
				{
					Object.DestroyImmediate(obj, allowDestroyingAssets);
				}
			}
		}
	}
}
