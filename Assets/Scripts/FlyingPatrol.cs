using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class FlyingPatrol : MonoBehaviour
{
    [SerializeField] Transform[] points;
    int destPoint = 0;

    float speed = 0.76f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        var velocity = speed * Time.deltaTime; 
        transform.position = Vector3.MoveTowards(transform.position, points[destPoint].position, velocity);

        Vector3 relativePos = points[destPoint].position - transform.position;

        if (relativePos != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            transform.rotation = rotation;
        }
        
        if (Vector3.Distance(transform.position, points[destPoint].position) < 0.001f)
        {
            GotoNextPoint();
        }
    }

    void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (points.Length == 0)
            return;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = (destPoint + 1) % points.Length;
    }
}
