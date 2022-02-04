using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avatar : MonoBehaviour
{
	public Transform geometry;

    public int idx;

    // Start is called before the first frame update
    void Start()
    {
        geometry.GetChild(idx).gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
