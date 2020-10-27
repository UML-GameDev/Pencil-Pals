using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Controller2D))]
public class Player : MonoBehaviour
{
    float maxSpeed = 10f;
    float acceleration = 20f;
    float jumpVelocity = 20f;
    float friction = 10f;
    float airFriction = 2f;
    float gravity = -40f;
    float fallModifier = 2f;
    bool shortHop = false;
    float shortHopModifier = 3f;
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

        if(velocity.y > 0 && Input.GetButtonUp("Jump"))
        {
            shortHop = true;
        } else if(velocity.y <= 0)
        {
            shortHop = false;
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

        if (velocity.y <= 0) {
            velocity.y += (gravity * Time.deltaTime) * (fallModifier);
        } else if (shortHop) {
            velocity.y += (gravity * Time.deltaTime) * (shortHopModifier);
        } else {
            velocity.y += gravity * Time.deltaTime;
        }
        controller.Move(velocity * Time.deltaTime);
    }
}
