using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Temp - prende denzel e lo autopossiede
        transform.parent.GetComponent<MouseLook>()?.Possess(transform.parent.gameObject);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
