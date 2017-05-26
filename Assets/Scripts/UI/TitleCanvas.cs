using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleCanvas : MonoBehaviour {

	public void OnPressStart()
    {
        SceneManager.LoadScene(1);        
    }
}
