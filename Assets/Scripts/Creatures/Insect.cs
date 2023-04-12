using System.Collections;
using TMPro;
using UnityEngine;

public class Insect : Creature
{
    Rigidbody rb;
    Vector3 myNormal;
    Camera cam;
    float gravity = -6f;
    Transform head;
    public float distanceForClimbing = .002f;
    Vector3 extraVect = Vector3.zero;
    PlayerCanvas playerCanvas;

    void Start()
    {
        head = transform.Find("head");
        rb = GetComponent<Rigidbody>();
        playerCanvas = FindObjectOfType<PlayerCanvas>();

        myNormal = Vector3.up;
        
        if (rb)
        {
            rb.freezeRotation = true;
        }
    }


    void Update()
    {
        //TODO QUI NON VA BENE - TOGLIERE
        playerCanvas.possessTxt.GetComponent<TMP_Text>().text = "";
        playerCanvas.creatureNameTxt.GetComponent<TMP_Text>().text = "";
        cam = GetComponentInChildren<Camera>();

        if (cam)
        {
            var point = new Vector3(cam.pixelWidth / 2, cam.pixelHeight / 2, 0);
            Ray ray = cam.ScreenPointToRay(point);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, distanceForClimbing))
            {
                //il raycast non funziona se punto per terra
                if(hit.normal != myNormal)
                {
                    playerCanvas.possessTxt.GetComponent<TMP_Text>().text = "[SPACE] Climb the wall";
                    
                    if (Input.GetButtonDown("Jump"))
                    {
                        CalulateNewNormal(hit.normal);
                    }
                }
            }

            //Con questo Raycast vediamo su quale superficie stiamo camminando 
            //TODO - questa logica è da rivedere non entrare sempre
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
    }

    void FixedUpdate()
    {
        if (rb && GetComponent<Soul>())
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
