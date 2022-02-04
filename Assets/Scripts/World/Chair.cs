using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Chair : ClickableObject
{
    public GameObject panel;
    public Vector3 sitPos;
    public Vector3 sitRot;

    PhotonView pv;

    private float sitDis = 2.5f;


    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        var myPlayer = WorldManager.Instance.myPlayer;
        if (myPlayer == null || myPlayer.bSit)
            return;

        var dis = Vector3.Distance(myPlayer.transform.position, transform.position);
        if (dis <= sitDis && !myPlayer.bSit)
        {
            WorldManager.Instance.SetNearChair(this);
        }
        else
            WorldManager.Instance.ClearNearChair(this);
    }

    public override void Clear()
    {
        throw new System.NotImplementedException();
    }

    public override void OnClick()
    {
        Debug.Log("Chair OnClick");

        WorldManager.Instance.Sit(this);
    }

    public void EnablePanel(bool b)
    {
        pv.RPC("RPC_EnablePanel", RpcTarget.All, b);
    }

    [PunRPC]
    public void RPC_EnablePanel(bool b)
    {
        //panel.SetActive(b);
    }
}
