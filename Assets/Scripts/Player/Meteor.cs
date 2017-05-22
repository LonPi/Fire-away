using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour {

    public float moveVelocity;
    public float damage;
    PlayerController controller;
    Rigidbody2D rb2D;
    Vector2 _velocity;
    Vector2 direction;

    void Start () {
        controller = GetComponentInParent<PlayerController>();
        rb2D = GetComponent<Rigidbody2D>();
        transform.parent = null;
        var _state = controller.state;
        direction = _state.isMovingRight ? Vector2.right : Vector2.left;
        rb2D.AddForce(new Vector2(moveVelocity * direction.x, 0f));

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.layer == LayerMask.NameToLayer("Enemy")))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            Debug.Log("Meteor: " + damage + " damage inflicted to enemy");
            enemy.TakeDamage(damage);
            
        }
        Destroy(gameObject,3);
    }


}
