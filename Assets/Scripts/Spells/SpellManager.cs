using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellManager: MonoBehaviour {

    [SerializeField]
    float
        meleeDamage,
        meleeRange,
        meleeCooldown,
        blinkRange,
        blinkCooldown,
        fireballDamage,
        fireballMoveSpeed,
        fireballTravelDistance,
        fireballCooldown,
        meteorInitialFallingHeight,
        meteorCastRange,
        meteorAreaOfDamage,
        meteorDamage,
        meteorCooldown;
    public GameObject FireballPrefab, MeteorPrefab;
    public MeleeSpell meleeSpell { get; private set; }
    public BlinkSpell blinkSpell { get; private set; }
    public FireballSpell fireballSpell { get; private set; }
    public MeteorSpell meteorSpell { get; private set; }
    
    void Awake()
    {
        meleeSpell = new MeleeSpell(meleeDamage, meleeRange, meleeCooldown);
        blinkSpell = new BlinkSpell(blinkRange, blinkCooldown);
        fireballSpell = new FireballSpell(fireballDamage, fireballMoveSpeed, fireballTravelDistance, fireballCooldown, FireballPrefab);
        meteorSpell = new MeteorSpell(meteorInitialFallingHeight, meteorCastRange, meteorAreaOfDamage, meteorDamage, meteorCooldown, MeteorPrefab);
    }

    void Update()
    {
        meleeSpell.Update();
        blinkSpell.Update();
        fireballSpell.Update();
        meteorSpell.Update();
    }

    public struct BlinkInfo
    {
        public const float SPELL_CAST_DELAY_AFTER_BLINK = 1f;
        public static bool castedBlinkPreviously;
        public static float castedBlinkTimestamp;
        public static void Reset()
        {
            castedBlinkPreviously = false;
            castedBlinkTimestamp = 0f;
        }
    }

    
}
