using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [System.Serializable]
    public class Reserve
    {
        public GameObject gObject;
        public int amount;
        Reserve(GameObject _object, int _amount)
        {
            gObject = _object;
            amount = _amount;
        }
    }

    public List<Reserve> objectsToReserve;

    public Wave[] waves;
    public AI enemyAI;

    private int enemiesRemainingToSpawn;
    private int enemiesStillAlive;

    float nextSpawnTime;

    Wave currentWave;
    private int currentWaveNumber;

    [System.Serializable]
    public class Wave {
        public int numberOfEnemies;
        public float spawnDelay;
    }

    TileMapGenerator map;

    public event System.Action<int> OnWaveIncr;

    // Use this for initialization
    void Start () {
        foreach (Reserve item in objectsToReserve) {
            PoolManager.instance.CreatePool(item.gObject, item.amount);
        }

        map = FindObjectOfType<TileMapGenerator>();
        currentWaveNumber = 0;
        nextSpawnTime = 0;
        NextWave();
    }

    void Update()
    {
        if (enemiesRemainingToSpawn > 0 && Time.time > nextSpawnTime) {
            enemiesRemainingToSpawn--;
            nextSpawnTime = Time.time + currentWave.spawnDelay;

            //Subscribe to OnDeath
            StartCoroutine(SpawnEnemy());
        }
    }

    void OnEnemyDeath()
    {
        enemiesStillAlive--;

        if (enemiesStillAlive == 0) {
            NextWave();
        }
    }

    void NextWave() {
        currentWaveNumber++;

        if (currentWaveNumber - 1 < waves.Length)
        {
            currentWave = waves[currentWaveNumber - 1];

            enemiesRemainingToSpawn = currentWave.numberOfEnemies;
            enemiesStillAlive = enemiesRemainingToSpawn;

            if (OnWaveIncr != null) {
                OnWaveIncr(currentWaveNumber);
            }
        }
    }

    IEnumerator SpawnEnemy()
    {
        float spawnDelay = 1;
        float flashSpeed = 4;

        Transform randTile = map.GetRandomOpenTile();

        Material getMaterial = randTile.GetComponent<Renderer>().material;
        Color initialColor = getMaterial.color;
        Color flashColor = Color.red;

        float spawnTimer = 0;

        while (spawnTimer < spawnDelay)
        {
            getMaterial.color = Color.Lerp(initialColor, flashColor, Mathf.PingPong(spawnTimer * flashSpeed, 1));
            spawnTimer += Time.deltaTime;
            yield return null;
        }

        AI spawnedEnemy = Instantiate(enemyAI, randTile.position + Vector3.up, Quaternion.identity) as AI;
        spawnedEnemy.OnDeath += OnEnemyDeath;
    }
}
