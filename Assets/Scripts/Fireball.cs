using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour {

    float moveSpeed = 2f;
    PlayerController controller;
    Vector2 _velocity;

	void Start () {
        controller = GetComponentInParent<PlayerController>();
        var _state = controller.state;
        if (_state.isMovingRight) _velocity = new Vector2(moveSpeed, 0f);
        else _velocity = new Vector2(-1 * moveSpeed, 0f);
        transform.parent = null;
    }
	
	void Update () {
        transform.Translate(_velocity * Time.deltaTime);
    }
}
