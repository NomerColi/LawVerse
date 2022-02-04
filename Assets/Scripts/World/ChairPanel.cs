using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairPanel : ClickableObject
{
    public Chair chair;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var myPlayer = WorldManager.Instance.myPlayer;

        if (myPlayer == null)
            return;

        transform.LookAt(myPlayer.transform);

        var rot = transform.eulerAngles;
        rot.x = 0;
        transform.eulerAngles = rot;
    }


    public override void Clear()
    {
        throw new System.NotImplementedException();
    }

    public override void OnClick()
    {
        chair.OnClick();
    }
}
