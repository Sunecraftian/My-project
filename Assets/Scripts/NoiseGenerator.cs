using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NoiseGenerator {

    public static float OGet2DPerlin(Vector2 position, float offset, float scale) {
        return Mathf.PerlinNoise((position.x + 0.1f) / VoxelData.chunkSize * scale + offset, (position.y + 0.1f) / VoxelData.chunkSize * scale + offset);
    }


    // public static float Get2DPerlin(Vector2 position, int seed, float scale, int octaves, float persistence, float lacunarity, float offset) {
    //     return Mathf.PerlinNoise((position.x + 0.1f) / scale * lacunarity + offset, (position.y + 0.1f) / scale * lacunarity + offset) * persistence;
    
    // }

    // public static float Get2DPerlin(int seed, float scale, int octaves, float persistence, float lacunarity, Vector2 offset) {

    //     float total = 0;
    //     float max = 0;
    //     float frequency = 1;
    //     float amplitude = 1;

    //     for (int i = 0; i < octaves; i++) {

    //         total += Mathf.PerlinNoise((offset.x + 0.1f) / scale * frequency + seed, (offset.y + 0.1f) / scale * frequency + seed) * amplitude;

    //         max += amplitude;

    //         amplitude *= persistence;
    //         frequency *= lacunarity;
    //     }

    //     return total / max;
    // }

    // public static float Get2DPerlin(int seed, float scale, int octaves, float persistence, float lacunarity, float offset) {
    //     float noise = 0;

    //     System.Random prng = new System.Random(seed); 
    //     for (int i = 0; i < octaves; i++) {
            
    //         offset += prng.Next(-100000, 100000); 
    //     }

    //     if (scale <= 0) scale = 0.0001f;

    //     float maxNoiseHeight = float.MinValue; 
    //     float minNoiseHeight = float.MaxValue; 

        

    //     for (int y = 0; y < World.worldSize; y++)
    //     {
    //         for (int x = 0; x < World.worldSize; x++)
    //         {

    //             float amplitude = 1;
    //             float frequency = 1;
    //             float noiseHeight = 0;

    //             for (int i = 0; i < octaves; i++) {
    //                 float sampleX = x / scale * frequency + offset;
    //                 float sampleY = y / scale * frequency + offset;

    //                 float perlinValue = Mathf.PerlinNoise(sampleX, sampleY); 
    //                 noiseHeight += perlinValue * amplitude;

    //                 amplitude *= persistence;
    //                 frequency *= lacunarity;

    //             }

    //             // Update Min and Max Noise Height
    //             if (noiseHeight > maxNoiseHeight) 
    //                 maxNoiseHeight = noiseHeight;
    //             else if (noiseHeight < minNoiseHeight)
    //                 minNoiseHeight = noiseHeight;

    //             noise = noiseHeight;

    //         }
    //     }

        
    //     noise = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noise);
            
    

    //     return noise;
    // }
}