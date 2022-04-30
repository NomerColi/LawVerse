using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIParent : MonoBehaviour
{
    private Image[] images;
    private Text[] texts;

    void Start()
    {
        images = GetComponentsInChildren<Image>(true);
        texts = GetComponentsInChildren<Text>(true);
    }

    public void SetVisible(bool b)
    {
        float a = b ? 1f : 0f;

        foreach (var image in images)
        {
            var color = image.color;
            color.a = a;
            image.color = color;
        }

        foreach (var text in texts)
        {
            var color = text.color;
            color.a = a;
            text.color = color;
        }
    }
}
