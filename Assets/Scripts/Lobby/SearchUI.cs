using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SearchUI : MonoBehaviour
{
    public List<Toggle> typeToggleList;
    public List<Toggle> regionToggleList;
    public List<Toggle> genderToggleList;


    public void OnTypeToggle(int idx)
    {
        if (!typeToggleList[idx].isOn)
            return;

        for (int i = 0; i < typeToggleList.Count; i++)
        {
            if (i != idx)
            typeToggleList[i].isOn = false;
        }
    }

    public void OnRegionToggle(int idx)
    {
        if (!regionToggleList[idx].isOn)
            return;

        for (int i = 0; i < regionToggleList.Count; i++)
        {
            if (i != idx)
            regionToggleList[i].isOn = false;
        }
    }

    public void OnGenderToggle(int idx)
    {
        if (!genderToggleList[idx].isOn)
            return;

        for (int i = 0; i < genderToggleList.Count; i++)
        {
            if (i != idx)
            genderToggleList[i].isOn = false;
        }
    }
}
