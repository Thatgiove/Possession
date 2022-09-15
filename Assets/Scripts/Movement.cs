using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    [SerializeField] float movementSpeed = 6;
    [SerializeField] float jumpHeight = 2;
    [SerializeField] float gravity = 20;
    float airControl = 5;

    Vector3 movementDirection;

    CharacterController characterController;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        characterController.stepOffset = 0;
        characterController.radius = transform.localScale.x * 1.5f;
    }

    void FixedUpdate()
    {
        var input = new Vector3(
                Input.GetAxis("Horizontal"),
                0,
                Input.GetAxis("Vertical"));

        input *= movementSpeed;

        input = transform.TransformDirection(input);

        if (characterController.isGrounded)
        {
            movementDirection = input;

            if (Input.GetButton("Jump"))
            {

            }
            else
            {

            }

            
        }
        else
        {
            input.y = movementDirection.y;
            movementDirection = Vector3.Lerp(movementDirection, input, airControl * Time.deltaTime);
        }

        movementDirection.y -= gravity * Time.deltaTime;
        characterController.Move(movementDirection * Time.deltaTime);
    }
}
