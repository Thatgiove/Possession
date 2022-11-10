using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField] float turnSpeed = 90f;
    [SerializeField] float headLowerAngleLimit = -80f;
    [SerializeField] float headUpperAngleLimit = 80f;

    float yaw = 0f;
    float pitch = 0f;
    Quaternion bodyStartOrientation;
    Quaternion headStartOrientation;

    public Transform head;
    Camera cam;

    void Start()
    {
        cam = GetComponentInChildren<Camera>();

        head = cam.transform;
        bodyStartOrientation = transform.localRotation;
        headStartOrientation = head.transform.localRotation;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        var hor = Input.GetAxis("Mouse X") * Time.deltaTime * turnSpeed;
        var vert = Input.GetAxis("Mouse Y") * Time.deltaTime * turnSpeed;

        yaw += hor;
        pitch += vert;

        pitch = Mathf.Clamp(pitch, headLowerAngleLimit, headUpperAngleLimit);

        var bodyRot = Quaternion.AngleAxis(yaw, Vector3.up);
        var headRot = Quaternion.AngleAxis(pitch * -1, Vector3.right);

        transform.localRotation = bodyRot * bodyStartOrientation;
        head.localRotation = headRot * headStartOrientation;

        //Apri la porte e azioni
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!GetComponent<Human>()) return;
            
            var point = new Vector3(cam.pixelWidth / 2,cam.pixelHeight / 2, 0);
            Ray ray = cam.ScreenPointToRay(point);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 30) && hit.transform.gameObject.GetComponent<Door>())
            {
                hit.transform.gameObject.GetComponent<Door>().OpenDoor(GetComponent<Human>().GetInventory());
            }
            if (Physics.Raycast(ray, out hit, 30) && hit.transform.gameObject.GetComponent<Engine>())
            {
                hit.transform.gameObject.GetComponent<Engine>().Deactivate();
            }
        }

        //possiedi creature
        if (Input.GetMouseButtonDown(0))
        {
            var point = new Vector3( cam.pixelWidth / 2, cam.pixelHeight / 2, 0);

            Ray ray = cam.ScreenPointToRay(point);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit, 30))
            {
                var creature = hit.transform.gameObject;
                var head = FindObjectOfType<Head>();

                if (head && creature.GetComponent<Creature>() || creature.GetComponent<Human>())
                {
                    var playerCanvas = FindObjectOfType<PlayerCanvas>();
                    var eye = creature.transform.Find("eye");

                    head.transform.parent = creature.transform;
                    if (eye)
                    {
                        head.transform.localPosition = eye.transform.localPosition;
                    }
                    else
                    {
                        head.transform.localPosition = Vector3.zero;
                    }

                    
                    head.transform.localRotation = Quaternion.identity;

                    Destroy(GetComponent<Movement>());
                    Destroy(GetComponent<MouseLook>());

 
                    creature.AddComponent<Movement>();
                    creature.AddComponent<MouseLook>();

                    //print("POSSESS: " + hit.transform.gameObject.name);

                    if (playerCanvas)
                    {
                        playerCanvas.creatureName.text = creature.GetComponent<Creature>().creatureName;

                        if (creature.GetComponent<Human>())
                        {
                            playerCanvas.ToggleElements(true);
                            
                            playerCanvas.thoughts.text = creature.GetComponent<Human>()?.CreateThoughts();
                            playerCanvas.items.text = creature.GetComponent<Human>()?.CreateInventory();
                            playerCanvas.items.color = Color.green;
                        }
                        else
                        {
                            playerCanvas.ToggleElements(false);
                        }
                        
                    }
                }


            }
        }
    }
}