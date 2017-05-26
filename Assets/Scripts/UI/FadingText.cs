using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadingText : MonoBehaviour {

    public float lifeSpan;
    float aliveTimer;
    public Text text;  

	void Start () {
        aliveTimer = 0f;
        text = GetComponent<Text>();

    }
	
	void Update () {
        aliveTimer += Time.deltaTime;
        if (aliveTimer >= lifeSpan)
        {
            text.enabled = false;
        }
    }

    public void ShowText()
    {
        text.enabled = true;
        aliveTimer = 0f;
    }

    // cant call this method in OnSceneLoaded
    // instead, text component has to be disabled from the beginning rather than calling this method
    public void HideText()
    {
        text.enabled = false;
        aliveTimer = 0f;
    } 
    public void SetParams(float lifeSpan)
    {
        this.lifeSpan = lifeSpan;
    }
}
