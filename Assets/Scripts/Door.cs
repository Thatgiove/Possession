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
    public Item GetItemNeeded()
    {
        return itemNeeded;
    }

    // Start is called before the first frame update
    void Start()
    {
    }



    void Update()
    {
    }

    public void OpenDoor(Item[] obj)
    {
        canOpen = obj.Contains(itemNeeded) || (isExit && FindObjectOfType<SceneController>().AllEnginesAreShutdown());

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
