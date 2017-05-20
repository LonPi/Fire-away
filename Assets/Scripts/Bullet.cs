using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    float bulletSpeed = 3f;
    Vector2 _velocity;

    // Use this for initialization
    void Start () {
        _velocity = new Vector2(bulletSpeed, 0f);
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(_velocity * Time.deltaTime);
    }
}
