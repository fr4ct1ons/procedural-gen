using System;
using UnityEngine;


public class TerrainGenerator : MonoBehaviour
{
    [SerializeField] private Terrain terrain;
    [SerializeField] private bool autoUpdateMesh = false;

    [Space] 
    [SerializeField] private float noiseScale = 1.1f;
    [SerializeField] private int octaves = 2;
    [SerializeField] private float persistence = 0.75f;
    [SerializeField] private float lacunarity = 2.0f;
    [SerializeField] private float heightMultiplier = 1.0f;
    [SerializeField] private int seed;
    [SerializeField] private Vector2 offset = Vector2.zero;
    [SerializeField] private AnimationCurve heightCurve;

    [Header("Setting any of the values below to 0 will not generate anything.")]
    [SerializeField] private int xSize = 1;
    [SerializeField] private float ySize = 1;
    [SerializeField] private int zSize = 1;

    public int XSize
    {
        get => xSize;
        set => xSize = value;
    }

    public int ZSize
    {
        get => zSize;
        set => zSize = value;
    }

    public bool AutoUpdateMesh => autoUpdateMesh;

    [ContextMenu("Generate terrain")]
    public void GenerateNewTerrain()
    {
        terrain.terrainData = GenerateTerrain(terrain.terrainData);
    }

    public TerrainData GenerateTerrain(TerrainData baseData)
    {
        baseData.heightmapResolution = xSize + 1;
        
        baseData.size = new Vector3(xSize, ySize, zSize);

        var heightMap = Noise.GenerateNoiseMap(xSize, zSize, noiseScale, octaves, persistence, lacunarity, seed, offset);
        for (int y = 0; y < zSize; y++)
        {
            for (int x = 0; x < xSize; x++)
            {
                heightMap[x, y] *= heightCurve.Evaluate(heightMap[x, y]) * heightMultiplier;
            }
        }

        baseData.SetHeights(0, 0, heightMap);

        return baseData;
    }

    private void OnValidate()
    {
        if (autoUpdateMesh)
        {
            GenerateNewTerrain();
        }
    }
}
