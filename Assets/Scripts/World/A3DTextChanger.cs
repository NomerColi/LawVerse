using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using MText;

public class A3DTextChanger : MonoBehaviour
{
    public List<Modular3DText> textList;

    [TestMethod]
    public void ChangeText()
    {
        foreach (var text in textList)
        {
            text.Text = Random.Range(1000, 9999).ToString() + " " + Random.Range(1000, 9999).ToString();
        }
    }
}
