using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NumberPanel : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    [SerializeField] int num = 15;
    [SerializeField] float setNumberTime = 5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetNumberAfter()
    {
        StartCoroutine(SetNumberRoutine());
    }

    IEnumerator SetNumberRoutine()
    {
        yield return new WaitForSeconds(setNumberTime);

        text.text = num.ToString();
    }
}
