﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController),typeof(SpellManager))]
public class Player : MonoBehaviour {

    public float hitPoints;
    public PlayerController Controller { get; private set; }
    public Transform _transform { get { return transform; } private set { } }
    public Vector2 spriteSize { get; private set; }
    [SerializeField]
    float 
        gravity, 
        moveVelocity, 
        jumpVelocity;
    SpellManager spellManager;
    Vector2 _velocity;
    Vector2 _localScale { get { return transform.localScale; } }
    bool _isDead;
    
	void Start ()
    {
        Controller = GetComponent<PlayerController>();
        spellManager = GetComponent<SpellManager>();
        spriteSize = new Vector2(GetComponent<SpriteRenderer>().bounds.size.x, GetComponent<SpriteRenderer>().bounds.size.y);
        _isDead = false;
	}

    private void Update()
    {
        HandleMovement();
        HandleSpells();
        if (hitPoints <= 0 && !_isDead)
        {
            GameManager.instance.GameOver();
            _isDead = true;
        }
    }

    void HandleMovement()
    {
        Flip();
        var _collisionInfo = Controller.collisionInfo;
        
        //prevent gravity buildup and prevent from sticking top
        if (Controller.collisionInfo.below || _collisionInfo.above)
        {
            _velocity.y = 0f;
        }

        if (Input.GetKeyDown(KeyCode.Space) && _collisionInfo.below)
        {
            _velocity.y = jumpVelocity;
        }

        _velocity.x = Input.GetAxisRaw("Horizontal") * moveVelocity;
        _velocity.y += gravity * Time.deltaTime;
        Controller.Move(_velocity * Time.deltaTime);
    }

    void HandleSpells()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            spellManager.meleeSpell.Cast(this);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            bool pressLeft = Input.GetKey(KeyCode.LeftArrow);
            bool pressRight = Input.GetKey(KeyCode.RightArrow);

            // check if the right combo is pressed
            bool canBlink = pressLeft & !pressRight | !pressLeft & pressRight;

            if (canBlink)
            {
                Debug.Log("Can blink to " + (pressLeft ? "left" : "right"));
                Vector2 direction = pressLeft ? Vector2.left : Vector2.right;
                spellManager.blinkSpell.Cast(this, direction);

            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            spellManager.fireballSpell.Cast(this);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            spellManager.meteorSpell.Cast(this);
            
        }
    }

    public void TakeDamage(float damage)
    {
        hitPoints -= damage;
        Debug.LogError("Player: took " + damage + " damage. HP remaining: " + hitPoints);
        if (hitPoints <= 0)
        {
            Debug.Log("Player is dead.");
        }
    }

    void Flip()
    {
        var _state = Controller.state;
        Vector2 _localScale = transform.localScale;
        // sprite is facing -x direction by default
        if (_state.isMovingRight && _localScale.x > 0 || !_state.isMovingRight && _localScale.x < 0)
        {
            _localScale.x *= -1;
            transform.localScale = _localScale;
        }
    }
}