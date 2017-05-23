using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour {

    float damage, areaOfDamage;
    public Transform _transform { get { return transform; } private set { } }
    BoxCollider2D _boxCollider;
    const float damageRegisterInterval = 0.5f;
    float lastDamageTime;

    void Start ()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        transform.parent = null;
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Friendly"));
    }

    public void SetParams(float damage, float areaOfDamage)
    {
        this.damage = damage;
        this.areaOfDamage = areaOfDamage;
    }

    private void Update()
    {
        // cap the damage instance per second
        if ((Time.time - lastDamageTime >= damageRegisterInterval))
            InflictDamage();
    }

    void InflictDamage()
    {
        Bounds bounds = _boxCollider.bounds;
        Vector2 raycastOrigin = bounds.center;
        float raycastDistance = areaOfDamage;
        Vector2 raycastDirection = Vector2.down;

        RaycastHit2D[] hits = Physics2D.CircleCastAll(raycastOrigin, raycastDistance, raycastDirection, raycastDistance, 1 << LayerMask.NameToLayer("Enemy"));
        foreach(RaycastHit2D hit in hits)
        {
            Enemy enemy = hit.collider.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
        lastDamageTime = Time.time;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Collision"))
            Destroy(gameObject, 0.5f);
    }
}
