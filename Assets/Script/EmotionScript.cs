using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionScript : MonoBehaviour{
    [SerializeField] float term = 1.0f;
    [SerializeField] int index;
    [SerializeField] List<Sprite> sprites;
    [SerializeField] SpriteRenderer spriteRenderer;
    
    // Start is called before the first frame update
    void Start(){
        // StartCoroutine("CountTime", term);
    }

    // Update is called once per frame
    void Update(){
        
    }

    IEnumerator CountTime(float delayTime) {
        index++;
        if(index >= sprites.Count){
            index = 0;
        }
        spriteRenderer.sprite = sprites[index];
        yield return new WaitForSeconds(delayTime);
        StartCoroutine("CountTime", term);
   }
}
