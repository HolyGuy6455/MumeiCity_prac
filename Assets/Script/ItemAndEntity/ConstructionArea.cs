using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ConstructionArea : MonoBehaviour
{
    public Transform followTransform;
    public SpriteRenderer sprite;
    [SerializeField] private List<Collider> colliderOverlaped = new List<Collider>();
    [SerializeField] List<string> overlapTag = new List<string>();
    [SerializeField] BoxCollider boxCollider;
    private BuildingPreset buildingData;
    public UnityEvent eventCallBack;
    public bool show;

    public bool isThereObstacle(){
        bool result = (colliderOverlaped.Count != 0);
        if(result){
            sprite.color = new Color(1.0f,0.5f,0.5f,0.5f);
        }else{
            sprite.color = new Color(1.0f,1.0f,1.0f,0.5f);
        }
        return result;
    }

    // Update is called once per frame
    void Update(){
        Vector3 newPosition = new Vector3();
        newPosition.x = Mathf.Round(followTransform.position.x + ((buildingData is null)?0:buildingData.relativeLocation.x));
        newPosition.y = Mathf.Round(followTransform.position.y + ((buildingData is null)?0:buildingData.relativeLocation.y));
        newPosition.z = Mathf.Round(followTransform.position.z + ((buildingData is null)?0:buildingData.relativeLocation.z));
        this.transform.position = newPosition;
    }

    public void SetBuildingData(BuildingPreset buildingData){
        this.buildingData = buildingData;
        if(buildingData == null || !show){
            sprite.sprite = null;
            this.transform.localScale = new Vector3(1,1,1);
        }else{
            sprite.sprite = buildingData.sprite;
            this.transform.localScale = buildingData.scale;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(overlapTag.Contains(other.tag)){
            colliderOverlaped.Add(other);
            eventCallBack.Invoke();
        }
    }

    private void OnTriggerExit(Collider other) {
        if(overlapTag.Contains(other.tag)){
            colliderOverlaped.Remove(other);
            eventCallBack.Invoke();
        }
    }
}
