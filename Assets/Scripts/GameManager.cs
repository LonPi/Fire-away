using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    public GameObject Enemy;
    public Transform[] SpawnPositions;
    public float spawnFrequency;
    public float maxEnemies;
    float spawnTimer = 0f;
    float numEnemies = 0;
    public float enemyLifeSpan;
    public Player _playerRef { get; private set; }
    Vector2 curSpawnPosition;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    // Use this for initialization
    void Start () {
        _playerRef = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update () {
        SpawnEnemy();
	}

    void SpawnEnemy()
    {
        spawnTimer += Time.deltaTime;
        float index = Random.Range(0, 1.99f);
        curSpawnPosition = SpawnPositions[(int)Mathf.Floor(index)].position;

        if (spawnTimer >= 1/spawnFrequency)
        {
            if (numEnemies < maxEnemies)
            {
                
                GameObject enemy = Instantiate(Enemy, curSpawnPosition, Quaternion.identity);
                Destroy(enemy, enemyLifeSpan);
                numEnemies++;
            }
            spawnTimer = 0f;
        }
    }
}
