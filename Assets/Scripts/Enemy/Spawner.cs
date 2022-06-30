using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] targetPrefabs;
    public Transform[] spawnPoints;
    public LayerMask groundMask;
    public float spawnInterval = 3f;
    public float rangeX = 20f;
    public float rangeZ = 20f;

    private float nextSpawnTime;


    // Start is called before the first frame update
    void Start()
    {
        nextSpawnTime = Time.time + spawnInterval;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnOneEnemyRandomly();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    private void SpawnOneEnemyRandomly()
    {
        int index = Random.Range(0, spawnPoints.Length);
        int toBeSpawn = Random.Range(0, targetPrefabs.Length);
        Instantiate(targetPrefabs[toBeSpawn], spawnPoints[index].position + Vector3.up * 2.5f, Quaternion.identity);
        
        // RaycastHit hit;
        // while (true)
        // {
        //     Vector3 pos = new Vector3(Random.Range(-rangeX, rangeX), 200f, Random.Range(-rangeZ, rangeZ));

        //     if (Physics.Raycast(pos, Vector3.down, out hit, 300f, groundMask))
        //     {
        //         int toBeSpawn = Random.Range(0, targetPrefabs.Length);
        //         Instantiate(targetPrefabs[toBeSpawn], hit.point + Vector3.up * 3f, Quaternion.identity);
        //         break;
        //     }
        // }
    }
}
