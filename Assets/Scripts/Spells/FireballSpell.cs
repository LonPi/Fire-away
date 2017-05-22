using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballSpell {
    float
        damage,
        cooldown,
        timer,
        lastInputTime;
    GameObject Fireball;

   public FireballSpell(float damage, float cooldown, GameObject Fireball)
    {
        this.damage = damage;
        this.cooldown = cooldown;
        this.Fireball = Fireball;
        timer = 0f;
    }

    public void Update()
    {
        Debug.Log("Fireball cd: " + timer);
        timer -= Time.deltaTime;
        if (timer <= 0f) timer = 0f;
    }

    public bool CanCastSpell()
    {
        return timer <= 0f;
    }

    public bool Cast(Player player)
    {
        if (!CanCastSpell() || Time.time - lastInputTime <= 1f)
        {
            return false;
        }

        var _state = player.Controller.state;
        Vector2 spriteSize = player.spriteSize;
        Transform _transform = player.transform;
        float offset_x = _state.isMovingRight ? spriteSize.y / 2 : -1 * spriteSize.y / 2;
        Vector2 spawnPosition = new Vector2(_transform.position.x + offset_x, _transform.position.y);
        GameObject.Instantiate(Fireball, spawnPosition, Quaternion.identity, _transform);

        // cooldown active
        timer = cooldown;
        // record last input time
        lastInputTime = Time.time;
        return true;
    }
}
