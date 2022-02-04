using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using agora_gaming_rtc;
using System;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class VoiceChatManager : MonoBehaviourPunCallbacks
{
	string appID = "a146bf4c54974d1d9dacc69840e74972";

	public static VoiceChatManager Instance;

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
		Debug.Log("Left channel with duration " + stats.duration);

		var players = FindObjectsOfType<PlayerController>();
        foreach (var p in players)
            p.ClearVideo();
	}

	void OnJoinChannelSuccess(string channelName, uint uid, int elapsed)
	{
		Debug.Log("Joined channel " + channelName + " uid : " + uid + " elasped : " + elapsed);

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
		Debug.Log("OnLeftedroom");

		rtcEngine.LeaveChannel();
	}

	void OnDestroy()
	{
		IRtcEngine.Destroy();
	}
}
