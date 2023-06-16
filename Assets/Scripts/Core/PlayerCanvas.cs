using TMPro;
using UnityEngine;

public class PlayerCanvas : MonoBehaviour
{

    public GameObject inventoryPanel;
    public GameObject thoughtsPanel;
    public GameObject itemsPanel;
    public GameObject itemModel;

    public GameObject possessTxt;
    public GameObject creatureNameTxt;
    
    public TMP_Text creatureName;

    public TMP_Text items;
    public TMP_Text thoughts;
    public bool canClimb { get; set; }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        possessTxt.GetComponent<TMP_Text>().text = "";
        creatureNameTxt.GetComponent<TMP_Text>().text = "";
        creatureName.GetComponent<TMP_Text>().text = "";

        if (canClimb)
        {
            possessTxt.GetComponent<TMP_Text>().text = "[SPACE] Climb the wall";
        }
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
