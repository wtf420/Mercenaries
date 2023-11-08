using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    LevelManager levelManager;

    [SerializeField]
    GameObject spawnEnemy;
    [SerializeField]
    Path path;
    [SerializeField]
    bool button;
    bool buttonCheck, spawnAble = true;

    public int enemySpawnLimit; //9999 means infinite
    public float enemySpawnInterval;

    // Start is called before the first frame update
    void Start()
    {
        levelManager = LevelManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        // if (button != buttonCheck)
        // {
        //     SpawnEnemy();
        // }
        // button = buttonCheck;
        if (spawnAble && enemySpawnLimit > 0)
            StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy()
    {
        spawnAble = false;

        GameObject enemy = Instantiate(spawnEnemy, this.transform.position, this.transform.rotation);
        Enemy e = enemy.GetComponent<Enemy>();
        e.Initialize(path);
        levelManager.AddEnemy(e);
        enemySpawnLimit -= 1;

        yield return new WaitForSeconds(enemySpawnInterval);
        spawnAble = true;
    }

    [ExecuteInEditMode]
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(this.transform.position, 0.5f);
    }
}
