using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEditor;
using Random = UnityEngine.Random;

public class BezierSpawner : MonoBehaviour
{
    [SerializeField] private SpawnerWave wave;
    [SerializeField] private bool loop;
    
    public Vector3[] points;

    private Vector3 bufferVector;
    private bool trigger = false, canSpawn = true;
    private int waveIndex = 0, enemyCount = 1;

    private void Reset()
    {
        points = new Vector3[]
        {
            new Vector3(1, 0, 0),
            new Vector3(2, 0, 0),
            new Vector3(3, 0, 0),
            new Vector3(4, 0, 0)
        };
    }
    
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
        bufferVector = GetPoint(Random.Range(0.0f, 1.0f));
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
                bufferVector = GetPoint(Random.Range(0.0f, 1.0f));
                Instantiate(wave.GetElement(i), bufferVector, Quaternion.identity); // Spawn the enemy
            }
        }
    }

    public Vector3 GetPoint(float t)
    {
        return transform.TransformPoint(Bezier.GetPoint(points[0], points[1], points[2], points[3], t));
    }
    
    public Vector3 GetVelocity (float t) {
        return transform.TransformPoint(Bezier.GetFirstDerivative(points[0], points[1], points[2], points[3], t)) -
               transform.position;
    }
    
    public Vector3 GetDirection (float t) {
        return GetVelocity(t).normalized;
    }
}