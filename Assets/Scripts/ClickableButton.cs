using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ClickableButton : ClickableObject
{
    public UnityEvent action;

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
        action.Invoke();
    }

    public override void Clear()
    {
        
    }
}
