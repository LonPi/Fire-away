using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    public GameObject Enemy;
    public Transform[] SpawnPositions;
    public float spawnFrequency;
    public Player _playerRef { get; private set; }
    public Tree _treeRef { get; private set; }
    Vector2 curSpawnPosition;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    void Start () {
        InitReferences();
    }

    void InitReferences()
    {
        _playerRef = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _treeRef = GameObject.FindGameObjectWithTag("Tree").GetComponent<Tree>();
    }

    public void GameOver()
    {
        StopAllCoroutines();
        StartCoroutine(_ReloadLevel());
    }

    IEnumerator _ReloadLevel()
    {
        yield return new WaitForSeconds(1);
        Scene currentLevel = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentLevel.buildIndex);
    }

    IEnumerator _SpawnEnemy()
    {
        while (true)
        {
            float index = Random.Range(0, 1.99f);
            curSpawnPosition = SpawnPositions[(int)Mathf.Floor(index)].position;
            Instantiate(Enemy, curSpawnPosition, Quaternion.identity);
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
        StartCoroutine(_SpawnEnemy());
    }
}
