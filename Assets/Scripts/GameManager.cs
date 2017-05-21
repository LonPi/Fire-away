using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    public GameObject Enemy;
    public Transform[] SpawnPositions;
    public float spawnFrequency;
    public Player _playerRef { get; private set; }
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
        _playerRef = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        StartCoroutine(_SpawnEnemy());
	}

    IEnumerator _SpawnEnemy()
    {
        while (true)
        {
            float index = Random.Range(0, 1.99f);
            curSpawnPosition = SpawnPositions[(int)Mathf.Floor(index)].position;
            yield return new WaitForSeconds(1 / spawnFrequency);
            Instantiate(Enemy, curSpawnPosition, Quaternion.identity);
        }
    }
}
