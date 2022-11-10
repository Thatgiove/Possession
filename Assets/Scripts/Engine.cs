using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : MonoBehaviour
{
    public GameObject light;
    public bool active = true;

    public void Deactivate()
    {
        active = false;
        light.SetActive(false);
    }
}
