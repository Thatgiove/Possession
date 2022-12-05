using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] bool canOpen;
    [SerializeField] bool isCell;
    [SerializeField] bool isOpen;
    public bool unlocked { get; set; }
    [SerializeField] bool isExit;
    [SerializeField] bool isElectronic;
    [SerializeField] Item itemNeeded;

    [SerializeField] string[] openConditions;

    SceneController sceneController;

    public IDictionary<string, string> conditionsDescription = new Dictionary<string, string>();
    string whyIsnotOpen;

    public string GetWhyIsnotOpen()
    {
        return whyIsnotOpen;
    }

    // Start is called before the first frame update
    void Start()
    {
        sceneController = FindObjectOfType<SceneController>();
        conditionsDescription.Add("Engine", "Closed: I need to shutdown all engines first");
    }



    void Update()
    {
    }
    bool CheckConditions()
    {
        whyIsnotOpen = "";
        //se ci sono condizioni, devono essere tutte soddisfatte
        if (openConditions.Length > 0)
        {
            foreach (var condition in openConditions)
            {
                if (sceneController.conditions.ContainsKey(condition) &&
                    !sceneController.conditions[condition].Invoke())
                {
                    whyIsnotOpen += conditionsDescription[condition] + " ";
                    return false;
                }
                    
            }
        }
        return true;
    }
    bool CheckObject(Item[] obj)
    {
        whyIsnotOpen = "Closed: I need a " + itemNeeded?.itemName;
        return obj.Contains(itemNeeded);
    }
    public void OpenDoor(Item[] obj)
    {
        canOpen = (itemNeeded == null || CheckObject(obj)) && CheckConditions();

        if (canOpen)
            unlocked = true;

        if (unlocked)
        {
            //var _Y = transform.eulerAngles.y;
            //if (_Y == 90 || _Y == -90)
            //{
            //    _Y = 0;
            //}
            //else
            //{
            //    _Y = 90;
            //}
            //if (!isOpen)
            //{
            //    transform.eulerAngles = new Vector3(0f, _Y, 0f);
            //}
            //else if (isOpen)
            //{
            //    transform.eulerAngles = new Vector3(0f, _Y, 0f);
            //}
            //isOpen = !isOpen;
            gameObject.SetActive(false);
        }
        
     
    }
}
