using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Calendar : MonoBehaviour
{
    public List<Toggle> toggleList;
    public List<Text> dayTextList;

    [SerializeField] private int startDay, prevMaxDay, maxDay;


    // Start is called before the first frame update
    void Start()
    {
        int day = startDay;
        bool bPrevMonth = true;
        foreach (var text in dayTextList)
        {
            text.text = day.ToString();

            day++;

            if ((bPrevMonth && day > prevMaxDay) || (!bPrevMonth && day > maxDay))
            {
                bPrevMonth = false;
                day = 1;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnToggleClicked(Toggle t)
    {
        if (!t.isOn)
            return;

        for (int i = 0; i < toggleList.Count; i++)
        {
            if (toggleList[i] != t)
                toggleList[i].isOn = false;
        }
    }
}
