using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (PlayerController))]
public class Player : MonoBehaviour {

    public GameObject Fireball, Meteor;
    public float meleeRange;
    public float meteorSpawnHeight;
    public float hitPoints;
    public float meleeDamage;
    public float meleeCooldownTime;
    public float blinkCooldownTime;
    public float fireballCooldownTime;
    public float meteorCooldownTime;
    float gravity = -20f;
    float moveVelocity = 5f;
    float jumpVelocity = 10f;
    float blinkDistance = 3f;
    PlayerController controller;
    Vector2 _velocity;
    Vector2 _localScale { get { return transform.localScale; } }
    public Transform _transform { get { return transform; } }
    Vector2 _spriteSize;
    
	void Start ()
    {
        controller = GetComponent<PlayerController>();
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
        var _state = controller.state;
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("melee");
            Vector2 direction = _state.isMovingRight ? Vector2.right : Vector2.left;
            Vector2 raycastOrigin;
            if (direction == Vector2.left)
                raycastOrigin = new Vector2(_transform.position.x - _spriteSize.x / 2, _transform.position.y);
            else
                raycastOrigin = new Vector2(_transform.position.x + _spriteSize.x / 2, _transform.position.y);
            RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, direction, meleeRange, 1 << LayerMask.NameToLayer("Enemy"));
            Debug.DrawRay(raycastOrigin, direction * meleeRange, Color.cyan);
            if (hit)
            {
                Enemy enemy = hit.collider.gameObject.GetComponent<Enemy>();
                Debug.Log("Player: enemy has taken " + meleeDamage + " melee damage.");
                enemy.TakeDamage(meleeDamage);
            }
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
            float offset_x = _state.isMovingRight ? _spriteSize.y / 2 : -1 * _spriteSize.y/2;
            Vector2 spawnPosition = new Vector2(_transform.position.x + offset_x, _transform.position.y);
            Instantiate(Fireball,spawnPosition,Quaternion.identity,_transform);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("meteor");
            float offset_x = _state.isMovingRight ? _spriteSize.y / 2 : -1 * _spriteSize.y / 2;
            Vector2 spawnPosition = new Vector2(transform.position.x + offset_x, _transform.position.y + meteorSpawnHeight);
            Instantiate(Meteor, spawnPosition, Quaternion.identity, _transform);
        }
    }

    public void TakeDamage(float damage)
    {
        hitPoints -= damage;
        Debug.Log("Player: took " + damage + " damage. HP remaining: " + hitPoints);
        if (hitPoints <= 0)
        {
            Debug.Log("Player is dead.");
            Destroy(gameObject);
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
