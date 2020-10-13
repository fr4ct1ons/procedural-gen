using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Wave", menuName = "Spawner Wave")]
public class SpawnerWave : ScriptableObject
{
    [SerializeField] private bool raycastToBottom;
    [SerializeField] GameObject[] elementsToSpawn;
    [SerializeField] int[] amountToSpawn;
    [SerializeField] float[] timeBetweenSpawn;
    [SerializeField] public float maxRotation = 10.0f;

    public bool RaycastToBottom => raycastToBottom;
    
    public int NumOfElements() { return elementsToSpawn.Length; }
    public GameObject GetElement(int pos) { return elementsToSpawn[pos]; }
    public float GetSpawnInterval(int pos) { return timeBetweenSpawn[pos]; }
    public int GetSpawnAmount(int pos) { return amountToSpawn[pos]; }
}