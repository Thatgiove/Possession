using System.Collections;
using TMPro;
using UnityEngine;

public class Insect : MonoBehaviour
{
    Rigidbody rb;
    Vector3 myNormal;
    Camera cam;
    float gravity = -6f;
    Transform head;
    float tiltAngle = 0;
    public float minX = -80f;
    public float maxX = 80.0f;
    Vector3 extraVect = Vector3.zero;
    PlayerCanvas playerCanvas;
    
    void Start()
    {
        head = transform.Find("head");
        rb = GetComponent<Rigidbody>();

        myNormal = transform.up;
        if (rb)
            rb.freezeRotation = true;

        playerCanvas = FindObjectOfType<PlayerCanvas>();
    }


    void Update()
    {
        playerCanvas.creatureNameTxt.GetComponent<TMP_Text>().text = "";
        playerCanvas.possessTxt.GetComponent<TMP_Text>().text = "";

        cam = GetComponentInChildren<Camera>();
        if (cam)
        {
            var point = new Vector3(cam.pixelWidth / 2, cam.pixelHeight / 2, 0);
            Ray ray = cam.ScreenPointToRay(point);
            RaycastHit hit;

            /*TODO parametrizzare la distanza*/
            if (Physics.Raycast(ray, out hit, 1))
            {
                var wall = hit.transform;
                
                if (wall.tag == "Wall" || wall.tag == "Ceiling")
                    playerCanvas.possessTxt.GetComponent<TMP_Text>().text = "[SPACE] Climb the wall";
    
          
                if (Input.GetButtonDown("Jump"))
                {
                    //TODO - trovare il calcolo per adesso il normale del soffitto è l'asse delle Y (up)
                    //mentre quello del muro e l'asse Z (forward)
                    if (wall.tag == "Ceiling")
                    {
                        extraVect = Vector3.zero;
                        CalulateNewNormal(Vector3.Cross(wall.right, wall.forward));
                    }
                    else if (wall.tag == "Wall")
                    {
                        extraVect = Vector3.zero;
                        CalulateNewNormal(Vector3.Cross(wall.right, wall.up));
                    }
                }
            }

            //Con questo Raycast vediamo su quale superficie stiamo camminando 
            //TODO - non entrare sempre
            if (Physics.Raycast(transform.position, -myNormal, out hit, 1))
            {
                //Quando entriamo nelle tubature la gravità ritorna normale
                if(hit.transform.tag == "Pipe")
                {
                    extraVect = head.transform.forward;
                    StartCoroutine(CalulateNewNormalAfterTime(Vector3.up));
                }
            }

           
        }
       

        var mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * 250;

        //TODO L'input è globale,gestirlo bene
        transform.RotateAround(transform.position, transform.up, mouseX);

        if (head)
        {
            tiltAngle += Input.GetAxis("Mouse Y") * -1;
            tiltAngle = Mathf.Clamp(tiltAngle, minX, maxX);
            head.transform.localRotation = Quaternion.Euler(tiltAngle, 0, 0);
        }
    }

    void FixedUpdate()
    {
        if (rb)
        {
            rb.AddForce(gravity * rb.mass * myNormal);

            var input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            input = transform.TransformDirection(input);
            rb.MovePosition(transform.position + input * Time.deltaTime * 2);
        }
    }

    void CalulateNewNormal(Vector3 wallNormal)
    {
        myNormal = wallNormal;
        var myForward = Vector3.Cross(transform.right, myNormal);
        var dstRot = Quaternion.LookRotation(myForward + extraVect, myNormal);
        transform.rotation = dstRot;
    }
    void JumpToWall(Vector3 point, Vector3 normal)
    {

        rb.isKinematic = true; 
        var orgPos = transform.position;
        var orgRot = transform.rotation;
        var dstPos = point + normal;

        var myForward = Vector3.Cross(transform.right, normal);

        var dstRot = Quaternion.LookRotation(myForward, normal);

        //for (float t = 0.0f; t < 1.0; ){
        //    t += Time.deltaTime;
        //    transform.position = Vector3.Lerp(orgPos, dstPos, t);
        //    transform.rotation = Quaternion.Slerp(orgRot, dstRot, t);
        //    yield return; // return here next frame
        //}

        myNormal = normal; // update myNormal
        rb.isKinematic = false; // enable physics
    }

    IEnumerator CalulateNewNormalAfterTime(Vector3 newNormal)
    {
        yield return new WaitForSeconds(.5f);
        CalulateNewNormal(newNormal);

    }
}
