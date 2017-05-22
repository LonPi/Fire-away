using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour {

    public float moveSpeed;
    public float damage;
    public float lifeSpan;
    PlayerController controller;
    Vector2 _velocity;
    bool _wasCollingBefore;
    Vector2 _spriteSize;

	void Start () {
        controller = GetComponentInParent<PlayerController>();
        _spriteSize = new Vector2(GetComponent<SpriteRenderer>().bounds.size.x, GetComponent<SpriteRenderer>().bounds.size.y);
        var _state = controller.state;
        if (_state.isMovingRight) _velocity = new Vector2(moveSpeed, 0f);
        else _velocity = new Vector2(-1 * moveSpeed, 0f);
        transform.parent = null;
        _wasCollingBefore = false;
    }
	
	void Update () {
        transform.Translate(_velocity * Time.deltaTime);
        InflictDamage();
    }

    void InflictDamage()
    {
        float raycastRadius, raycastDistance;
        Vector2 directionX = _velocity.x < 0 ? Vector2.left : Vector2.right;
        raycastDistance = raycastRadius = _spriteSize.x/2;
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, raycastRadius, directionX, raycastDistance, 1 << LayerMask.NameToLayer("Enemy"));
        if (hit && !_wasCollingBefore)
        {
            Enemy enemy = hit.collider.gameObject.GetComponent<Enemy>();
            Debug.Log("Fireball: " + damage + " damage inflicted to enemy");
            enemy.TakeDamage(damage);
            _wasCollingBefore = true;
        }
        if (!hit && _wasCollingBefore)
        {
            _wasCollingBefore = false;
        }
    }
}
