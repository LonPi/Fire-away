using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour {

    public float hitPoints;
    public Transform _transform { get { return transform; } private set { } }
    bool _isDead;

    void Start () {
        _isDead = false;
    }
	
	void Update () {
		
	}

    public void TakeDamage(float damage)
    {
        hitPoints -= damage;
        Debug.Log("Tree: took " + damage + " damage. HP remaining: " + hitPoints);
        if (hitPoints <= 0)
        {
            Debug.Log("Tree is dead.");
            _isDead = true;
        }
    }
}
