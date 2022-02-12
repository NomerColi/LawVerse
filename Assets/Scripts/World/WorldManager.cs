using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using agora_gaming_rtc;

public class WorldManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    public static WorldManager Instance;

	public Image myVideoSurfaceMaskImage;
	public RawImage myVideoSurfaceRawImage;
    public VideoSurface myVideoSurface;
	public Camera myMinimapCamera;

	public List<GameObject> popupList;
	public GameObject escapeUI;
	public GameObject mapUI;
	public GameObject tutorialUI;
	public VideoScreenPopUp screenUI;
	public ElevatorPopUp elevatorUI;


	public GameObject myTicketUI;

	public GameObject sitUI;
	public GameObject getTicketUI;

	public bool uiOn = false;

	public WorldPlayer myPlayer;

	public Table table;

	public NumberPanel numberPanel;

	[SerializeField] private float teleportTime;


	private bool bEnable = false;

	void Awake()
	{
		if(Instance)
		{
			Destroy(gameObject);
			return;
		}
		
		Instance = this;
	}

    IEnumerator Start()
    {
		yield return null;
    }

	void Update() 
    {
		if (!bEnable)
			return;
		
		bool uiToggled = false;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
			Debug.Log("GetKey escape");

			uiToggled = true;

			if (!CloseAllPopUp())
            	escapeUI.SetActive(true);
        }
		else if (Input.GetKeyDown(KeyCode.Tab))
		{
			if (!mapUI.activeSelf)
				CloseAllPopUp();

			Debug.Log("GetKey M");
			uiToggled = true;
			mapUI.SetActive(!mapUI.activeSelf);
		}
		else if (Input.GetMouseButtonDown(0))
		{
			if (tutorialUI.activeSelf)
				tutorialUI.SetActive(false);
			else if (sitUI.activeSelf)
			{
				nearChair.OnClick();
				sitUI.SetActive(false);
			}
			else if (getTicketUI.activeSelf)
			{
				myTicketUI.SetActive(true);

				numberPanel.SetNumberAfter();
			}
			else if (myPlayer.bSit)
			{
				StandUp();
			}
		}
		else if (Input.GetKeyDown(KeyCode.B))
		{
			myPlayer.PlayWaving();
		}
		else if (Input.GetKeyDown(KeyCode.N))
		{
			myPlayer.PlayThumbUp();
		}
		else if (Input.GetKeyDown(KeyCode.M))
		{
			myPlayer.PlayCalling();
		}

		if (true)
		{
			uiOn = false;

			foreach (var popup in popupList)
			{
				if (popup.activeSelf)
					uiOn = true;
			}

			if (myPlayer.bSit)
				uiOn = true;

			//Debug.Log("UI TOGGELD   uiON : " + uiOn);

			Cursor.visible = uiOn;
			Cursor.lockState = uiOn ? CursorLockMode.None : CursorLockMode.Locked;
		}
    }

	public void EnableChild(bool b)
	{
		Debug.Log("EnableChild b : " + b);

		bEnable = true;

		foreach (Transform t in transform)
		{
			t.gameObject.SetActive(b);
		}

		StartCoroutine(PlayerInitRoutine());
	}

	private IEnumerator PlayerInitRoutine()
	{
		InstantiateMyPlayer();
				
		if (TeleportAfterWorldEnter.Instance.toPos != Vector3.zero)
		{
			Debug.Log("TeleportAfterWorldEnter toPos : " + TeleportAfterWorldEnter.Instance.toPos);

			Teleport(TeleportAfterWorldEnter.Instance.toPos, TeleportAfterWorldEnter.Instance.toRot);
			
			yield return new WaitForSeconds(teleportTime);
		}

        Loading.Instance.Off();

		//tutorialUI.gameObject.SetActive(true);
	}

	public bool CloseAllPopUp()
	{
		Debug.Log("CloseAllPopUp");

		foreach (var popup in popupList)
		{
			if (popup.activeSelf)
			{
				popup.SetActive(false);
				return true;
			}
		}

		return false;
	}

	public void EnableMyVideoSurface()
	{
		myVideoSurfaceMaskImage.enabled = true;
		myVideoSurfaceRawImage.enabled = true;
	}

	public void InstantiateMyPlayer()
	{
		Debug.Log("Instantiate PlayerManager");
		var p = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Player"), new Vector3(0, 1, 0), Quaternion.identity);
		myMinimapCamera.transform.SetParent(p.transform);
		myMinimapCamera.transform.localPosition = new Vector3(0, myMinimapCamera.transform.position.y, 0);

		myPlayer = p.GetComponent<WorldPlayer>();
	}

	public void OnResumeButtonClicked()
	{

	}

	public void OnDisconnectButtonClicked()
	{
		Debug.Log("OnDisconnectButtonClicked");

		PhotonNetwork.LoadLevel("Main");
		PhotonNetwork.LeaveRoom();
		Loading.Instance.On();
	}

	public void OpenVideoScreenPopUp(VideoScreen screen)
	{
		Debug.Log("OpenVideoScreenPopUp");

		uiOn = true;

		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;

		screenUI.gameObject.SetActive(true);
		screenUI.Setup(screen);
	}

	public void OpenElevatorPopUp(Elevator elevator)
	{
		Debug.Log("OpenElevatorPopUp");
		
		uiOn = true;

		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;

		elevatorUI.gameObject.SetActive(true);
		elevatorUI.Setup(elevator);
	}


	public void Teleport(Vector3 pos, Vector3 rot)
    {
		Debug.Log("Teleport pos : " + pos + " rot : " + rot);

		CloseAllPopUp();
        StartCoroutine(TeleportRoutine(pos, rot));
    }

    private IEnumerator TeleportRoutine(Vector3 pos, Vector3 rot)
    {
        Loading.Instance.On();

        myPlayer.transform.position = pos;
        myPlayer.transform.localEulerAngles = rot;

        yield return new WaitForSeconds(teleportTime);

        Loading.Instance.Off();
        WorldManager.Instance.mapUI.gameObject.SetActive(false);
    }

	public void Sit(Chair chair)
	{
		Debug.Log("Sit");

		chair.EnablePanel(false);

		var pos = chair.transform.position + chair.sitPos;
		var rot = chair.transform.eulerAngles + chair.sitRot;
		
		myPlayer.Sit(pos, rot, table.chairList.IndexOf(chair));

		table.playerList.Add(myPlayer);
		table.agoraUIDList.Add(myPlayer.agoraUID);
		table.EnableUI();

		photonView.RPC("RPC_JoinTable", RpcTarget.Others, myPlayer.agoraUID.ToString());
	}

	[PunRPC]
	public void RPC_JoinTable(string uid)
	{
		table.OnPlayerJoin(uint.Parse(uid));
	}

	public void StandUp()
	{
		myPlayer.StandUp();

		table.agoraUIDList.Remove(myPlayer.agoraUID);
		table.DisableUI();
		
		photonView.RPC("RPC_ExitTable", RpcTarget.Others, myPlayer.agoraUID.ToString());
	}

	[PunRPC]
	public void RPC_ExitTable(string uid)
	{
		table.OnPlayerExit(uint.Parse(uid));
	}

	private Chair nearChair;
	public void SetNearChair(Chair chair)
	{
		nearChair = chair;
		sitUI.SetActive(true);
	}

	public void ClearNearChair(Chair chair)
	{
		if (nearChair == chair)
		{
			nearChair = null;
			sitUI.SetActive(false);
		}
	}
}