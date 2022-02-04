using agora_gaming_rtc;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerController : MonoBehaviourPunCallbacks
{
	[SerializeField] GameObject ui;

	[SerializeField] GameObject cameraHolder;

	[SerializeField] float mouseSensitivity, sprintSpeed, walkSpeed, jumpForce, smoothTime;

	int itemIndex;
	int previousItemIndex = -1;

	float verticalLookRotation;
	bool grounded;
	Vector3 smoothMoveVelocity;
	Vector3 moveAmount;

	Rigidbody rb;

	PhotonView PV;

	const float maxHealth = 100f;
	float currentHealth = maxHealth;

	PlayerManager playerManager;

	public VideoSurface myVideoSurface;
	public VideoSurface videoSurface;

	public Animator animator;

	void Awake()
	{
		rb = GetComponent<Rigidbody>();
		PV = GetComponent<PhotonView>();

		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;

		playerManager = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManager>();
	}

	void Start()
	{
		if(PV.IsMine)
		{
			SetVideoSurface(myVideoSurface);
		}
		else
		{
			Destroy(GetComponentInChildren<Camera>().gameObject);
			Destroy(rb);
			Destroy(ui);

			SetVideoSurface(videoSurface);
		}
	}

	Vector3 prevPos = Vector3.zero;
	void Update()
	{
		if(PV.IsMine)
		{
			Look();
			Move();
			Jump();

			if(transform.position.y < -10f) // Die if you fall out of the world
			{
				Die();
			}
		}
		else 
		{
			if (transform.position != prevPos)
			{
				// int i = Input.GetKey(KeyCode.LeftShift) ? 2 : 1;
				// animator.SetInteger("arms", i);
				// animator.SetInteger("legs", i);

				animator.SetInteger("arms", 1);
				animator.SetInteger("legs", 1);
			}
			else
			{
				animator.SetInteger("arms", 5);
				animator.SetInteger("legs", 5);
			}

			prevPos = transform.position;
		}
	}

	void Look()
	{
		animator.SetInteger("arms", 0);
        animator.SetInteger("legs", 0);

		transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);

		verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
		verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

		cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
	}

	void Move()
	{
		Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

		moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed), ref smoothMoveVelocity, smoothTime);
		
		if (moveDir != Vector3.zero)
		{
			int i = Input.GetKey(KeyCode.LeftShift) ? 2 : 1;
			animator.SetInteger("arms", i);
			animator.SetInteger("legs", i);
		}
		else
		{
			animator.SetInteger("arms", 5);
        	animator.SetInteger("legs", 5);
		}
	}

	void Jump()
	{
		if(Input.GetKeyDown(KeyCode.Space) && grounded)
		{
			rb.AddForce(transform.up * jumpForce);
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
		if(!PV.IsMine)
			return;

		rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
	}

	void Die()
	{
		playerManager.Die();
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
	
		v.SetForUser(uid);
        v.SetEnable(true);
        v.SetVideoSurfaceType(AgoraVideoSurfaceType.RawImage);
        v.SetGameFps(30);

		Debug.Log("SetVideoSurface  " + (v == myVideoSurface ? "MyVideoSurface" : "VideoSurface") + " uid : " + uid);
	}

	public void ClearVideo()
	{
		if (myVideoSurface != null)
			myVideoSurface.SetEnable(false);

		videoSurface.SetEnable(false);
	}
}