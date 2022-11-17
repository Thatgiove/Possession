using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Human : Creature
{
    [SerializeField] bool isEnemy;
    [SerializeField] string[] thoughts;
    [SerializeField] string[] inventory;
  
    void Start()
    {
        
    }
  

    // Update is called once per frame
    void Update()
    {
        
    }

    public string CreateThoughts()
    {
        var t = "";
        foreach (var _t in thoughts)
        {
            t += $"{_t}..." ;
        }
        return t;
    }   
    public string CreateInventory()
    {
        var t = "";
        foreach (var _t in inventory)
        {
            t += $"{_t}, " ;
        }
        return t;
    }

    public string[] GetInventory()
    {
        return inventory;
    }
}
