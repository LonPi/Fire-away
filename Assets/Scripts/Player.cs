using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (CharacterController2D))]

public class Player : MonoBehaviour {

    float gravity = -20f;
    float moveVelocity = 5f;
    float jumpVelocity = 10f;
    CharacterController2D controller;
    Vector2 velocity;
    
	void Start ()
    {
        controller = GetComponent<CharacterController2D>();
	}

    private void Update()
    {
        velocity.x = Input.GetAxisRaw("Horizontal") * moveVelocity;

        // prevent gravity buildup and prevent from sticking top
        if (controller.state.IsCollidingBelow || controller.state.IsCollidingAbove)
        {
            velocity.y = 0f;
        }

        if (Input.GetKeyDown(KeyCode.Space) && controller.state.IsCollidingBelow)
        {
            velocity.y = jumpVelocity;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

}
