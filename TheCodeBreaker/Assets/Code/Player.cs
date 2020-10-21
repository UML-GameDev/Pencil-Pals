using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Controller2D))]
public class Player : MonoBehaviour
{
    public float moveSpeed = 10;
    public float jumpVelocity = 20;
    float gravity = -20;
    Vector3 velocity;

    Controller2D controller;

    void Start()
    {
        controller = GetComponent<Controller2D>();
    }

    void Update() {

        if(controller.collisions.top || controller.collisions.bottom)
        {
            velocity.y = 0;
        }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetButton("Jump") && controller.collisions.bottom)
        {
            velocity.y += jumpVelocity;
        }

        velocity.x = input.x * moveSpeed;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
