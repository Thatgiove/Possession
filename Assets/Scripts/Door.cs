using System.Linq;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] bool canOpen;
    [SerializeField] bool isOpen;
    [SerializeField] bool isExit;
    [SerializeField] bool isElectronic;
    public string OpenObj;

    // Start is called before the first frame update
    void Start()
    {
    }


    void Update()
    {
    }

    public void OpenDoor(string[] obj)
    {
        canOpen = obj.Contains(OpenObj);
        if(!canOpen) return;

        if (!isOpen)
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
        else
        {
            transform.eulerAngles = new Vector3(0f, 90f, 0f);
        }
        isOpen = !isOpen;
    }
}
