using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : MonoBehaviour
{
    public GameObject light;
    public bool active = true;
    public int index;

    public void Deactivate()
    {
        active = false;
        light.SetActive(false);
    }
}
