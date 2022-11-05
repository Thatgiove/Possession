using UnityEngine;

public class Creature : MonoBehaviour, IControllable
{
    public string creatureName;
    public bool isGravityEnabled;
    public bool canPossess;
    [SerializeField] float velocity;
    [SerializeField] float turnSpeed;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
