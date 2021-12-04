using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 인게임에서 보이는 건물의 GameObject
 */
public class BuildingObject : MonoBehaviour
{
    // public BuildingPreset buildingPreset;
    public BuildingData buildingData;
    public SpriteRenderer spriteRenderer;
    public GameObject spriteObject;
    public BoxCollider boxCollider;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize(BuildingData buildingData){
        BuildingPreset buildingPreset = GameManager.Instance.buildingManager.GetBuildingPreset(buildingData.buildingCode);
        this.buildingData = buildingData;
        this.transform.localScale = buildingPreset.scale;
        spriteRenderer.sprite = buildingPreset.sprite;
    }

}
