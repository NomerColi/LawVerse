using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarRotate : MonoBehaviour
{
    public GameObject avatar;

    Vector3 prevPos = Vector3.zero;
    Vector3 posDelta = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDrag()
    {
        posDelta = Input.mousePosition - prevPos;
        avatar.transform.Rotate(transform.up, -Vector3.Dot(posDelta, Camera.main.transform.right), Space.World);

        prevPos = Input.mousePosition;
    }
}
