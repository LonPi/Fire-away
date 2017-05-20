using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (PlayerController))]
public class Player : MonoBehaviour {

    public GameObject Fireball;
    float gravity = -20f;
    float moveVelocity = 5f;
    float jumpVelocity = 10f;
    float blinkDistance = 3f;
    PlayerController controller;
    Vector2 _velocity;
    Vector2 _localScale;
    Transform _transform;
    Vector2 _spriteSize;
    
	void Start ()
    {
        controller = GetComponent<PlayerController>();
        _localScale = transform.localScale;
        _transform = transform;
        _spriteSize = new Vector2(GetComponent<SpriteRenderer>().bounds.size.x, GetComponent<SpriteRenderer>().bounds.size.y);
	}

    private void Update()
    {
        HandleMovement();
        HandleSpells();
    }

    void HandleMovement()
    {
        Flip();
        var _collisionInfo = controller.collisionInfo;
        
        //prevent gravity buildup and prevent from sticking top
        if (controller.collisionInfo.below || _collisionInfo.above)
        {
            _velocity.y = 0f;
        }

        if (Input.GetKeyDown(KeyCode.Space) && _collisionInfo.below)
        {
            _velocity.y = jumpVelocity;
        }

        _velocity.x = Input.GetAxisRaw("Horizontal") * moveVelocity;
        _velocity.y += gravity * Time.deltaTime;
        controller.Move(_velocity * Time.deltaTime);
    }

    void HandleSpells()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("melee");
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
            var _state = controller.state;
            float offset_x = _state.isMovingRight ? _spriteSize.y / 2 : -1 * _spriteSize.y/2;
            Vector2 spawnPosition = new Vector2(_transform.position.x + offset_x, _transform.position.y);
            Instantiate(Fireball,spawnPosition,Quaternion.identity,_transform);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("meteor");
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
