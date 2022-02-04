using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TeleportPoint : MonoBehaviour, IPointerClickHandler
{
    public Transform trans;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        Debug.Log("OnMosueDown");
        WorldManager.Instance.Teleport(trans.position, trans.eulerAngles);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("OnPointerClick");
        WorldManager.Instance.Teleport(trans.position, trans.eulerAngles);
    }
}
