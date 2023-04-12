using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float movementSpeed = 6;
    [SerializeField] float jumpHeight = 2;
    [SerializeField] float gravity = 50;

    Vector3 movementDirection;
    float airControl = 5;
    CharacterController characterController;
    
    Creature c;
    MouseLook ml;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        c = GetComponent<Creature>();
    }

    void Update()
    {
        if(!c || c.GetComponent<Insect>()) return;

        var input = new Vector3(Input.GetAxis("Horizontal"),  0, Input.GetAxis("Vertical"));

        input *= movementSpeed;

        if (characterController && characterController.isGrounded)
        {
            if (Input.GetButton("Jump"))
            {
            }
            else
            {
            }
        }
        else
        {
            //gli animali si muovono sul piano
            if (c.isGravityEnabled)
            {
                movementDirection.y -= gravity * Time.deltaTime;
                input.y = movementDirection.y;
                movementDirection = Vector3.Lerp(movementDirection, input, airControl * Time.deltaTime); 
            }
            //TODO FARE MEGLIO gli insetti camminano sui muri 
            else if (!c.isGravityEnabled && c.GetComponent<Insect>())
            {
                return;
            }
            //TODO creare la classe Bird?
            //gli uccelli si muovono in tutte le direzioni
            else
            {
                ml = GetComponent<MouseLook>();
                if (ml)
                {
                    // Aggiorna la rotazione locale dell'oggetto
                    c.transform.localRotation = 
                        Quaternion.Euler(0f, transform.localRotation.eulerAngles.y, 0f) * ml.head.localRotation;
                    movementDirection = transform.TransformDirection(ml.head.transform.localRotation * Vector3.forward );
                }
            }
        }

        input = transform.TransformDirection(input);
        movementDirection = input;
        characterController.Move(movementDirection * Time.deltaTime);
    }
}
