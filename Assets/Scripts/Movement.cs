using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float movementSpeed = 6;
    [SerializeField] float jumpHeight = 2;
    [SerializeField] float gravity = 20;

    Vector3 movementDirection;
    float airControl = 5;
    CharacterController characterController;
    Creature c;


    void Awake()
    {
        characterController = GetComponent<CharacterController>();

        c = GetComponent<Creature>();

    }

    void FixedUpdate()
    {
        var input = new Vector3(
                Input.GetAxis("Horizontal"),
                0,
                Input.GetAxis("Vertical"));

        input *= movementSpeed;

        input = transform.TransformDirection(input);
        movementDirection = input;

        if (characterController.isGrounded)
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

            if (c && c.isGravityEnabled)
            {
                input.y = movementDirection.y;
                movementDirection = Vector3.Lerp(movementDirection, input, airControl * Time.deltaTime);
            }
        }

        if (c && c.isGravityEnabled)
        {
            movementDirection.y -= gravity * Time.deltaTime;
        }

        characterController.Move(movementDirection * Time.deltaTime);
    }
}
