using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //TODO temp rende denzel e lo autopossiede
        transform.parent.GetComponent<Soul>()?.EnterInCreatureBody(transform.parent.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
