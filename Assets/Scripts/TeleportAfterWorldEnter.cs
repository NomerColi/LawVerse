using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportAfterWorldEnter : MonoBehaviour
{
    public static TeleportAfterWorldEnter Instance;

    void Awake()
	{
		if(Instance)
		{
			Destroy(gameObject);
		}
		else
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}

    public Vector3 toPos;
    public Vector3 toRot;
}
