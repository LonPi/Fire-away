using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour {

    float damage;
    public Transform _transform { get { return transform; } private set { } }
    SpriteRenderer _spriteRenderer;
    BoxCollider2D _boxCollider;
    const float damageRegisterInterval = 0.5f;
    float lastDamageTime;

    void Start ()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _boxCollider = GetComponent<BoxCollider2D>();
        transform.parent = null;
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Friendly"));
    }

    public void SetParams(float damage)
    {
        this.damage = damage;
    }

    private void Update()
    {
        string frame = _spriteRenderer.sprite.name;
        // cap the damage instance per second
        if ((Time.time - lastDamageTime >= damageRegisterInterval) && (frame == "player_UltFx3" || frame == "player_UltFx4"))
        {
            InflictDamage();
        }
        if (frame == "player_UltFx5")
        {
            SelfDestroy();
        }
    }

    void InflictDamage()
    {
        Bounds bounds = _boxCollider.bounds;
        Vector2 raycastOrigin = bounds.center;
        float raycastDistance = bounds.size.y;
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


    void SelfDestroy()
    {
        Destroy(gameObject,1f);
    }
}
