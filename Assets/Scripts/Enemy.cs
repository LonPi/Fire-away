using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    // public variables
    public float moveVelocity;
    public float hitPoints;
    public float damage;
    // private variables
    Vector2 _spriteSize;
    Vector2 _initialSpawnPosition;
    Vector2 _moveDirection;
    Transform _transform { get { return transform; } }
    Vector3 _localScale { get { return transform.localScale; } }
    bool _wasCollidingBefore;

	void Start () {
        _spriteSize = new Vector2(GetComponent<SpriteRenderer>().bounds.size.x, GetComponent<SpriteRenderer>().bounds.size.y);
        _initialSpawnPosition = transform.position;
        _moveDirection = Vector2.left;
        _wasCollidingBefore = false;
        if (_initialSpawnPosition.x < 0)
            _moveDirection = Vector2.right;
    }
	
	void Update () {
        Move();
        InflictDamage();
    }

    void Move()
    {
        Flip();
        transform.Translate(_moveDirection * moveVelocity * Time.deltaTime);
    }

    public void TakeDamage(float _damage)
    {
        hitPoints -= _damage;
        Debug.Log("Enemy: took " + _damage + " damage. HP remaining: " + hitPoints);
        if (hitPoints <= 0)
        {
            Debug.Log("Enemy dead.");
            Destroy(gameObject);
        }
    }

    void InflictDamage()
    {
        float raycastDistance, raycastRadius;
        raycastDistance = raycastRadius = _spriteSize.x / 2;
        RaycastHit2D hit = Physics2D.CircleCast(_transform.position, raycastRadius, _moveDirection, raycastDistance, 1 << LayerMask.NameToLayer("Player"));
        if (hit && !_wasCollidingBefore)
        {
            // TODO: there will be more friendly object types
            Debug.Log("Player taking damage from enemy!!!");
            Player player = hit.collider.gameObject.GetComponent<Player>();
            player.TakeDamage(damage);
            _wasCollidingBefore = true;
        }
        if (!hit && _wasCollidingBefore)
        {
            _wasCollidingBefore = false;
        }
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
