using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (PlayerController))]

public class Player : MonoBehaviour {

    float gravity = -20f;
    float moveVelocity = 5f;
    float jumpVelocity = 10f;
    PlayerController controller;
    Vector2 velocity;
    
	void Start ()
    {
        controller = GetComponent<PlayerController>();
	}

    private void Update()
    {
        velocity.x = Input.GetAxisRaw("Horizontal") * moveVelocity;

        //prevent gravity buildup and prevent from sticking top
        if (controller.collisionInfo.below || controller.collisionInfo.above)
        {
            velocity.y = 0f;
        }

        if (Input.GetKeyDown(KeyCode.Space) && controller.collisionInfo.below)
        {
            velocity.y = jumpVelocity;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

}
