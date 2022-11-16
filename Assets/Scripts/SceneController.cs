using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    public GameObject[] switches;
    public Material greenMaterial;

    public bool isGameOver;

    void Start()
    {
        
    }

    public void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0;
    }

    public void SetMaterial(int index)
    {
        switches[index].GetComponent<Renderer>().material = greenMaterial;
    }

    void Update()
    {
        
    }
}
