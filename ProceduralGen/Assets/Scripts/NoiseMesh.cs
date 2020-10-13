using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

public class NoiseMesh : MonoBehaviour
{

    [SerializeField] private new MeshRenderer renderer;
    [SerializeField] private MeshFilter filter;
    [SerializeField] private bool autoUpdateMesh;
    [SerializeField] private bool generateMeshOnAwake;
    
    [Space] 
    [SerializeField] private float noiseScale;
    [SerializeField] private int octaves;
    [SerializeField] private float persistence;
    [SerializeField] private float lacunarity;
    [SerializeField] private float heightMultiplier = 1.0f;
    [SerializeField] private int seed;
    [SerializeField] private Vector2 offset = Vector2.zero;
    [SerializeField] private AnimationCurve heightMultiplierCurve;
    
    [Header("Setting any of the values below to 0 will not generate anything.")]
    [SerializeField] private int xSize = 1;
    [SerializeField] private int zSize = 1;
    [SerializeField] private float xSpacing = 1.0f, zSpacing = 1.0f;

    private Mesh mesh;
    private List<Vector3> vertices;
    private int[] triangles;
    
    public bool AutoUpdateMesh => autoUpdateMesh;

    private void Awake()
    {
        if(generateMeshOnAwake)
            GenerateMesh();
    }

    [ContextMenu("Generate mesh")]
    public void GenerateMesh()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(xSize, zSize, noiseScale, octaves, persistence, lacunarity, seed, offset);
        
        mesh = MeshGenerator.GenerateTerrainMesh(noiseMap, heightMultiplierCurve, heightMultiplier).BuildMesh();

        filter.mesh = mesh;
    }

    public float GetXLength()
    {
        return xSize * xSpacing;
    }

    public float GetZLength()
    {
        return zSize * zSpacing;
    }

    public Texture2D GenerateNoiseTexture()
    {
        float[,] pixels = Noise.GenerateNoiseMap(xSize, zSize, noiseScale, octaves, persistence, lacunarity);
        
        Texture2D tex = new Texture2D(xSize, zSize);
        
        Color[] colors = new Color[xSize * zSize];

        {
            int i = 0;
            for (int y = 0; y < zSize; y++)
            {
                for (int x = 0; x < xSize; x++)
                {
                    colors[i] = new Color(pixels[x,y], pixels[x,y], pixels[x,y]);
                    i++;
                }
            }
        }
        
        tex.SetPixels(colors);
        tex.Apply();

        return tex;
    }
}
