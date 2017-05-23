using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballSpell {
    float
        damage,
        moveSpeed,
        travelDistance,
        cooldown,
        timer,
        inputDelay,
        lastInputTime;
    GameObject FireballPrefab;

   public FireballSpell(float damage, float moveSpeed, float travelDistance, float cooldown, GameObject FireballPrefab)
    {
        this.damage = damage;
        this.moveSpeed = moveSpeed;
        this.travelDistance = travelDistance;
        this.cooldown = cooldown;
        this.FireballPrefab = FireballPrefab;
        timer = 0f;
        inputDelay = 0.3f;
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
        Transform _transform = player.transform;
        float offset_x = player.isFacingRight ? spriteSize.y / 2 : -1 * spriteSize.y / 2;
        Vector2 spawnPosition = new Vector2(_transform.position.x + offset_x, _transform.position.y);
        GameObject gameObj = GameObject.Instantiate(FireballPrefab, spawnPosition, Quaternion.identity, _transform);
        gameObj.GetComponent<Fireball>().SetParams(moveSpeed, damage, travelDistance, player.isFacingRight ? 1: -1);
        // cooldown active
        timer = cooldown;
        // record last input time
        lastInputTime = Time.time;
        return true;
    }
}
