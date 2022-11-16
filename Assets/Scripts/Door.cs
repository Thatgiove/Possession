using System.Linq;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] bool canOpen;
    [SerializeField] bool isCell;
    [SerializeField] bool isOpen;
    [SerializeField] bool unlocked;
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
        canOpen = obj.Contains(OpenObj) || (isExit && AllEnginesAreShutdown());

        if (canOpen)
            unlocked = true;


        if (unlocked)
        {
            var _Y = transform.eulerAngles.y;
            if (_Y == 90 || _Y == -90)
            {
                _Y = 0;
            }
            else
            {
                _Y = 90;
            }
            if (!isOpen)
            {
                transform.eulerAngles = new Vector3(0f, _Y, 0f);
            }
            else if (isOpen)
            {
                transform.eulerAngles = new Vector3(0f, _Y, 0f);
            }

            isOpen = !isOpen;
        }
        
     
    }



    bool AllEnginesAreShutdown()
    {
        foreach (var en in FindObjectsOfType<Engine>())
        {
            if (en.active) return false;
        };

        return true;
    }
}
