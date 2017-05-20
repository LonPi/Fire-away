using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    // public variables
    public GameObject Bullet;
    public float moveVelocity;
    public float attackFrequency = 1f;
    // private variables
    Player _playerRef { get { return GameManager.instance._playerRef; } }
    float attackTimer = 0f;
    Vector2 _targetPosition;
    Vector2 _spriteSize;
    Vector2 _initialSpawnPosition;
    Vector2 _moveDirection;
    Transform _transform { get { return transform; } }
    Vector3 _localScale { get { return transform.localScale; } }

	void Start () {
        
        _spriteSize = new Vector2(GetComponent<SpriteRenderer>().size.x, GetComponent<SpriteRenderer>().size.y);
        _initialSpawnPosition = transform.position;
        _moveDirection = Vector2.left;
        if (_initialSpawnPosition.x - _playerRef.transform.position.x < 0)
            _moveDirection = Vector2.right;
    }
	
	void Update () {

        Move();
    }

    void FireBullet()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackFrequency)
        {
            float offset_x = _spriteSize.y / 2;
            Vector2 spawnPosition = new Vector2(_transform.position.x + offset_x, _transform.position.y);
            Instantiate(Bullet, spawnPosition, Quaternion.identity);
            attackTimer = 0f;
        }
    }

    void Move()
    {
        Flip();
        transform.Translate(_moveDirection * moveVelocity * Time.deltaTime);
    }

    void Flip()
    {
        // sprite is facing +x direction by default
        if (_moveDirection == Vector2.right && _localScale.x < 0 || _moveDirection == Vector2.left && _localScale.x > 0)
        {
            Vector2 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }
}
