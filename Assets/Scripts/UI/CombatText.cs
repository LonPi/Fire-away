using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatText : MonoBehaviour {

    public float moveSpeed;
    public float lifeSpan;
    Text text;
    string fillText;
    Enemy parent;

    void Start () {
        text = GetComponent<Text>();
        text.text = fillText;
        parent = GetComponentInParent<Enemy>();
        Destroy(gameObject, lifeSpan);
	}
	
    public void SetParams(string _fillText)
    {
        this.fillText = _fillText;
    }

	void Update () {

        Flip();
        transform.Translate(new Vector2(0f, moveSpeed * Time.deltaTime));
	}

    void Flip()
    {
        RectTransform rect = GetComponent<RectTransform>();
        if (parent.transform.localScale.x < 0 && rect.localScale.x > 0 || parent.transform.localScale.x > 0 && rect.localScale.x < 0)
        {
            Vector3 theScale = rect.localScale;
            theScale.x *= -1;
            rect.localScale = theScale;
        }
    }

}
