using agora_gaming_rtc;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class WorldPlayer : MonoBehaviourPunCallbacks
{
	[SerializeField] GameObject ui;

	[SerializeField] GameObject cameraHolder;

	[SerializeField] float mouseSensitivity, sprintSpeed, walkSpeed, jumpForce, smoothTime;
	[SerializeField] float interactionDistance = 3f;

	float verticalLookRotation;
	bool grounded;
	Vector3 smoothMoveVelocity;
	Vector3 moveAmount;

	Collider col;
	Rigidbody rb;

	PhotonView PV;

	public VideoSurface videoSurface;

	public Animator animator;

	public Transform geometry;

	public SpriteRenderer playerIcon;

	public bool bSit = false;
	[SerializeField] float sitXRot;

	private bool isActing = false;

	public uint agoraUID;

	void Awake()
	{
		col = GetComponent<Collider>();
		rb = GetComponent<Rigidbody>();
		PV = GetComponent<PhotonView>();

		if (PV.IsMine)
		{
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
		}
	}

	void Start()
	{
		if(PV.IsMine)
		{
			Destroy(ui);

			SetVideoSurface(WorldManager.Instance.myVideoSurface);
			WorldManager.Instance.EnableMyVideoSurface();

			geometry.GetChild(PlayerPrefs.GetInt("Avatar")).gameObject.SetActive(true);
		}
		else
		{
			geometry.GetChild(Random.Range(0, 2)).gameObject.SetActive(true);

			Destroy(GetComponentInChildren<Camera>().gameObject);
			Destroy(rb);
			Destroy(playerIcon.gameObject);

			SetVideoSurface(videoSurface);
		}
	}

	Vector3 prevPos = Vector3.zero;
	void Update()
	{
		if (bSit)
		{
			animator.SetBool("isWalking", false);
			animator.SetBool("isRunning", false);

			return;
		}

		if(PV.IsMine)
		{
			if (isActing)
				return;

			Look();
			Move();
		}
		else 
		{
			if (transform.position != prevPos)
			{
				animator.SetBool("isWalking", true);
			}
			else
			{
				animator.SetBool("isWalking", false);
			}

			prevPos = transform.position;
		}
	}

	void Look()
	{
		if (WorldManager.Instance.uiOn)
			return;

		//animator.SetInteger("arms", 0);
        //animator.SetInteger("legs", 0);

		transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);

		verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
		verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

		cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;

		Debug.DrawRay(cameraHolder.transform.position, cameraHolder.transform.forward * interactionDistance);

		if (!Input.GetMouseButtonDown(0))
			return;

		RaycastHit hit;
 
        if (Physics.Raycast (cameraHolder.transform.position, cameraHolder.transform.forward, out hit, interactionDistance))
		{
			Debug.Log("WorldPlayer Raycast object : " + hit.collider.gameObject.name);
            if (hit.collider.tag == "Clickable")
			{
				var clickable = hit.collider.GetComponent<ClickableObject>();
				if (clickable != null)
					clickable.OnClick();
			}
        }    
	}

	void Move()
	{
		Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

		moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed), ref smoothMoveVelocity, smoothTime);
		
		if (moveDir != Vector3.zero)
		{
			string s = Input.GetKey(KeyCode.LeftShift) ? "isRunning" : "isWalking";
			animator.SetBool(s, true);
		}
		else
		{
			animator.SetBool("isWalking", false);
			animator.SetBool("isRunning", false);
		}
	}

	public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
	{
		
	}

	public void SetGroundedState(bool _grounded)
	{
		grounded = _grounded;
	}

	void FixedUpdate()
	{
		if (bSit || isActing)
			return;

		if(!PV.IsMine)
			return;

		rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
	}
    
	public void SetVideoSurface(VideoSurface v)
	{
		uint uid = 0;
		if (PV.Controller.CustomProperties.TryGetValue("agoraID", out object agoraID))
		{
			Debug.Log("Agora ID : " + agoraID.ToString());
			var str = agoraID.ToString();
			uid = uint.Parse(str);
		}

		if (!PV.IsMine)
		{
			v.SetForUser(uid);
			v.SetVideoSurfaceType(AgoraVideoSurfaceType.RawImage);
			v.SetGameFps(30);
			v.SetEnable(true);
		}

		agoraUID = uid;

		Debug.Log("SetVideoSurface uid : " + uid);
	}

	public void ClearVideo()
	{
		videoSurface.SetEnable(false);
	}

	private Vector3 posBeforeSit;
	private Vector3 rotBeforeSit;
	private int chairIndex = -1;
	public void Sit(Vector3 pos, Vector3 rot, int chairIdx)
	{
		photonView.RPC("RPC_Sit", RpcTarget.All, pos, rot, chairIdx);
		
		rb.detectCollisions = false;
		rb.isKinematic = true;
	}

	[PunRPC]
	public void RPC_Sit(Vector3 pos, Vector3 rot, int chairIdx)
	{
		Debug.Log("RPC_Sit prevPos : " + transform.position + " pos : " + pos + " prevRot : " + transform.eulerAngles + " rot : " + rot);

		col.enabled = false;

		bSit = true;

		animator.SetBool("isSitting", true);

		posBeforeSit = transform.position;
		rotBeforeSit = transform.eulerAngles;
		chairIndex = chairIdx;

		transform.position = pos;
		transform.eulerAngles = rot;

		if (PV.IsMine)
			cameraHolder.transform.localEulerAngles = new Vector3(sitXRot, 0, 0);

		WorldManager.Instance.table.chairList[chairIndex].EnablePanel(false);
	}

	public void StandUp()
	{
		photonView.RPC("RPC_StandUp", RpcTarget.All);

		rb.detectCollisions = true;
		rb.isKinematic = false;
	}

	[PunRPC]
	public void RPC_StandUp()
	{
		transform.position = posBeforeSit;
		transform.eulerAngles = rotBeforeSit;

		col.enabled = true;

		bSit = false;

		animator.SetBool("isSitting", false);
		animator.SetBool("isWalking", true);

		WorldManager.Instance.table.chairList[chairIndex].EnablePanel(true);
	}

	public void SetVideoSurfaceEnable()
	{
		videoSurface.SetEnable(true);
	}

	//[SerializeField] private float waveTime;
	public void PlayWaving()
	{
		photonView.RPC("RPC_PlauWaving", RpcTarget.All, "isWaving", 2f);
	}

	public void PlayThumbUp()
	{
		photonView.RPC("RPC_PlauWaving", RpcTarget.All, "isThumbingUp", 1f);
	}

	public void PlayCalling()
	{
		photonView.RPC("RPC_PlauWaving", RpcTarget.All, "isCalling", 1f);
	}

	[PunRPC]
	public void RPC_PlauWaving(string paraName, float time)
	{
		if (isActing)
			return;
		
		StartCoroutine(PlayActionRoutine(paraName, time));
	}

	private IEnumerator PlayActionRoutine(string paraName, float time)
	{
		isActing = true;
		animator.SetBool("isWalking", false);
		animator.SetBool(paraName, true);

		yield return new WaitForSeconds(time);

		isActing = false;
		animator.SetBool(paraName, false);
	}
}