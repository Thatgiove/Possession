using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MouseLook : MonoBehaviour
{
    [SerializeField] float turnSpeed = 90f;
    [SerializeField] float headLowerAngleLimit = -80f;
    [SerializeField] float headUpperAngleLimit = 80f;

    PlayerCanvas playerCanvas;
    GameObject possessTxt;
    GameObject creatureName;

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


        if (GetComponent<Patrol>() && GetComponent<NavMeshAgent>())
        {
            GetComponent<Patrol>().enabled = false;
            GetComponent<NavMeshAgent>().enabled = false;
        }
        if (GetComponent<FlyingPatrol>())
        {
            GetComponent<FlyingPatrol>().enabled = false;
        }

        playerCanvas = FindObjectOfType<PlayerCanvas>();
        if (playerCanvas)
        {
            possessTxt = playerCanvas.possessTxt;
            creatureName = playerCanvas.creatureNameTxt;
        }
 
    }

    private void Update()
    {
        RenderName();


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

            var point = new Vector3(cam.pixelWidth / 2, cam.pixelHeight / 2, 0);
            Ray ray = cam.ScreenPointToRay(point);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 3) && hit.transform.gameObject.GetComponent<Door>())
            {
                hit.transform.gameObject.GetComponent<Door>().OpenDoor(GetComponent<Human>().GetInventory());
            }
            if (Physics.Raycast(ray, out hit, 3) && hit.transform.gameObject.GetComponent<Engine>())
            {
                hit.transform.gameObject.GetComponent<Engine>().Deactivate();
                FindObjectOfType<SceneController>().SetMaterial(hit.transform.gameObject.GetComponent<Engine>().index);
            }
        }

        //possiedi creature
        if (Input.GetMouseButtonDown(0))
        {
            var point = new Vector3(cam.pixelWidth / 2, cam.pixelHeight / 2, 0);

            Ray ray = cam.ScreenPointToRay(point);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 40))
            {
                var creature = hit.transform.gameObject;
                var head = FindObjectOfType<Head>();

                if (head && creature.GetComponent<Creature>() || creature.GetComponent<Human>())
                {


                    var eye = creature.transform.Find("eye");

                    if (GetComponent<Patrol>() && GetComponent<NavMeshAgent>())
                    {
                        GetComponent<Patrol>().enabled = true;
                        GetComponent<NavMeshAgent>().enabled = true;
                    }
                    if (GetComponent<FlyingPatrol>())
                    {
                        GetComponent<FlyingPatrol>().enabled = true;
                    }

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

    private void RenderName()
    {
        var point = new Vector3(cam.pixelWidth / 2, cam.pixelHeight / 2, 0);
        Ray ray = cam.ScreenPointToRay(point);
        RaycastHit hit;

        possessTxt?.SetActive(false);
        creatureName?.SetActive(false);
        if (Physics.Raycast(ray, out hit, 30) &&
            hit.transform.GetComponent<Creature>() &&
            creatureName &&
            possessTxt)
        {
            possessTxt.SetActive(true);
            possessTxt.GetComponent<TMP_Text>().text = "possess:";
            creatureName.SetActive(true);
            creatureName.GetComponent<TMP_Text>().text = hit.transform.gameObject.name;
        }
        if (Physics.Raycast(ray, out hit, 3) &&
            GetComponent<Human>() &&
            hit.transform.GetComponent<Door>() &&
            creatureName &&
            possessTxt)
        {
            creatureName.SetActive(true);
            creatureName.GetComponent<TMP_Text>().text = "(E) Open door";
        }        
        if (Physics.Raycast(ray, out hit, 3) &&
            GetComponent<Human>() &&
            hit.transform.GetComponent<Engine>() &&
            creatureName &&
            possessTxt)
        {
  
            creatureName.SetActive(true);
            creatureName.GetComponent<TMP_Text>().text = "(E) Shutdown Engine";
        }
    }

}