using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// </summary>
/// <typeparam name="T"></typeparam>


public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T> {
	protected static T st_instance;
	public static T Instance {
		get {
			if (st_instance == null) {
				var i = Resources.FindObjectsOfTypeAll<T>()
							.Where(o=>o.gameObject.scene.name != null)
							.ToArray();

				if (i.Length == 0) {
					Debug.LogError("scene안에 해당하는 컴포넌트가 없습니다.  name : " + typeof(T).Name);
					return null;
				}else if( i.Length > 1){
					Debug.LogWarning( "scene안에 2개이상 object가 있습니다. name : " + typeof(T).Name);
					st_instance = i.First();
				}else{
					st_instance = i.First();
				}
			}
			return st_instance;
		}
	}

	public static bool IsExist{
		get{
			if(st_instance != null)
				return true;
			

			var i = Resources.FindObjectsOfTypeAll<T>()
							.Where(o=>o.gameObject.scene.name != null)
							.ToArray();

			if(i.Length == 0)
				return false;
			else
				return true;
		}
	}
	
	private void OnApplicationQuit() {
		st_instance = null;
	}
}