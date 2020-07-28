using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int width;
    public int height;

    public string seed;
    public bool useRandomSeed;

    char [] characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();
    int desiredSeedLength = 16;

    [Range(0,100)]
    public int randomFillPercent;

    int[,] map;
    int[,] smoothedMap;

    void Start()
    {
        GenerateMap();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GenerateMap();
        }
    }

    void GenerateMap()
    {
        map = new int[width, height];
        RandomFillMap();

        for(int i = 0; i < 5; i++)
        {
            SmoothMap();
        }

        MeshGenerator meshGen = GetComponent<MeshGenerator>();
        meshGen.GenerateMesh(map, 1);
    }

    void RandomFillMap()
    {
        Debug.Log(Time.time);
        if (useRandomSeed)
        {
            seed = "";
            while(seed.Length < desiredSeedLength)
            {
                seed += characters[Random.Range(0, characters.Length)];
            }
        }

        System.Random pseudoRandom = new System.Random(seed.GetHashCode());

        for(int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if(x == 0 || x == width-1 || y == 0 || y == height - 1)
                {
                    map[x, y] = 1;
                }
                else
                {
                    map[x, y] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? 1 : 0;
                }
                
            }
        }

    }

    void SmoothMap()
    {
        smoothedMap = map;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighbourWallTiles = GetSurroundingWallCount(x, y);

                if(neighbourWallTiles > 4)
                {
                    smoothedMap[x, y] = 1;
                }
                else if (neighbourWallTiles < 4)
                {
                    smoothedMap[x, y] = 0;
                }
            }
        }
        map = smoothedMap;
    }

    int GetSurroundingWallCount(int gridX, int gridY)
    {
        int wallCount = 0;
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
        {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
            {
                if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height)
                {
                    if (neighbourX != gridX || neighbourY != gridY)
                    {
                        wallCount += map[neighbourX, neighbourY];
                    }
                }
                else
                {
                    wallCount++;
                }
            }

        }

        return wallCount;
    }

    void OnDrawGizmos()
    {
        /*
       if (map != null)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Gizmos.color = (map[x, y] == 1) ? Color.black : Color.white;
                    Vector3 pos = new Vector3(-width / 2 + x + .5f, 0, -height / 2 + y + .5f);
                    Gizmos.DrawCube(pos, Vector3.one);
                }
            }
        } 
        */
    }


}
