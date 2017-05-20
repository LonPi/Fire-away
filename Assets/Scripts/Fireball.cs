using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour {

    float moveSpeed = 2f;
    float skinWidth = .015f;
    PlayerController controller;
    Vector2 _velocity;
    

	void Start () {
        controller = GetComponentInParent<PlayerController>();
        var _state = controller.state;
        if (_state.isMovingRight) _velocity = new Vector2(moveSpeed, 0f);
        else _velocity = new Vector2(-1 * moveSpeed, 0f);
        transform.parent = null;
    }
	
	void Update () {
        transform.Translate(_velocity * Time.deltaTime);
        DetectEnemy();
    }

    void DetectEnemy()
    {
        float raycastRadius, raycastDistance;
        Vector2 directionX = _velocity.x < 0 ? Vector2.left : Vector2.right;
        raycastDistance = raycastRadius = Mathf.Abs(_velocity.x) * Time.deltaTime;
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, raycastRadius, directionX, raycastDistance, 1 << LayerMask.NameToLayer("Enemy"));
        if (hit)
        {
            Destroy(hit.collider.gameObject);
            Destroy(gameObject);
        }
    }
}
