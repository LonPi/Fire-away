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
    Transform _transform { get { return transform; } }
    Vector3 _localScale { get { return transform.localScale; } }
    Vector2 _targetPosition;
    Vector2 _moveDirection;
    float lastAttackTime;
    float attackInterval = 0.5f;
    bool _isDead;
    Animator animator;
    bool wander;
    bool hasArrivedTargetDestination;

	void Start () {
        animator = GetComponent<Animator>();
        _spriteSize = new Vector2(GetComponent<SpriteRenderer>().bounds.size.x, GetComponent<SpriteRenderer>().bounds.size.y);
        _targetPosition= GameManager.instance._treeRef.transform.position;
        _moveDirection = (_targetPosition.x - transform.position.x > 0 ? Vector2.right : Vector2.left);
        _isDead = false;
        wander = false;
        hasArrivedTargetDestination = false;
    }
	
	void Update () {
        if (_isDead)
            return;

        Move();

        if ((Time.time - lastAttackTime >= attackInterval))
            InflictDamage();
        
    }

    void Move()
    {
        Flip();
        if (transform.position.x == _targetPosition.x && _targetPosition.x == GameManager.instance._treeRef.transform.position.x)
        {
            float randomDirX = Random.Range(-1.99f, 1.99f);
            float randomDistX = Random.Range(1.2f, 2.2f);
            _targetPosition = new Vector2(transform.position.x + (randomDirX * randomDistX), transform.position.y);
        }
        else if (transform.position.x == _targetPosition.x && _targetPosition.x != GameManager.instance._treeRef.transform.position.x)
            _targetPosition = new Vector2(GameManager.instance._treeRef.transform.position.x, transform.position.y);
        _moveDirection = (_targetPosition.x - transform.position.x > 0 ? Vector2.right : Vector2.left);
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(_targetPosition.x, transform.position.y), moveVelocity * Time.deltaTime);
    }

    public void TakeDamage(float _damage)
    {
        hitPoints -= _damage;
        if (hitPoints <= 0 && !_isDead)
        {
            _isDead = true;
            animator.SetTrigger("dead");
            GameManager.instance._playerRef.IncrementKillCount();
            Destroy(gameObject,2);
        }
    }

    void InflictDamage()
    {
        float raycastDistance, raycastRadius;
        raycastDistance = raycastRadius = _spriteSize.x / 2;
        RaycastHit2D hit = Physics2D.CircleCast(_transform.position, raycastRadius, _moveDirection, raycastDistance, 1 << LayerMask.NameToLayer("Friendly"));
        lastAttackTime = Time.time;
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
