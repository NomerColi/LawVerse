using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using agora_gaming_rtc;
using agora_utilities;
using AgoraNative;
using uWindowCapture;

#if UNITY_EDITOR || UNITY_STANDALONE

public class VideoScreenPopUp : MonoBehaviour
{
    public VideoScreen screen;

    public RawImage sampleImage;

    public Dropdown dropdown;

    int CurrentDisplay = 0;

    private void Update() 
    {
        if (!screen.windowTexture.enabled || screen.windowTexture.window == null || screen.windowTexture.window.texture == null)
        {
            sampleImage.texture = null;

            return;
        }

        sampleImage.texture = screen.windowTexture.window.texture;
    }

    public void Setup(VideoScreen scr)
    {
        screen = scr;

        StartCoroutine(SetupRoutine());
    }

    private IEnumerator SetupRoutine()
    {
        yield return new WaitForSeconds(1f);

        var log = "before Window list : \n";
        foreach (var window in UwcManager.windows)
        {
            log += "idx : " + window.Key + " title : " + window.Value.title + " isAlive : " + window.Value.isAlive + " \n";
        }
        Debug.Log(log);

        var windowDic = new Dictionary<int, UwcWindow>();
        for (int i = 0; i < UwcManager.windows.Count; i++)
        {
            var window = UwcManager.windows[i];
            if (string.IsNullOrWhiteSpace(window.title) || !window.isAlive)
            {
                
            }
            else
                windowDic.Add(i, window);
        }

        log = "after Window list : \n";
        foreach (var window in windowDic)
        {
            log += "idx : " + window.Key + " title : " + window.Value.title + " isAlive : " + window.Value.isAlive + " \n";
        }
        Debug.Log(log);

        dropdown.options = windowDic.Select(w => new Dropdown.OptionData(w.Value.title)).ToList();

        dropdown.onValueChanged.AddListener(delegate{OnDropdownValueChanged(dropdown);});
    }

    public void OnDropdownValueChanged(Dropdown drop)
    {
        screen.windowTexture.partialWindowTitle = drop.options[dropdown.value].text;
    }

    public void PreviewScreenShare()
    {
        screen.windowTexture.partialWindowTitle = dropdown.options[dropdown.value].text;
    }

    public void StartScreenShare()
    {
        screen.windowTexture.enabled = true;
        screen.windowTexture.partialWindowTitle = dropdown.options[dropdown.value].text;
        screen.StartSharing();
    }

    public void StopScreenCapture()
    {
        screen.StopSharing();

        screen.windowTexture.partialWindowTitle = "";
        screen.enabled = false;
        sampleImage.texture = null;
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}

#endif