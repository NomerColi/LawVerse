using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : ClickableObject
{
    public Building building;
    
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnClick()
    {
        WorldManager.Instance.OpenElevatorPopUp(this);
    }

    public override void Clear()
    {
        throw new System.NotImplementedException();
    }

    public void MoveToFloor(int floor)
    {
        Debug.Log("MoveToFloor  floor : " + floor);

        var pos = building.exitPos;
        var rot = building.rot;

        if (floor != 0)
        {
            pos = building.floorPos + new Vector3(0, building.floorHeight, 0) * floor;
            rot *= -1;
        }

        WorldManager.Instance.Teleport(pos, rot);
    }
}
