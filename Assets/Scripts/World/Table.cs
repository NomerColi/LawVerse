using agora_gaming_rtc;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Table : MonoBehaviour
{
    public Canvas canvas;
    
    public List<VideoSurface> videoSurfaceList;
    public List<Chair> chairList;

    public List<WorldPlayer> playerList;
    public List<uint> agoraUIDList;

    public RawImage screenImage;
    public VideoScreen videoScreen;

    // Start is called before the first frame update
    void Start()
    {
        canvas.gameObject.SetActive(false);

        foreach (var v in videoSurfaceList)
        {
            v.transform.parent.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!canvas.gameObject.activeInHierarchy)
            return;

        // Debug.Log("Table update video screen !enabled : " + !videoScreen.windowTexture?.enabled + " window null : " + (videoScreen.windowTexture?.window == null) + " texture null : " + (videoScreen.windowTexture?.window?.texture == null));
        // if (!videoScreen.windowTexture.enabled || videoScreen.windowTexture.window == null || videoScreen.windowTexture.window.texture == null)
        // {
        //     screenImage.texture = null;

        //     return;
        // }

        screenImage.texture = videoScreen.windowTexture.material_.mainTexture;
    }

    public void EnableUI()
    {
        canvas.gameObject.SetActive(true);

        for (int i = 0; i < agoraUIDList.Count; i++)
        {
            var v = videoSurfaceList[i];
            v.transform.parent.gameObject.SetActive(true);
            if (agoraUIDList[i] != WorldManager.Instance.myPlayer.agoraUID)
                v.SetForUser(agoraUIDList[i]);
            v.SetEnable(true);
            v.SetVideoSurfaceType(AgoraVideoSurfaceType.RawImage);

            // var parent = videoSurfaceList[i].transform.parent;
            // parent.gameObject.SetActive(true);
            // Destroy(videoSurfaceList[i].gameObject);
            // var obj = new GameObject("VideoSurface__");
            // obj.transform.SetParent(parent);
            // obj.AddComponent<RawImage>();
            // obj.transform.localScale = Vector3.one;
            // var rectTrans = obj.GetComponent<RectTransform>();
            // rectTrans.sizeDelta = new Vector3(300f, 200f);
            // rectTrans.localPosition = new Vector3(rectTrans.localPosition.x, rectTrans.localPosition.y, 0);
            // rectTrans.localRotation = new Quaternion(0, 0, 180f, 0);
            // var v = obj.AddComponent<VideoSurface>();
            // v.SetForUser(agoraUIDList[i]);
            // v.SetEnable(true);
            // v.SetVideoSurfaceType(AgoraVideoSurfaceType.RawImage);

            // videoSurfaceList[i] = v;
        }
    }

    public void DisableUI()
    {
        canvas.gameObject.SetActive(false);

        foreach (var v in videoSurfaceList)
        {
            v.SetEnable(false);
            v.transform.parent.gameObject.SetActive(false);
        }
    }

    public void OnPlayerJoin(uint uid)
    {
        Debug.Log("Table OnPlayerJoin uid : " + uid);

        agoraUIDList.Add(uid);

        if (canvas.gameObject.activeSelf)
        {
            var v = videoSurfaceList[agoraUIDList.Count - 1];
            v.transform.parent.gameObject.SetActive(true);
            v.SetForUser(uid);
            v.SetEnable(true);
            v.SetVideoSurfaceType(AgoraVideoSurfaceType.RawImage);
        }
    }

    public void OnPlayerExit(uint uid)
    {
        Debug.Log("Table OnPlayerExit uid : " + uid);

        agoraUIDList.Remove(uid);
    }

    [TestMethod]
    public void EnableVideo(int idx, int b)
    {
        videoSurfaceList[idx].SetEnable(b == 1);
    }

    [TestMethod]
    public void SetUserVideo(int idx, string uid)
    {
        videoSurfaceList[idx].SetForUser(uint.Parse(uid));
    }
}
