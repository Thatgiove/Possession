using TMPro;
using UnityEngine;

public class PlayerCanvas : MonoBehaviour
{

    public GameObject inventoryPanel;
    public GameObject thoughtsPanel;
    public TMP_Text creatureName;

    public TMP_Text items;
    public TMP_Text thoughts;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleElements(bool hide)
    {
        if(inventoryPanel && thoughtsPanel)
        {
            inventoryPanel.SetActive(hide);
            thoughtsPanel.SetActive(hide);
        }
    }

}
