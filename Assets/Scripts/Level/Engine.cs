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
        if (active)
        {
            active = false;
            light.SetActive(false);
            FindObjectOfType<SceneController>().ActivatelevelProgressionTxt();
        }
    }
}
