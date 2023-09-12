using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public Sprite seaTile;
    public Sprite sandTile;
    public Sprite greenTile;
    public Sprite noiseSprite;

    public Texture2D noiseTexture;
    public Texture2D waterHeightmapTexture;

    public Tilemap waterTilemap;
    public RuleTile waterRuletile;

    public Material testMaterial;

    public GameObject go;
    public GameObject waterGO;

    private int gridWidth;
    private int gridHeight;
    private int gridSize;
    private float zoomFactor;

    private float[,] perlinValue;

    void Start()
    {
        //gridWidth = 300;
        //gridHeight = 300;
        gridSize = 400;
        zoomFactor = 100.0f;

        perlinValue = new float[gridSize, gridSize];
        noiseTexture = new Texture2D(gridSize, gridSize);
        waterHeightmapTexture = new Texture2D(gridSize, gridSize);
        noiseTexture.filterMode = FilterMode.Point;
        //waterHeightmapTexture.filterMode = FilterMode.Point;

        for(int x = 0; x < noiseTexture.width; x++)
        {
            for(int y = 0; y < noiseTexture.height; y++)
            {
                float noise = (Perlin.Fbm(new Vector2(x / zoomFactor, y / zoomFactor), 12) + 1 ) / 2;
                float c = Circular((float)x, (float)y, gridSize, gridSize * 2 /3);
                noise -= c;
                perlinValue[x, y] = noise;

                float min_limit = 0.2f;
                float water_limit = 0.5f;
                float sand_limit = 0.53f;
                float grass_limit = 0.8f;

                waterHeightmapTexture.SetPixel(x, y, Color.white);
                // water limit
                if ( noise > min_limit && noise <= water_limit)
                {
                    float t = (noise - min_limit) / (water_limit - min_limit);
                    float r = Mathf.Lerp(30, 40, t);
                    float g = Mathf.Lerp(176, 255, t);
                    float b = Mathf.Lerp(251, 255, t);
                    noiseTexture.SetPixel(x, y, new Color( (float)r/255, (float)g/255, (float)b/255, 1));  //water		
                    float lerpV = (water_limit - noise) / (water_limit - min_limit);
                    //waterHeightmapTexture.SetPixel(x, y, Color.white * lerpV);
                    waterHeightmapTexture.SetPixel(x, y, Color.white * noise);
                    waterTilemap.SetTile(new Vector3Int(x, y, 0), waterRuletile);
                }
                else if (noise > water_limit && noise <= sand_limit)
                {
                    noiseTexture.SetPixel(x, y, new Color( (float)255/255, (float)246/255, (float)193/255, 1)); //sand		
                }
                else if (noise > sand_limit && noise <= grass_limit)
                {
                    noiseTexture.SetPixel(x, y, new Color( (float)118/255, (float)239/255, (float)124/255, 1)); //grass
                }
                else
                {
                    //noiseTexture.SetPixel(x, y, new Color( (float)245/255, (float)250/255, (float)245/255, 1)); //snow
                }
            }
        }
        noiseTexture.Apply();
        waterHeightmapTexture.Apply();
        //go.GetComponent<SpriteRenderer>().material.SetTexture("_HeightTexture", waterHeightmapTexture);
        waterTilemap.GetComponent<TilemapRenderer>().material.SetTexture("_HeightTexture", waterHeightmapTexture);
    }

    // Update is called once per frame
    void Update()
    {
        testMaterial.SetTexture("HeightTexture", noiseTexture);
    }

    float Circular(float x, float y, int size, float islandSize)
    {
        float gradiant = 1f;
        gradiant /= (x * y) / (size * size) * (1 - (x / size)) * (1 - (y / size));
        gradiant -= 16.0f;
        gradiant /= islandSize;
        return gradiant;
    }
}
