using System.Collections;
using TMPro;
using UnityEngine;
/// <summary>
/// MouseLook si occupa della rotazione del corpo
/// e della testa. Per il momento gestisce anche le
/// azioni delle creature
/// </summary>
public class MouseLook : MonoBehaviour
{
    [SerializeField] float turnSpeed = 250f;
    [SerializeField] float headLowerAngleLimit = -80f;
    [SerializeField] float headUpperAngleLimit = 80f;

    PlayerCanvas playerCanvas;

    float tiltAngle = 0;
    public Transform head;
    Camera cam;
    bool showText = true;

    void Start()
    {
        cam = GetComponentInChildren<Camera>();

        head = cam.transform;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerCanvas = FindObjectOfType<PlayerCanvas>();
    }

    private void Update()
    {
        var mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * 250;

        transform.RotateAround(transform.position, transform.up, mouseX);

        if (head)
        {
            tiltAngle += Input.GetAxis("Mouse Y") * -1;
            tiltAngle = Mathf.Clamp(tiltAngle, headLowerAngleLimit, headUpperAngleLimit);
            head.transform.localRotation = Quaternion.Euler(tiltAngle, 0, 0);
        }

        //TODO spostare
        //Apri la porte e azioni
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!GetComponent<Human>()) return;

            var point = new Vector3(cam.pixelWidth / 2, cam.pixelHeight / 2, 0);
            Ray ray = cam.ScreenPointToRay(point);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 3) && hit.transform.gameObject.GetComponent<Door>())
            {
                var door = hit.transform.gameObject.GetComponent<Door>();
                door?.OpenDoor(GetComponent<Human>().GetInventory());
                if (!door.unlocked)
                {
                    showText = false;
                    StartCoroutine(ReactivateText());
                }
            }
            if (Physics.Raycast(ray, out hit, 3) && hit.transform.gameObject.GetComponent<Engine>())
            {
                hit.transform.gameObject.GetComponent<Engine>().Deactivate();
                FindObjectOfType<SceneController>().SetMaterial(hit.transform.gameObject.GetComponent<Engine>().index);
            }
        }
        RenderName();
    }

    //TODO - questa logica va spostata nel PlayerCanvas - rivedere
    private void RenderName()
    {
        var point = new Vector3(cam.pixelWidth / 2, cam.pixelHeight / 2, 0);
        Ray ray = cam.ScreenPointToRay(point);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 30) &&
            hit.transform.GetComponent<Creature>())
        {
            playerCanvas.possessTxt.GetComponent<TMP_Text>().text = "possess:";
            playerCanvas.creatureNameTxt.GetComponent<TMP_Text>().text = hit.transform.gameObject.name;
        }

        else if (Physics.Raycast(ray, out hit, 3) &&
            GetComponent<Human>() &&
            hit.transform.GetComponent<Door>())
        {
            if (showText)
            {
                playerCanvas.creatureName.GetComponent<TMP_Text>().text = "(E) Open door";
            }
            else
            {
                playerCanvas.creatureName.GetComponent<TMP_Text>().text = hit.transform.GetComponent<Door>()?.GetWhyIsnotOpen();
            }
        }

        else if (Physics.Raycast(ray, out hit, 3) &&
            GetComponent<Human>() &&
            hit.transform.GetComponent<Engine>())
        {
            playerCanvas.creatureName.GetComponent<TMP_Text>().text = "(E) Shutdown Engine";
        }


    }


    IEnumerator ReactivateText()
    {
        yield return new WaitForSeconds(2);
        showText = true;
    }
}