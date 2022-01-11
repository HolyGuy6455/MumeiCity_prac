using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionArea : MonoBehaviour
{
    public Transform followTransform;
    public GameObject sprite;
    [SerializeField] private List<Collider> colliderOverlaped = new List<Collider>();
    private List<string> overlapTag = new List<string>();
    private BoxCollider boxCollider;
    private BuildingPreset buildingData;

    public bool isThereObstacle(){
        return (colliderOverlaped.Count != 0);
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (string overlapTagString in new[]{"Building","Wall"}){
            overlapTag.Add(overlapTagString);
        }
        boxCollider = this.GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition = new Vector3();
        newPosition.x = Mathf.Round(followTransform.position.x + ((buildingData is null)?0:buildingData.relativeLocation.x));
        newPosition.y = Mathf.Round(followTransform.position.y + ((buildingData is null)?0:buildingData.relativeLocation.y));
        newPosition.z = Mathf.Round(followTransform.position.z + ((buildingData is null)?0:buildingData.relativeLocation.z));
        this.transform.position = newPosition;
    }

    public void SetBuildingData(BuildingPreset buildingData){
        this.buildingData = buildingData;
        SpriteRenderer spriteRenderer = sprite.GetComponent<SpriteRenderer>();
        if(buildingData == null){
            spriteRenderer.sprite = null;
            this.transform.localScale = new Vector3(1,1,1);
        }else{
            spriteRenderer.sprite = buildingData.sprite;
            this.transform.localScale = buildingData.scale;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(overlapTag.Contains(other.tag)){
            colliderOverlaped.Add(other);
            Debug.Log("OnTriggerEnter");
        }
    }

    private void OnTriggerExit(Collider other) {
        if(overlapTag.Contains(other.tag)){
            colliderOverlaped.Remove(other);
            Debug.Log("OnTriggerExit");
        }
    }
}
