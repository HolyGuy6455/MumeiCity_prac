using UnityEngine;
using UnityEngine.UI;

public class Compass : MonoBehaviour{
    [SerializeField] RectTransform niddleTransform;
    [SerializeField] GameObject startObject;
    [SerializeField] GameObject endObject;
    private void Update() {
        Vector3 diffVector = endObject.transform.position - startObject.transform.position;
        diffVector.y = 0;
        diffVector = diffVector.normalized;
        Debug.Log("diffVector - " + diffVector);
        float rotationValue = Mathf.Atan2(diffVector.x,diffVector.z);
        rotationValue *= -180/Mathf.PI;
        niddleTransform.rotation = Quaternion.Euler(0, 0, rotationValue);
        
    }
}
