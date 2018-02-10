using UnityEngine;
using System.Collections;

public class WaveSpawner : MonoBehaviour {

    public enum SpawnState { SPAWNING, WAITING, COUNTING};//defining the data structure

    [System.Serializable]
    public class Wave
    {
        public string name;
        public Transform enemy;
        public int count;
        public float rate;//spawn rate

    }

    public Wave[] waves;
    private int nextWave = 0;
    public int NextWave
    {
        get { return nextWave + 1; }
    }

    public Transform[] spawnPoints;

    public float timeBetweenWaves = 5f;
    private float waveCountdown;
    public float WaveCountdown
    {
        get { return waveCountdown; }
    }

    private float searchCountdown = 1f;

    private SpawnState state = SpawnState.COUNTING;
    public SpawnState State
    {
        get { return state; }
    }
    
	void Start ()
    {
        if (spawnPoints.Length == 0)
        {
            Debug.Log("No spawn points referenced.");
        }
            waveCountdown = timeBetweenWaves;	
	}
	
	void Update ()
    {
        if (state == SpawnState.WAITING)//check if player has killed all the enemies. if they are still alive
        {
            if (!EnemyIsAlive())
            {
                WaveCompleted();
            }
            else//if we still have alive enemies
            {
                return;//let the player kill them before we do it
            }
        }

        if (waveCountdown <= 0)
        {
            if (state != SpawnState.SPAWNING)
            {
                StartCoroutine(SpawnWave(waves[nextWave])); 
            }
        }
        else
        {
            waveCountdown -= Time.deltaTime;
        }
	}

    void WaveCompleted()
    {
        Debug.Log("Wave completed!");

        state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;

        if (nextWave + 1 > waves.Length - 1)
        {
            nextWave = 0;
            Debug.Log("ALL WAVES COMPLETE! Looping...");
        }
        else
        {
            nextWave++;
        }        
    }

    bool EnemyIsAlive()
    {
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0f)//checking if an enemy is alive only if we actually spawned enemies
        {
            searchCountdown = 1f;

            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                return false;
            }
        }
        return true;
    }

    IEnumerator SpawnWave(Wave _wave)
    {
        Debug.Log("Spawing Wave: " + _wave.name);
        state = SpawnState.SPAWNING;

        //spawn
        for (int i = 0; i < _wave.count; i++)//loop through the number of enemies we want to spawn
        {
            SpawnEnemy(_wave.enemy);//spawn an enemy
            yield return new WaitForSeconds(1f/_wave.rate);
        }

        state = SpawnState.WAITING;//when we are done spawning, we are waiting for the player to kill all the enemies

        yield break;
    }

    void SpawnEnemy(Transform _enemy)
    {
        Debug.Log("Spawning Enemy:" + _enemy.name);


        Transform _sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(_enemy, _sp.position, _sp.rotation);
        
    }
}


