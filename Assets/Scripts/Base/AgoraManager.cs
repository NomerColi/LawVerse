using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using agora_gaming_rtc;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class AgoraManager : MonoBehaviourPunCallbacks
{
    string appID = "a146bf4c54974d1d9dacc69840e74972";

    public static AgoraManager Instance;

    IRtcEngine rtcEngine;

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
   
    void Start()
	{
		if(string.IsNullOrEmpty(appID))
		{
			Debug.LogError("App ID not set in VoiceChatManager script");
			return;
		}

		rtcEngine = IRtcEngine.GetEngine(appID);

		rtcEngine.OnJoinChannelSuccess += OnJoinChannelSuccess;
		rtcEngine.OnLeaveChannel += OnLeaveChannel;
		rtcEngine.OnError += OnError;

		rtcEngine.EnableSoundPositionIndication(true);
	}

	void OnError(int error, string msg)
	{
		Debug.LogError("Error with Agora  error : " + error + " msg : " + msg);
	}

	void OnLeaveChannel(RtcStats stats)
	{
		Debug.Log("AgoraManager Left channel with duration " + stats.duration);

		var players = FindObjectsOfType<PlayerController>();
        foreach (var p in players)
            p.ClearVideo();
	}

	void OnJoinChannelSuccess(string channelName, uint uid, int elapsed)
	{
		Debug.Log("AgoraManager Joined channel " + channelName + " uid : " + uid + " elasped : " + elapsed);

		Hashtable hash = new Hashtable();
		hash.Add("agoraID", uid.ToString());
		PhotonNetwork.SetPlayerCustomProperties(hash);
	}

	public IRtcEngine GetRtcEngine()
	{
		return rtcEngine;
	}

	public override void OnJoinedRoom()
	{
		Debug.Log("OnJoinedroom name : " + PhotonNetwork.CurrentRoom.Name);

		rtcEngine.EnableVideo();
		rtcEngine.EnableVideoObserver();

		rtcEngine.JoinChannel(PhotonNetwork.CurrentRoom.Name);
	}

	public override void OnLeftRoom()
	{
		Debug.Log("OnLeftroom");

		rtcEngine.LeaveChannel();
		rtcEngine.DisableVideo();
		rtcEngine.DisableVideoObserver();
	}

	void OnDestroy()
	{
		Debug.Log("OnDestroy");

		IRtcEngine.Destroy();
	}

	[TestMethod]
	public void EnableVideo()
	{
		rtcEngine.EnableVideo();
		rtcEngine.EnableVideoObserver();
	}
}
