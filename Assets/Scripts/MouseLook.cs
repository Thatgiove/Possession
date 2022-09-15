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

    Transform head;
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


        if (Input.GetMouseButtonDown(0))
        {
            var point = new Vector3(
                cam.pixelWidth / 2,
                cam.pixelHeight / 2,
                0);
            Ray ray = cam.ScreenPointToRay(point);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit, 5))
            {
                var creature = hit.transform.gameObject;
                var head = FindObjectOfType<Head>();

                if (creature.GetComponent<Creature>() && head)
                {

                    head.transform.parent = creature.transform;
                    head.transform.localPosition = Vector3.zero;
                    head.transform.localRotation = Quaternion.identity;

                    //Destroy(GetComponent<CharacterController>());
                    Destroy(GetComponent<Movement>());
                    Destroy(GetComponent<MouseLook>());

                    //var cc = creature.AddComponent<CharacterController>();
 

                    creature.AddComponent<Movement>();
                    creature.AddComponent<MouseLook>();

                    print("POSSESS: " + hit.transform.gameObject.name);
                }
            }
        }
    }
}