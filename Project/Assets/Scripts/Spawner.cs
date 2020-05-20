using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] EnemySpawnPoints;
    public GameObject[] PlayerSpawnPoints;
    public GameObject fighter;
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject spawnPoint in EnemySpawnPoints) {
            Instantiate(fighter, spawnPoint.transform);
        }

        foreach (GameObject spawnPoint in PlayerSpawnPoints)
        {
            Instantiate(fighter, spawnPoint.transform);
        }
    }
}
