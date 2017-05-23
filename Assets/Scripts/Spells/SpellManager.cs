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
        meteorStartHeight,
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
        meteorSpell = new MeteorSpell(meteorStartHeight, meteorDamage, meteorCooldown, MeteorPrefab);
    }

    void Start()
    {
    }

    void Update()
    {
        meleeSpell.Update();
        blinkSpell.Update();
        fireballSpell.Update();
        meteorSpell.Update();
    }

    
}
