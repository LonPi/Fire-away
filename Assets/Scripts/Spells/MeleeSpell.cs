using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeSpell {

    float
        damage,
        range,
        cooldown,
        timer,
        inputDelay = 0.3f,
        lastInputTime;

    public MeleeSpell(float damage, float range, float cooldown)
    {
        timer = 0f;
        lastInputTime = 0f;
        this.damage = damage;
        this.range = range;
        this.cooldown = cooldown;
    }

    public void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f) timer = 0f;
    }

    public bool CanCastSpell()
    {
        return timer <= 0f;
    }

    public bool Cast(Player player)
    {
        if (!CanCastSpell() || Time.time - lastInputTime <= inputDelay)
        {
            return false;
        }
        var _state = player.Controller.state;
        Transform _transform = player.transform;
        Vector2 _spriteSize = player.spriteSize;
        Vector2 direction = _state.isMovingRight ? Vector2.right : Vector2.left;
        Vector2 raycastOrigin;
        if (direction == Vector2.left)
            raycastOrigin = new Vector2(_transform.position.x - _spriteSize.x / 2, _transform.position.y);
        else
            raycastOrigin = new Vector2(_transform.position.x + _spriteSize.x / 2, _transform.position.y);
        RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, direction, range, 1 << LayerMask.NameToLayer("Enemy"));
        Debug.DrawRay(raycastOrigin, direction * range, Color.cyan);
        if (hit)
        {
            Enemy enemy = hit.collider.gameObject.GetComponent<Enemy>();
            Debug.Log("Player: enemy has taken " + range + " melee damage.");
            enemy.TakeDamage(range);
        }
        // cooldown active
        timer = cooldown;
        // record last input time
        lastInputTime = Time.time;
        return true;
    }
}
