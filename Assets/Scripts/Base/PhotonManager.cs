using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public static PhotonManager Instance;

    private List<RoomInfo> roomList = new List<RoomInfo>();

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
		Debug.Log("Connecting to Master");
		PhotonNetwork.ConnectUsingSettings();
	}

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnConnectedToMaster()
	{
        Loading.Instance.Off();
		Debug.Log("Connected to Master");
		PhotonNetwork.JoinLobby();
		PhotonNetwork.AutomaticallySyncScene = true;
	}

    public override void OnJoinedLobby()
	{
		
		Debug.Log("Joined Lobby");
	}

    public void OnJoinButtonClicked()
    {
        foreach (var room in roomList)
        {
            if (room.Name == "master")
            {
                JoinRoom(room);
                return;
            }
        }

        Debug.Log("There's no master room");

        CreateRoom();
    }

    public void CreateRoom()
    {
        Debug.Log("CreateRoom");

        PhotonNetwork.CreateRoom("master");
        Loading.Instance.On();
    }

    public override void OnJoinedRoom()
	{
        Debug.Log("OnJoinedRoom");

		Player[] players = PhotonNetwork.PlayerList;

		for(int i = 0; i < players.Count(); i++)
		{
			
		}

        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.LoadLevel("World");
	}

    public override void OnMasterClientSwitched(Player newMasterClient)
	{
		
	}

    public override void OnCreateRoomFailed(short returnCode, string message)
	{
		Debug.LogError("Room Creation Failed: " + message);
		Loading.Instance.Off();
	}

    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        Loading.Instance.On();
    }

    public void LeaveRoom()
	{
		PhotonNetwork.LeaveRoom();
		Loading.Instance.On();
	}

    public override void OnRoomListUpdate(List<RoomInfo> list)
	{
        roomList.Clear();

        roomList = list;
	}

    public void EnterWorld()
    {
        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.LoadLevel("World");
    }
}