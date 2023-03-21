using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Human : Creature
{
    [SerializeField] bool isEnemy;
    [SerializeField] string[] thoughts;

    [SerializeField] Item[] inventory;
  
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

    public void CreateInventory()
    {
        var playerCanvas = FindObjectOfType<PlayerCanvas>();

        var itemsPanel = playerCanvas?.itemsPanel;
        var itemModel = playerCanvas?.itemModel;

        if (itemsPanel && itemModel && inventory.Length > 0)
        {
            foreach (var item in inventory)
            {
                var _item = Instantiate(itemModel);
                _item.transform.parent = itemsPanel.transform;
                _item.transform.GetChild(0).GetComponent<Image>().sprite = item.itemImage;
                _item.transform.GetChild(1).GetComponent<TMP_Text>().text = item.itemName;
            }
        }
    }
    public void DestroyInventory()
    {
        var playerCanvas = FindObjectOfType<PlayerCanvas>();

        var itemsPanel = playerCanvas?.itemsPanel;
        if (itemsPanel)
        {
            for (int i = itemsPanel.transform.childCount - 1; i >= 0; i--)
            {
                Object.Destroy(itemsPanel.transform.GetChild(i).gameObject);
            }
        }
    }

    public Item[] GetInventory()
    {
        return inventory;
    }
}
