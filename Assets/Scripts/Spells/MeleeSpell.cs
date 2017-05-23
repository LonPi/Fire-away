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
        bool blinkDelayOver = true;
        if (SpellManager.BlinkInfo.castedBlinkPreviously)
            blinkDelayOver = false;
        // enforce delay in spell casting after blink
        if (!blinkDelayOver &&
            (Time.time - SpellManager.BlinkInfo.castedBlinkTimestamp >= SpellManager.BlinkInfo.SPELL_CAST_DELAY_AFTER_BLINK))
        {
            blinkDelayOver = true;
            SpellManager.BlinkInfo.Reset();
        }
        return timer <= 0f && (Time.time - lastInputTime >= inputDelay) && blinkDelayOver;
    }

    public bool Cast(Player player)
    {
        if (!CanCast())
            return false;

        Vector2 _spriteSize = player.spriteSize;
        Vector2 raycastDirection =  player.isFacingRight ? Vector2.right : Vector2.left;
        float raycastDistance = _spriteSize.x / 2 + range;
        // circle cast downwards if player is jumping+melee
        raycastDirection = !player.Controller.collisionInfo.below ? Vector2.down : raycastDirection;
        raycastDistance = _spriteSize.y + range;
        Vector2 raycastOrigin = player.transform.position;
        RaycastHit2D[] hits = Physics2D.CircleCastAll(raycastOrigin, raycastDistance, raycastDirection, raycastDistance+_spriteSize.y/2, 1 << LayerMask.NameToLayer("Enemy"));
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
