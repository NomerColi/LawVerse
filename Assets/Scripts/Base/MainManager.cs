using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainManager : MonoBehaviour
{
    [SerializeField] TMP_InputField usernameInput;

    public List<Toggle> toggleList;


    void Start()
    {
        Screen.SetResolution(1280, 720, false);
    }

    public void OnToggle(int idx)
    {
        if (!toggleList[idx].isOn)
            return;

        for (int i = 0; i < toggleList.Count; i++)
        {
            if (i != idx)
            toggleList[i].isOn = false;
        }
    }

    public void OnJoinButtonClicked()
    {
        int toggleIdx = 0;

        for (int i = 0; i < toggleList.Count; i++)
        {
            if (toggleList[i].isOn)
            {
                toggleIdx = i;
                break;
            }
        }

        if (!string.IsNullOrEmpty(usernameInput.text) && toggleIdx != -1)
        {
            PlayerPrefs.SetString("username", usernameInput.text);

            PlayerPrefs.SetInt("type", toggleIdx);
            string typeStr = "유저";
            if (toggleIdx == 1)
                typeStr = "변호사";
            else if (toggleIdx == 2)
                typeStr = "관리자";
            PlayerPrefs.SetString("typename", typeStr);
            PhotonManager.Instance.OnJoinButtonClicked();
        }
    }
}
