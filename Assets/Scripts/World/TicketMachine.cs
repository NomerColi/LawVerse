using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicketMachine : MonoBehaviour
{
    private float getTicketDis = 2.5f;

    
    void Update()
    {
        var myPlayer = WorldManager.Instance.myPlayer;
        if (myPlayer == null)
            return;

        var dis = Vector3.Distance(myPlayer.transform.position, transform.position);
        if (dis <= getTicketDis && !WorldManager.Instance.myTicketUI.activeSelf)
        {
            WorldManager.Instance.getTicketUI.SetActive(true);
        }
        else
            WorldManager.Instance.getTicketUI.SetActive(false);
    }
}
