using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElevatorPopUp : MonoBehaviour
{
    private Elevator elevator;

    public Transform buttonParent;
    public GameObject floorButtonPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Setup(Elevator ev)
    {
        Clear();

        elevator = ev;

        for (int i = 0; i < elevator.building.floors.Count; i++)
        {
            var btn = Instantiate(floorButtonPrefab).GetComponent<Button>();
            btn.transform.SetParent(buttonParent);

            int floor = i;
            btn.onClick.AddListener(() => OnFloorButtonClick(floor));
            btn.transform.GetComponentInChildren<Text>().text = floor.ToString();
        }
    }

    public void Clear()
    {
        foreach(Transform btn in buttonParent.transform)
        {
            Destroy(btn.gameObject);
        }
    }

    public void OnFloorButtonClick(int floor)
    {
        elevator.MoveToFloor(floor);
    }
}
