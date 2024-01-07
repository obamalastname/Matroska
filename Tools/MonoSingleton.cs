using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Matroska.Tools
{
	public abstract class MonoSingleton<T> : ExtendedMonoBehavior where T : MonoSingleton<T>
	{
		private static T _instance;
		public static T Instance
		{
			get
			{
				if (_instance == null)
				{
					Debug.LogError(typeof(T).ToString() + " is missing.");
				}

				return _instance;
			}
		}
		
		void Awake()
		{
			if (_instance != null)
			{
				Debug.LogError("Second instance of " + typeof(T) + " created. This is not supposed to happen");
				Destroy(this.gameObject);
			}
			_instance = this as T;
			Init();
		}

		void OnDestroy()
		{
			if (_instance == this)
			{
				_instance = null;
			}
		}
		
		/// <summary>
		/// Singleton is initialized
		/// </summary>
		public virtual void Init() { }
	}
}