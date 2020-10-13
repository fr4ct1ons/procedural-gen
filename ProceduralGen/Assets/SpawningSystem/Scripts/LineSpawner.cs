using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineSpawner : MonoBehaviour
{
    public Vector3 p0 = Vector3.zero, p1 = Vector3.one;
    [SerializeField] private SpawnerWave wave;
    [SerializeField] private bool loop;
    
    private Vector3 bufferVector;
    private bool trigger = false, canSpawn = true;
    private int waveIndex = 0, enemyCount = 1;

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
        bufferVector = Vector3.Lerp(transform.TransformPoint(p0), transform.TransformPoint(p1), Random.Range(0.0f, 1.0f));
        Instantiate(wave.GetElement(waveIndex), bufferVector, Quaternion.identity); // Spawn the enemy
        enemyCount++; // Count number of the spawned enemies
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
                bufferVector = Vector3.Lerp(transform.TransformPoint(p0), transform.TransformPoint(p1), Random.Range(0.0f, 1.0f));
                Instantiate(wave.GetElement(i), bufferVector, Quaternion.identity); // Spawn the enemy
            }
        }
    }
}
