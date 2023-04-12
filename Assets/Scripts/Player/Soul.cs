using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Questa classe si occupa di gestire la possessione 
/// delle creature e viene trasferita col metodo EnterInCreatureBody
/// </summary>
public class Soul : MonoBehaviour
{
    Camera cam;
    Head head;

    void Start()
    {
        head = FindObjectOfType<Head>();
       
        cam = head.GetComponent<Camera>();

        DisableAI(false);
    }

    void Update()
    {
        //possiedi creature
        if (Input.GetMouseButtonDown(0))
        {
            var point = new Vector3(cam.pixelWidth / 2, cam.pixelHeight / 2, 0);

            Ray ray = cam.ScreenPointToRay(point);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 40))
            {
                var creature = hit.transform.gameObject;
                
                if (creature.GetComponent<Creature>())
                {
                    EnterInCreatureBody(creature);
                }
            }
        }
    }

    public void EnterInCreatureBody(GameObject creature)
    {
        if (!head) return;

        var eye = creature.transform.Find("eye");

        DisableAI(true);

        head.transform.parent = creature.transform;

        if (eye)
        {
            head.transform.localPosition = eye.transform.localPosition;
            head.transform.localRotation = eye.transform.localRotation;
        }
        else
        {
            head.transform.localPosition = Vector3.zero;
        }

        head.transform.localRotation = Quaternion.identity;

        Destroy(GetComponent<Movement>());
        Destroy(GetComponent<MouseLook>());
        Destroy(this);

        creature.AddComponent<Movement>();
        creature.AddComponent<MouseLook>();
        creature.AddComponent<Soul>();

        var playerCanvas = FindObjectOfType<PlayerCanvas>();
        if (playerCanvas)
        {
            playerCanvas.creatureName.text = creature.GetComponent<Creature>().creatureName;
            GetComponent<Human>()?.DestroyInventory();
            if (creature.GetComponent<Human>())
            {
                playerCanvas.ToggleElements(true);

                playerCanvas.thoughts.text = creature.GetComponent<Human>()?.CreateThoughts();


                creature.GetComponent<Human>()?.CreateInventory();
                playerCanvas.items.color = Color.green;
            }
            else
            {
                playerCanvas.ToggleElements(false);
            }

        }
    }

    void DisableAI(bool isDisabled)
    {
        //false quando prendo il controllo, true quando lo perdo 
        if (GetComponent<Patrol>() && GetComponent<NavMeshAgent>())
        {
            GetComponent<Patrol>().enabled = isDisabled;
            GetComponent<NavMeshAgent>().enabled = isDisabled;
        }
        if (GetComponent<FlyingPatrol>())
        {
            GetComponent<FlyingPatrol>().enabled = isDisabled;
        }
        if (GetComponent<EnemyVisibility>())
        {
            GetComponent<EnemyVisibility>().enabled = isDisabled;

            if (isDisabled)
            {
                FindObjectOfType<SceneController>().EnableEnemyVisibilityAfter3Seconds(gameObject);
            }
        }
        if (GetComponent<Light>())
        {
            GetComponent<Light>().enabled = isDisabled;
        }
    }
}
