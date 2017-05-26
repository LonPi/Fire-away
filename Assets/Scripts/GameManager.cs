using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    public GameObject[] Enemies;
    public GameObject testText;
    public Transform[] SpawnPositions;
    public float spawnFrequency;
    public Player _playerRef { get; private set; }
    public Tree _treeRef { get; private set; }
    public Camera _cameraRef { get; private set; }
    public int currentLevel { get; private set; }
    public float currentExp { get; private set; }
    public float expRequiredToCompleteLevel { get; private set; }
    FadingText _gameLostText;
    Vector2 curSpawnPosition;
    float fadeTime = 2.0f;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    void InitReferences()
    {
        _playerRef = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _treeRef = GameObject.FindGameObjectWithTag("Tree").GetComponent<Tree>();
        _cameraRef = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        _gameLostText = GameObject.Find("GameLost").GetComponent<FadingText>();
    }

    void ResetProgression()
    {
        currentLevel = 1;
        currentExp = 0;
        expRequiredToCompleteLevel = 200; // for level 1
    }

    public void GameOver()
    {
        StopAllCoroutines();
        _gameLostText.ShowText();
        StartCoroutine(_ReloadLevel());
    }

    public void IncrementExp(float exp)
    {
        this.currentExp += exp;
        //Debug.Log("gained " + exp + " currrent exp: " + currentExp);
        if (currentExp >= expRequiredToCompleteLevel)
        {
            currentLevel++;
            currentExp = 0;
            expRequiredToCompleteLevel += expRequiredToCompleteLevel * 0.2f;
            expRequiredToCompleteLevel =  Mathf.Floor(expRequiredToCompleteLevel);
            SoundManager.instance.PlaySingle(SoundManager.instance.levelUpSFX);
        }
    }

    IEnumerator _ReloadLevel()
    {
        yield return new WaitForSeconds(fadeTime);
        fadeTime = GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        Scene loadedLevel = SceneManager.GetActiveScene();
        SceneManager.LoadScene(loadedLevel.buildIndex);
    }

    IEnumerator _SpawnEnemy()
    {
        while (true)
        {
            float spawnIndex = Random.Range(0, SpawnPositions.Length-0.01f);
            float enemyIndex = Random.Range(0, Enemies.Length - 0.01f);
            curSpawnPosition = SpawnPositions[(int)Mathf.Floor(spawnIndex)].position;
            GameObject selectedEnemy = Enemies[(int)Mathf.Floor(enemyIndex)];
            GameObject enemyObj = Instantiate(selectedEnemy, curSpawnPosition, Quaternion.identity);
            enemyObj.GetComponent<Enemy>().SetParams(currentLevel);
            yield return new WaitForSeconds(1 / spawnFrequency);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitReferences();
        ResetProgression();
        StartCoroutine(_SpawnEnemy());
    }
}
