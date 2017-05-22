using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkSpell {
    float
        range,
        cooldown,
        timer,
        lastInputTime;

    public BlinkSpell(float range, float cooldown)
    {
        timer = 0f;
        lastInputTime = 0f;
        this.range = range;
        this.cooldown = cooldown;
    }

    public void Update()
    {
        Debug.Log("Blink cd: " + timer);
        timer -= Time.deltaTime;
        if (timer <= 0f) timer = 0f;
    }

    public bool CanCastSpell()
    {
        return timer <= 0f;
    }

    public bool Cast(Player player, Vector2 direction)
    {
        if (!CanCastSpell() || Time.time - lastInputTime <= 1f)
        {
            return false;
        }

        player.Controller.Move(range * direction);

        // cooldown active
        timer = cooldown;
        // record last input time
        lastInputTime = Time.time;
        return true;
    }
}
