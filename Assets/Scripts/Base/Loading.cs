using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Loading : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [SerializeField] TMP_Text text;

    public static Loading Instance;

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

    public void On(string msg = "")
    {
        if (!string.IsNullOrEmpty(msg))
            text.text = msg;

        canvas.gameObject.SetActive(true);
    }

    public void Off()
    {
        canvas.gameObject.SetActive(false);
    }
}
