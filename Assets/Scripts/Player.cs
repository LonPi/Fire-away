using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (PlayerController))]

public class Player : MonoBehaviour {

    float gravity = -20f;
    float moveVelocity = 5f;
    float jumpVelocity = 10f;
    float blinkDistance = 3f;
    PlayerController controller;
    Vector2 velocity;
    
	void Start ()
    {
        controller = GetComponent<PlayerController>();
	}

    private void Update()
    {
        //HandleMovement();
        HandleSpells();
    }

    void HandleMovement()
    {
        Flip();
        var _collisionInfo = controller.collisionInfo;
        
        //prevent gravity buildup and prevent from sticking top
        if (controller.collisionInfo.below || _collisionInfo.above)
        {
            velocity.y = 0f;
        }

        if (Input.GetKeyDown(KeyCode.Space) && _collisionInfo.below)
        {
            velocity.y = jumpVelocity;
        }

        velocity.x = Input.GetAxisRaw("Horizontal") * moveVelocity;
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleSpells()
    {
        // TODO: play animation for each spells
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("melee");
            // check enemy in melee range
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("blink");
            bool pressLeft = Input.GetKey(KeyCode.LeftArrow);
            bool pressRight = Input.GetKey(KeyCode.RightArrow);

            // check if the right combo is pressed
            bool canBlink = pressLeft & !pressRight | !pressLeft & pressRight;
            
            if (canBlink)
            {
                Debug.Log("Can blink to " + (pressLeft ? "left" : "right"));
                Vector2 direction = pressLeft ? Vector2.left : Vector2.right;
                controller.Blink(blinkDistance * direction, direction);
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("fireball");
            // spawn fireball sprite and check collision for that sprite
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("meteor");
            // same as fireball
        }
    }

    void Flip()
    {
        var _state = controller.state;
        Vector2 _localScale = transform.localScale;
        // sprite is facing -x direction by default
        if (_state.isMovingRight && _localScale.x > 0 || !_state.isMovingRight && _localScale.x < 0)
        {
            _localScale.x *= -1;
            transform.localScale = _localScale;
        }
    }

}
