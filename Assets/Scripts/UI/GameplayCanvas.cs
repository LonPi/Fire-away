using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayCanvas : MonoBehaviour {

    OnScreenMessage tryAgainText;

    void Start ()
    {
        tryAgainText = GameObject.Find("TryAgain").GetComponentInChildren<OnScreenMessage>();
	}

    public void OnRestartLevel()
    {
        tryAgainText.ShowText();
    }
}
