using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PolygonSpawner : MonoBehaviour
{
    [SerializeField] SpawnerWave wave;
    [SerializeField] bool loop;

    public List<Vector3> points;
    
    Vector3 bufferVector, bottomL, topR;
    bool canSpawn = true, trigger = false;
    int waveIndex = 0, enemyCount = 1;

    private void Awake()
    {
        PolygonBounds(points, out bottomL, out topR, transform.position.y);
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
        enemyCount++; // Count number of the spawned enemies
        do
        {
            bufferVector.Set(Random.Range(bottomL.x, topR.x), 0, Random.Range(bottomL.z, topR.z));
            bufferVector = transform.TransformPoint(bufferVector);
        } while (!ContainsPoint(points, bufferVector));

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
        Vector3 tempBottomL, tempTopR;
        PolygonBounds(points, out tempBottomL, out tempTopR, transform.position.y);
        for (int i = 0; i < wave.NumOfElements(); i++)
        {
            for (int j = 0; j < wave.GetSpawnAmount(i); j++)
            {
                do
                {
                    bufferVector.Set(Random.Range(tempBottomL.x, tempTopR.x), 0, Random.Range(tempBottomL.z, tempTopR.z));
                    bufferVector = transform.TransformPoint(bufferVector);
                } while (!ContainsPoint(points, bufferVector));
                Instantiate(wave.GetElement(i), bufferVector, Quaternion.identity); // Spawn the enemy
            }
        }
    }
    
    public static bool ContainsPoint(List<Vector3> polyPoints, Vector3 p)
    {
        var j = polyPoints.Count - 1;
        var inside = false;
        for (int i = 0; i < polyPoints.Count; j = i++)
        {
            var pi = polyPoints[i];
            var pj = polyPoints[j];
            if (((pi.z <= p.z && p.z < pj.z) || (pj.z <= p.z && p.z < pi.z)) &&
                (p.x < (pj.x - pi.x) * (p.z - pi.z) / (pj.z - pi.z) + pi.x))
                inside = !inside;
        }
        return inside;
    }

    public static void PolygonBounds(List<Vector3> polyPoints, out Vector3 bottomL, out Vector3 topR, float height=0.0f)
    {
        float yTop = 0, yBottom = 0, xLeft = 0, xRight = 0;
        
        for(int i = 0; i < polyPoints.Count; i++)
        {
            if (polyPoints[i].x < xLeft)
                xLeft = polyPoints[i].x;
            if (polyPoints[i].x > xRight)
                xRight = polyPoints[i].x;
            if (polyPoints[i].z < yBottom)
                yBottom = polyPoints[i].z;
            if (polyPoints[i].z > yTop)
                yTop = polyPoints[i].z;
        }
        
        bottomL = new Vector3(xLeft, height, yBottom);
        topR = new Vector3(xRight, height, yTop);
    }
}
