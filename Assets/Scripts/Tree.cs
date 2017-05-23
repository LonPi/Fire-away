using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour {

    public float hitPoints;
    public Transform _transform { get { return transform; } private set { } }

    void Start () {
    }
	
	void Update () {
		
	}

    public void TakeDamage(float damage)
    {
        hitPoints -= damage;
        Debug.Log(hitPoints);
    }
}
