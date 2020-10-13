using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


/// <summary>
/// EnemySpawner class - A simple class that spawns enemies based on a ScriptableWave object.
/// </summary>
public class VolumeSpawner : MonoBehaviour
{
    [SerializeField] SpawnerWave wave;
    [SerializeField] bool loop;

    public Vector3 topR = new Vector3(-1, -1, -1);
    public Vector3 bottomL = new Vector3(1, 1, 1); 
    Vector3 bufferVector;
    bool canSpawn = true, trigger = false;
    int waveIndex = 0, enemyCount = 1;

    void Update()
    {
        if (trigger && canSpawn)
            StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy()
    {
        if (waveIndex > wave.NumOfElements() - 1) // If the end of the list was reached: Destroy myself.
        {
            if(!loop)
                Destroy(gameObject);
            else
            {
                trigger = false;
                waveIndex = 0;
                enemyCount = 1;
            }
        }
        canSpawn = false;
        enemyCount++; // Count number of the spawned enemies
        
        bufferVector.Set(Random.Range(bottomL.x, topR.x), Random.Range(bottomL.y, topR.y), Random.Range(bottomL.z, topR.z));
        bufferVector = transform.TransformPoint(bufferVector);
        Instantiate(wave.GetElement(waveIndex), bufferVector, Quaternion.identity); // Spawn the enemy
        yield return new WaitForSeconds(wave.GetSpawnInterval(waveIndex)); // Wait for the time delay for the spawned enemy
        
        if (enemyCount > wave.GetSpawnAmount(waveIndex)) // If the required number of enemies was spawned: Advance in the list
        {
            waveIndex++;
            enemyCount = 1;
        }

        canSpawn = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!trigger)
            trigger = true;
    }

    public void SpawnAll()
    {
        for (int i = 0; i < wave.NumOfElements(); i++)
        {
            for (int j = 0; j < wave.GetSpawnAmount(i); j++)
            {
                bufferVector.Set(Random.Range(bottomL.x, topR.x), Random.Range(bottomL.y, topR.y), Random.Range(bottomL.z, topR.z));
                bufferVector = transform.TransformPoint(bufferVector);
                Instantiate(wave.GetElement(i), bufferVector, Quaternion.identity); // Spawn the enemy
            }
        }
    }
}