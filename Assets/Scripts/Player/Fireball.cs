using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour {

    float
        moveSpeed,
        damage,
        travelDistance;
    int moveDirection;
    Player player;
    Vector2 _velocity;
    Vector2 targetPosition;
    Vector2 _spriteSize;

	void Start () {
        player = GetComponentInParent<Player>();
        _spriteSize = new Vector2(GetComponent<SpriteRenderer>().bounds.size.x, GetComponent<SpriteRenderer>().bounds.size.y);
        targetPosition = new Vector2(transform.position.x + travelDistance, transform.position.y);
        _velocity = new Vector2(moveSpeed * moveDirection, 0f);
        transform.parent = null;
    }

    void Update () {
        Move();
        InflictDamage();
    }

    void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, _velocity.x * Time.deltaTime);
        if ((Vector2)transform.position == targetPosition)
        {
            Destroy(gameObject);
        }
    }

    public void SetParams(float moveSpeed, float damage, float travelDistance, int moveDirection)
    {
        this.moveSpeed = moveSpeed;
        this.damage = damage;
        this.travelDistance = travelDistance;
        this.moveDirection = moveDirection;
    }

    void InflictDamage()
    {
        float raycastRadius, raycastDistance;
        Vector2 raycastDirection = _velocity.x < 0 ? Vector2.left : Vector2.right;
        raycastDistance = raycastRadius = _spriteSize.x/2;
        Vector2 raycastOrigin = transform.position;
        RaycastHit2D[] hits = Physics2D.CircleCastAll(raycastOrigin, raycastRadius, raycastDirection, raycastDistance, 1 << LayerMask.NameToLayer("Enemy"));
        foreach (RaycastHit2D hit in hits)
        {
            Enemy enemy = hit.collider.gameObject.GetComponent<Enemy>();
            enemy.TakeDamage(damage);
        }
    }
}
