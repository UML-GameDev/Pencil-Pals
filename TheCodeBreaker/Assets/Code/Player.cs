using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Controller2D))]
public class Player : MonoBehaviour
{
    float maxSpeed = 10;
    float acceleration = 20;
    float jumpVelocity = 12;
    float friction = 10;
    float airFriction = 2;
    float gravity = -30;
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

        velocity.x += input.x * acceleration * Time.deltaTime;
        if(Mathf.Abs(velocity.x) >= maxSpeed)
        {
            velocity.x = Mathf.Sign(velocity.x) * maxSpeed;
        }

        if (controller.collisions.bottom && (input.x == 0 || input.x != Mathf.Sign(velocity.x)))
        {
            velocity.x -= Mathf.Sign(velocity.x) * Mathf.Abs(velocity.x) * friction * Time.deltaTime;
        } else if(!controller.collisions.bottom && (input.x == 0 || input.x != Mathf.Sign(velocity.x)))
        {
            velocity.x -= Mathf.Sign(velocity.x) * Mathf.Abs(velocity.x) * airFriction * Time.deltaTime;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
