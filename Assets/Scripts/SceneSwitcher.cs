using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class SceneSwitcher : MonoBehaviour
{
    void Update() 
    {
        if (Input.GetKey(KeyCode.Backspace))
        {
            Application.Quit();
            // Launcher.Instance.LeaveRoom();
            // VoiceChatManager.Instance.GetRtcEngine().LeaveChannel();
            // SceneSwitch("Menu");
        }
    }

    public void SceneSwitch(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
