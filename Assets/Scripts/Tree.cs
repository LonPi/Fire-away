using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour {

    public float maxHitPoints;
    public float hitPoints { get; private set; }
    public Transform _transform { get { return transform; } private set { } }
    bool _isDead;

    void Start () {
        hitPoints = maxHitPoints;
        _isDead = false;
    }
	
	void Update () {
        if (hitPoints <= 0 && !_isDead)
        {
            GameManager.instance.GameOver();
            _isDead = true;
        }
    }

    public void TakeDamage(float damage)
    {
        hitPoints -= damage;
    }
}
