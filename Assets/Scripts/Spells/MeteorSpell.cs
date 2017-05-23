using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpell {
    float
        startHeight,
        damage,
        cooldown,
        timer,
        lastInputTime,
        inputDelay;
    GameObject MeteorPrefab;

    public MeteorSpell(float startHeight, float damage, float cooldown, GameObject MeteorPrefab)
    {
        this.startHeight = startHeight;
        this.damage = damage;
        this.cooldown = cooldown;
        timer = 0f;
        this.MeteorPrefab = MeteorPrefab;
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

        var _state = player.Controller.state;
        Vector2 spriteSize = player.spriteSize;
        Transform transform = player.transform;
        float offset_x = player.isFacingRight ? spriteSize.y / 2 : -1 * spriteSize.y / 2;
        Vector2 meteorStartPosition = new Vector2(transform.position.x + offset_x, transform.position.y + startHeight);
        GameObject gameObj = GameObject.Instantiate(MeteorPrefab, meteorStartPosition, Quaternion.identity, transform);
        gameObj.GetComponent<Meteor>().SetParams(damage);
        lastInputTime = Time.time;
        timer = cooldown;
        return true;
    }
}
