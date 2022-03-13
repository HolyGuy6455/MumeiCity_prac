using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

/*
 * 인게임에서 보이는 건물의 GameObject
 */
public class GridMapManager : MonoBehaviour{
    [SerializeField] Tilemap invisibleTilemap;
    [SerializeField] List<GroundLevelIndex> groundLevelIndexList;
    [SerializeField] GameObject defaultGroundPrefab;
    [SerializeField] GameObject groundParent;
    [SerializeField] bool visible;
    bool visibleBefore;
    public Tilemap _invisibleTilemap{get{return invisibleTilemap;}}
    private Dictionary<Tile, GroundLevel> groundDictionary;
    public float floorHeight = 1.0f;
    TileBase[] allTiles;
    int xMax = 3;
    int yMax = 3;
    [SerializeField] bool[,] waterArray;
    [SerializeField] float[,] heightArray;

    public enum GroundLevel{
        B1,
        F1,
        F2,
        F3,
        STAIR,
        WATER
    }
    [Serializable]
    public class GroundLevelIndex{
        public GroundLevel groundLevel;
        public Tile tile;
    }

    private void Update() {
        if(visible != visibleBefore){
            visibleBefore = visible;
            foreach (Transform child in groundParent.transform){
                child.GetComponent<MeshRenderer>().enabled = visible;
            }
        }
    }

    private void Awake() {
        groundDictionary = new Dictionary<Tile, GroundLevel>();
        foreach (GroundLevelIndex groundLevelIndex in groundLevelIndexList){
            groundDictionary[groundLevelIndex.tile] = groundLevelIndex.groundLevel;
        }
    }

    private void Start() {
        BoundsInt bounds = invisibleTilemap.cellBounds;
        allTiles = invisibleTilemap.GetTilesBlock(bounds);
        xMax = bounds.size.x;
        yMax = bounds.size.y;
        waterArray = new bool[xMax,yMax];
        heightArray = new float[xMax,yMax];

        for (int x = 0; x < xMax; x++){
            for (int y = 0; y < yMax; y++){
                Vector3 location = new Vector3(x,0,y);
                GameObject newGroundObject = Instantiate(defaultGroundPrefab,location,Quaternion.identity);
                newGroundObject.transform.SetParent(groundParent.transform);
                float defaultLevel = GetFloorHeight(x,y,true);
                newGroundObject.transform.Translate(0,defaultLevel,0);
                if(GetGroundLevel(x,y) == GroundLevel.STAIR){
                    newGroundObject.transform.Rotate(0.1f,0.0f,0.0f);
                }
                waterArray[x,y] = GetGroundLevel(x,y) == GroundLevel.WATER;
                heightArray[x,y] = defaultLevel;
            }
        }
        Vector3 origin = invisibleTilemap.origin;
        origin.x += 0.5f;
        origin.z = origin.y * 1.5f + 2.5f;
        origin.y = -3.5f;
        groundParent.transform.Translate(origin);
        groundParent.transform.localScale = new Vector3(1.0f,1.0f,1.5f);
    }

    private GroundLevel GetGroundLevel(int x, int y){
        TileBase tile = allTiles[x + y * xMax];
        GroundLevel groundLevel = GroundLevel.B1;
        if(tile != null){
            groundLevel = groundDictionary[tile as Tile];
        }
        return groundLevel;
    }

    private float GetFloorHeight(int x, int y, bool recurrence){
        float result = 0.0f;
        TileBase tile = allTiles[x + y * xMax];
        GroundLevel groundLevel = GroundLevel.B1;
        if(tile != null){
            groundLevel = groundDictionary[tile as Tile];
        }
        switch (groundLevel){
            case GroundLevel.B1:
                result = 0.0f;
                break;
            case GroundLevel.F1:
                result = 1.0f;
                break;
            case GroundLevel.F2:
                result = 2.0f;
                break;
            case GroundLevel.F3:
                result = 3.0f;
                break;
            case GroundLevel.STAIR:
                if(recurrence){
                    Vector2Int[] neighbor = new Vector2Int[]{
                        new Vector2Int(-1,-2),
                        new Vector2Int(-1,-1),
                        new Vector2Int(-1, 0),
                        new Vector2Int(-1, 1),
                        new Vector2Int( 1,-2),
                        new Vector2Int( 1,-1),
                        new Vector2Int( 1, 0),
                        new Vector2Int( 1, 1),
                    };
                    foreach (Vector2Int nextDoor in neighbor){
                        result += GetFloorHeight(x+nextDoor.x,y+nextDoor.y,false);
                    }
                    result /= neighbor.Length;
                }else{
                    result = 0.0f;
                }
                
                break;
            case GroundLevel.WATER:
                if(recurrence){
                    Vector2Int[] neighbor = new Vector2Int[]{
                        new Vector2Int(-1,-1),
                        new Vector2Int(-1, 0),
                        new Vector2Int(-1, 1),
                        new Vector2Int( 1,-1),
                        new Vector2Int( 1, 0),
                        new Vector2Int( 1, 1),
                        new Vector2Int( 0, 1),
                        new Vector2Int( 0,-1),
                    };
                    foreach (Vector2Int nextDoor in neighbor){
                        result += GetFloorHeight(x+nextDoor.x,y+nextDoor.y,false);
                    }
                    result /= neighbor.Length;
                }else{
                    result = 0.4f;
                }
                break;
            default:
                break;
        }
        return result;
    }

    public bool amIInWater(Vector3 vector){
        Vector3 groundParentLocation = groundParent.transform.position;
        int x = (int)(vector.x - groundParentLocation.x);
        int y = (int)((vector.z - groundParentLocation.z)/1.5f);
        return waterArray[x,y];
    }

    public float amIInCave(Vector3 vector){
        Vector3 groundParentLocation = groundParent.transform.position;
        int x = (int)(vector.x - groundParentLocation.x);
        int y = (int)((vector.z - groundParentLocation.z)/1.5f);
        return heightArray[x,y];
    }
}