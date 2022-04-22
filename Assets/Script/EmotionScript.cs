using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmotionScript : MonoBehaviour{
    [SerializeField] float term = 1.0f;
    [SerializeField] int index;
    [SerializeField] List<Sprite> sprites;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] RectTransform backgroundTransform;
    [SerializeField] Text speechText;
    [SerializeField] Animator animator;
    /*
     *  표정을 여기에 정리해둬야지
     * 1 : confidence 자신만만
     * 2 : curious 호기심
     * 3 : lasstitude 느긋함
     * 4 : angry 화났음
     * 5 : sad 슬픔
     * 6 : shy 부끄러워
     * 7 : sly 음흉함
     * 8 : smile 웃음
     */
    
    // Start is called before the first frame update
    void Start(){
        // StartCoroutine("CountTime", term);
    }

    // Update is called once per frame
    void Update(){
        
    }

    public void SetFace(int index){
        spriteRenderer.sprite = sprites[index];
    }

    public void SetText(string text){
        speechText.text = text;

        float width = Mathf.Min(speechText.preferredWidth,240);
        float height = speechText.preferredHeight;
        backgroundTransform.sizeDelta = new Vector2(width+15,height+30);
    }

    public void SetTrigger(){
        Debug.Log("SetTrigger");
        animator.SetTrigger("EmotionTrigger");
        Debug.Log("SetTrigger");
    }

    public void Upgrade(string value){
        SetFace(8);
        SetText(value+" is Complete!");
        SetTrigger();
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
