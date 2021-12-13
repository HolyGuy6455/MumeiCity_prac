using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 인게임에서 보이는 건물의 GameObject
 */
public class BuildingObject : MonoBehaviour
{
    public BuildingData buildingData;
    public SpriteRenderer spriteRenderer;
    public GameObject spriteObject;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize(BuildingData buildingData){
        BuildingPreset buildingPreset = GameManager.Instance.buildingManager.GetBuildingPreset(buildingData.code);
        this.buildingData = buildingData;
        Debug.Log("Scale!");
        Debug.Log(this.transform.localScale);
        Debug.Log(buildingPreset.scale);
        this.transform.localScale = buildingPreset.scale;
        spriteRenderer.sprite = buildingPreset.sprite;
    }

}
