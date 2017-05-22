using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpell {
    float
        startHeight,
        damage,
        cooldown,
        timer,
        lastInputTime;
    GameObject Meteor;

    public MeteorSpell(float startHeight, float damage, float cooldown, GameObject Meteor)
    {
        this.startHeight = startHeight;
        this.damage = damage;
        this.cooldown = cooldown;
        timer = 0f;
        this.Meteor = Meteor;
    }

    public void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f) timer = 0f;
    }

    public bool CanCast()
    {
        return timer <= 0f;
    }

    public bool Cast(Player player)
    {
        if (!CanCast() || Time.time - lastInputTime <= 1f)
            return false;

        var _state = player.Controller.state;
        Vector2 spriteSize = player.spriteSize;
        Transform transform = player.transform;

        float offset_x = _state.isMovingRight ? spriteSize.y / 2 : -1 * spriteSize.y / 2;
        Vector2 meteorStartPosition = new Vector2(transform.position.x + offset_x, transform.position.y + startHeight);
        GameObject.Instantiate(Meteor, meteorStartPosition, Quaternion.identity, transform);
        lastInputTime = Time.time;
        timer = cooldown;
        return true;
    }
}
