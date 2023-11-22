using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<Wave> waves;
    public uint enemySpawnLimit { get; protected set; } //how much enemy will spawn
    public List<Enemy> spawnedEnemies { get; protected set; }

    bool waveDoneSpawning;
    int waveIndex;

    void Awake()
    {
        enemySpawnLimit = 0;
        foreach (Wave w in waves)
        {
            foreach (SpawnSequence ss in w.spawnSequences)
            {
                foreach (SpawnInfo si in ss.enemySpawnInfos)
                    enemySpawnLimit += si.quantity;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        spawnedEnemies = new List<Enemy>();
        StartCoroutine(SpawnEnemy());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator SpawnEnemy()
    {
        while (waveIndex < waves.Count)
        {
            waveDoneSpawning = false;
            yield return new WaitForSeconds(waves[waveIndex].delayBeforeSpawn);
            //Debug.Log("Spawning wave: " + waveIndex.ToString());
            yield return StartCoroutine(SpawnWave(waves[waveIndex]));

            //wait until wave down spawning and all enemies are dead, check every 0.2 sec
            while (!waveDoneSpawning || spawnedEnemies.Count != 0)
            {
                yield return new WaitForSeconds(0.2f);
            }

            waveIndex++;
        }
    }

    public IEnumerator SpawnWave(Wave wave)
    {
        if (wave.spawnSequences.Count > 0)
        {
            int index = 0;

            if (wave.spawnSequences[index].WaitType == WaitBetweenSequenceType.Parallel)
            {
                StartCoroutine(SpawnSpawnSequence(wave.spawnSequences[index], index));
            }
            else
            {
                yield return StartCoroutine(SpawnSpawnSequence(wave.spawnSequences[index], index));
            }
            index++;

            while (index < wave.spawnSequences.Count)
            {
                //yield return new WaitForSeconds(wave.spawnSequences[index].delayPostSequence);

                while (index < wave.spawnSequences.Count &&
                    wave.spawnSequences[index].WaitType == WaitBetweenSequenceType.Parallel)
                {
                    yield return new WaitForSeconds(wave.spawnSequences[index].delayPostSequence);
                    StartCoroutine(SpawnSpawnSequence(wave.spawnSequences[index], index));
                    index++;
                }

                if (spawnedEnemies.Count == 0 && index < wave.spawnSequences.Count)
                {
                    yield return new WaitForSeconds(wave.spawnSequences[index].delayPostSequence);
                    yield return StartCoroutine(SpawnSpawnSequence(wave.spawnSequences[index], index));
                    index++;
                }
            }

            waveDoneSpawning = true;
        }
    }

    public IEnumerator SpawnSpawnSequence(SpawnSequence spawnSequence, int i)
    {
        //Debug.Log("Spawning Sequence: " + i.ToString());
        foreach (SpawnInfo enemySpawnInfo in spawnSequence.enemySpawnInfos)
        {
            yield return StartCoroutine(Spawn(enemySpawnInfo, spawnSequence.path));
        }
        yield return new WaitForSeconds(0);
    }

    public IEnumerator Spawn(SpawnInfo enemySpawnInfo, Path path)
    {
        for (int i = 0; i < enemySpawnInfo.quantity; i++)
        {
            Enemy e = Instantiate(enemySpawnInfo.enemy, transform.position, new Quaternion(0.0f, 0.0f, 0.0f, 0.0f));
            spawnedEnemies.Add(e);
            LevelManager.Instance.AddEnemy(e);
            e.Initialize(path);

            yield return new WaitForSeconds(enemySpawnInfo.delayBetweenSpawn);
        }
    }

    [ExecuteInEditMode]
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(this.transform.position, 0.5f);
        Gizmos.color = Color.red;
        foreach (Wave w in waves)
        {
            foreach (SpawnSequence ss in w.spawnSequences)
                Gizmos.DrawLine(this.transform.position, ss.path.GetNodePosition(0));
        }
    }
}
