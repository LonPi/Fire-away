using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(PlayerController))]
public class Player : MonoBehaviour {

    float moveSpeed = 5f;
    float jumpSpeed = 5f;
    float gravity = -20f;

    Vector2 velocity;
    PlayerController controller;
    
    void Start()
    {
        controller = GetComponent<PlayerController>();    
    }
    void Update()
    {
        velocity.x = Input.GetAxis("Horizontal") * moveSpeed;

        if (controller.collisionInfo.above || controller.collisionInfo.below)
        {
            velocity.y = 0f;
        }
        if (Input.GetKeyDown(KeyCode.Space) && controller.collisionInfo.below)
        {
            velocity.y = jumpSpeed;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
