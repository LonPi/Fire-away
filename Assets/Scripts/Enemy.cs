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
    Transform _transform { get { return transform; } }
    Vector3 _localScale { get { return transform.localScale; } }
    Vector2 _targetPosition;
    Vector2 _moveDirection;
    float lastRaycastTime;

	void Start () {
        _spriteSize = new Vector2(GetComponent<SpriteRenderer>().bounds.size.x, GetComponent<SpriteRenderer>().bounds.size.y);
        _initialSpawnPosition = transform.position;
        _targetPosition= GameManager.instance._treeRef.transform.position;
        _moveDirection = (_targetPosition.x - transform.position.x > 0 ? Vector2.right : Vector2.left);
    }
	
	void Update () {
        Move();
        if (Time.time - lastRaycastTime > 0.5f)
            InflictDamage();
    }

    void Move()
    {
        Flip();
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(_targetPosition.x, transform.position.y), moveVelocity * Time.deltaTime);
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
        RaycastHit2D hit = Physics2D.CircleCast(_transform.position, raycastRadius, _moveDirection, raycastDistance, 1 << LayerMask.NameToLayer("Friendly"));
        lastRaycastTime = Time.time;
        if (hit)
        {
            Player player = hit.collider.gameObject.GetComponent<Player>();
            Tree tree = hit.collider.gameObject.GetComponent<Tree>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }

            if (tree != null)
            {
                tree.TakeDamage(damage);
            }
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
