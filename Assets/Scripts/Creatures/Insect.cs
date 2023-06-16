using System.Collections;
using TMPro;
using UnityEditor.PackageManager;
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
    public bool canClimb;

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
    
        cam = GetComponentInChildren<Camera>();
        
        if (cam)
        {
            var point = new Vector3(cam.pixelWidth / 2, cam.pixelHeight / 2, 0);
            Ray ray = cam.ScreenPointToRay(point);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, distanceForClimbing))
            {
                //il raycast non funziona se punto per terra
                if (hit.normal != myNormal)
                {

                    playerCanvas.canClimb = true;
                    
                    if (Input.GetButtonDown("Jump"))
                    {
                        CalulateNewNormal(hit.normal);
                    }
                }
            }
            else
            {
                playerCanvas.canClimb = false;
            }

     
            //Per il momento se sto cadendo in qualsiasi direzione dopo un tot di tempo
            //ripristino la gravità normale
            bool isColliding = Physics.Raycast(transform.position, -myNormal, out hit, 1f, ~LayerMask.NameToLayer("Ignore Raycast"));
 
            if (!isColliding)
            {
                rb.AddForce(-rb.velocity * 1.2f);
                frameCount++;
                if (frameCount >= 55 && !hasBeenCalled)
                {
                    //decelero il corpo in caduta
                    hasBeenCalled = true;
                    RestoreUpDirection();
                }
                
                //Invoke("RestoreUpDirection", .8f);
            }
        }
    }
    private int frameCount = 0;
    private bool hasBeenCalled = false;



    void RestoreUpDirection()
    {
        hasBeenCalled = false;
        frameCount = 0;

        CalulateNewNormal(Vector3.up);
    }

    void FixedUpdate()
    {
        if (rb && GetComponent<Soul>())
        {
            rb.AddForce(gravity * rb.mass * myNormal);

            var input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            input = transform.TransformDirection(input);

            rb.MovePosition(transform.position + input * Time.fixedDeltaTime * 2);
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
}
