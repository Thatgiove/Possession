using UnityEditor;
using UnityEngine;

public class EnemyVisibility : MonoBehaviour
{
    public Transform target = null;
    public float maxDistance = 10f;
    [Range(0f, 360f)]
    public float angle = 45f;
    [SerializeField] bool visualize = true;
    public bool targetVisible { get; private set; }

    void Start()
    {
    }

    void Update()
    {
        targetVisible = CheckVisibility();
        if(visualize)
        {
            //if (targetVisible)
            //{
            //    print("VISIBLE");
            //}
            //else
            //{
            //    print("NOT VISIBLE");
            //}

            //var color = targetVisible ? Color.yellow : Color.white;
            //GetComponent<Renderer>().material.color = color;
        }
    }

    bool CheckVisibility()
    {
        //calcola la direzione del target
        var directionToTarget = target.position - transform.position;

        //i gradi dell'angolo rispetto al vettore di forward
        var degreesToTarget = Vector3.Angle(transform.forward, directionToTarget);

        var withinArc = degreesToTarget < (angle / 2);

        if (!withinArc)
        {
            return false;
        }

        //calcola la distanza verso il target
        var distanceToTarget = directionToTarget.magnitude;
       
        var rayDistance = Mathf.Min(maxDistance, distanceToTarget);

        var ray = new Ray(transform.position, directionToTarget);
        RaycastHit hit;

        var canSee = false;

        if(Physics.Raycast(ray, out hit, 7))
        {
            if (hit.transform == target)
            {
                canSee = true;
                Debug.DrawLine(transform.position, hit.point, Color.red);
                FindObjectOfType<SceneController>().GameOver();
            }
            else
            {
                Debug.DrawLine(transform.position, hit.point);
            }
        }
        else
        {

            //Debug.DrawLine(transform.position, directionToTarget.normalized * rayDistance);
        }
        return canSee;

    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(EnemyVisibility))]
public class EnemyVisibilityEditor : Editor
{
    private void OnSceneGUI()
    {
        var visibility = target as EnemyVisibility;

        Handles.color = new Color(1, 1, 1, 0.1f);

        //disegnamo un arco
        var forwardPoinmtMinusHalfAngle = Quaternion.Euler(0, -visibility.angle / 2, 0) * visibility.transform.forward;

        Vector3 arcStart = forwardPoinmtMinusHalfAngle * visibility.maxDistance;

        Handles.DrawSolidArc(
            visibility.transform.position,
            Vector3.up,
            arcStart,
            visibility.angle,
            visibility.maxDistance
            );
        Handles.color = Color.red;

        Vector3 handlePosition = visibility.transform.position + visibility.transform.forward * visibility.maxDistance;

        visibility.maxDistance = Handles.ScaleValueHandle(
            visibility.maxDistance,
            handlePosition,
            visibility.transform.rotation,
            1,
            Handles.ConeHandleCap,
            0.25f
            );
    }
}
#endif