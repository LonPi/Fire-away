using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeSpell {

    float
        damage,
        range,
        cooldown,
        inputDelay,
        lastInputTime,
        timer;

    public MeleeSpell(float damage, float range, float cooldown)
    {
        timer = 0f;
        this.damage = damage;
        this.range = range;
        this.cooldown = cooldown;
        this.inputDelay = 0.3f;
    }

    public void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f) timer = 0f;
    }

    public bool CanCast()
    {
        return timer <= 0f && (Time.time - lastInputTime >= inputDelay);
    }

    public bool Cast(Player player)
    {
        if (!CanCast())
            return false;

        Transform _transform = player.transform;
        Vector2 _spriteSize = player.spriteSize;
        Vector2 raycastDirection =  player.isFacingRight ? Vector2.right : Vector2.left;
        Vector2 raycastOrigin;
        if (raycastDirection == Vector2.left)
            raycastOrigin = new Vector2(_transform.position.x - _spriteSize.x / 2, _transform.position.y);
        else
            raycastOrigin = new Vector2(_transform.position.x + _spriteSize.x / 2, _transform.position.y);
        float raycastDistance = _spriteSize.x/ 2 + range;
        RaycastHit2D[] hits = Physics2D.CircleCastAll(raycastOrigin, raycastDistance, raycastDirection, raycastDistance, 1 << LayerMask.NameToLayer("Enemy"));

        foreach (RaycastHit2D hit in hits)
        {
            Enemy enemy = hit.collider.gameObject.GetComponent<Enemy>();
            enemy.TakeDamage(damage);
        }
        // cooldown active
        timer = cooldown;
        lastInputTime = Time.time;
        return true;
    }
}
