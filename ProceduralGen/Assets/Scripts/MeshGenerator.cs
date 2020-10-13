using System;
using UnityEngine;

public static class MeshGenerator
{
    public static GeneratedMeshData GenerateTerrainMesh(float[,] heightMap, AnimationCurve heightCurve, float heightMultiplier = 1.0f)
    {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        float topLeftX = (width - 1) / -2.0f;
        float topLeftZ = (height - 1) / 2.0f;
        
        GeneratedMeshData data = new GeneratedMeshData(width, height);
        int vertexIndex = 0;
        
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                data.vertices[vertexIndex] = new Vector3(topLeftX + x, heightCurve.Evaluate(heightMap[x, y]) * heightMultiplier, topLeftZ - y);
                data.uvs[vertexIndex] = new Vector2(x / (float) width, y / (float) height);
                if (x < width - 1 && y < height - 1)
                {
                    data.AddTriangle(vertexIndex, vertexIndex + width + 1, vertexIndex + width);
                    data.AddTriangle(vertexIndex + width + 1, vertexIndex, vertexIndex + 1);
                }

                vertexIndex++;
            }
        }

        return data;
    }
}

public class GeneratedMeshData
{
    public Vector3[] vertices;
    public int[] triangles;
    public Vector2[] uvs;

    private int triangleIndex = 0;

    public GeneratedMeshData(int meshWidth, int meshHeight)
    {
        vertices = new Vector3[meshWidth * meshHeight];
        triangles = new int[(meshWidth - 1) * (meshHeight - 1) * 6];
        uvs = new Vector2[meshWidth * meshHeight];
    }

    public void AddTriangle(int a, int b, int c)
    {
        triangles[triangleIndex] = a;
        triangles[triangleIndex + 1] = b;
        triangles[triangleIndex + 2] = c;

        triangleIndex += 3;
    }

    public Mesh BuildMesh()
    {
        Mesh mesh = new Mesh();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        
        mesh.RecalculateNormals();

        return mesh;
    }
}


public static class Noise
{
    public static float[,] GenerateNoiseMap(int width, int height, float noiseScale, int octaves, float persistence,
        float lacunarity, int seed=0, Vector2 noiseOffset = new Vector2())
    {
        float[,] map = new float[width, height];
        float minHeight = float.MaxValue;
        float maxHeight = float.MinValue;
        
        System.Random rng = new System.Random(seed);
        
        Vector2[] octaveOffsets = new Vector2[octaves];

        for (int i = 0; i < octaveOffsets.Length; i++)
        {
            float offsetX = rng.Next(-100000, 100000) + noiseOffset.x;
            float offsetY = rng.Next(-100000, 100000) + noiseOffset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        if (noiseScale <= 0)
            noiseScale = 0.00001f;
        
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float amplitude = 1;
                    float frequency = 1;
                    float noiseHeight = 0;
                    for (int o = 0; o < octaves; o++)
                    {
                        float sampleX = x / noiseScale * frequency + octaveOffsets[o].x;
                        float sampleY = y / noiseScale * frequency + octaveOffsets[o].y;
                        
                        float perlin = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                        noiseHeight += perlin * amplitude;

                        amplitude *= persistence;
                        frequency *= lacunarity;
                    }

                    if (noiseHeight > maxHeight)
                        maxHeight = noiseHeight;
                    else if (noiseHeight < minHeight)
                        minHeight = noiseHeight;
                    map[x, y] = noiseHeight ;
                }
            }
        }

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < height; x++)
            {
                map[x, y] = Mathf.InverseLerp(minHeight, maxHeight, map[x, y]);
            }
        }

        return map;
    }
}