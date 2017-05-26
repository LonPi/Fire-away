using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayCanvas : MonoBehaviour {

    GameOverText gameOverText;

    void Start () {
        gameOverText = GetComponentInChildren<GameOverText>();
	}
	
    public void OnGameOver()
    {
        gameOverText.ShowText();
    }
}
