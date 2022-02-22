using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractUI : MonoBehaviour
{
    [SerializeField] RectTransform UIRectTransform;
    [SerializeField] new Camera camera;
    [SerializeField] Animator animator;
    [SerializeField] Image actingImage_1;
    [SerializeField] Image actingImage_2;
    [SerializeField] List<InteractTypeSprite> interactTypeIcons;
    Dictionary<Interactable.InteractType,InteractTypeSprite> _interactTypeIconsDictionary;
    public bool visible{
        get{
            return animator.GetBool("isVisible");
        }
        set{
            animator.SetBool("isVisible",value);
        }
    }
    // Start is called before the first frame update
    void Start(){
        _interactTypeIconsDictionary = new Dictionary<Interactable.InteractType, InteractTypeSprite>();
        foreach (InteractTypeSprite item in interactTypeIcons){
            _interactTypeIconsDictionary[item.interactType] = item;
        }
    }

    public void MoveUIPositionFromTransform(Transform targetTransform){
        Vector2 ViewportPosition=camera.WorldToScreenPoint(targetTransform.position);
        UIRectTransform.position = new Vector3(ViewportPosition.x,ViewportPosition.y,0);
    }

    public void UpdateIcon(Interactable.InteractType interactType){
        InteractTypeSprite spriteSet = _interactTypeIconsDictionary[interactType];
        actingImage_1.sprite = spriteSet.actingSprite_1;
        actingImage_2.sprite = spriteSet.actingSprite_2;
    }
}

[System.Serializable]
public class InteractTypeSprite{
    public Interactable.InteractType interactType;
    public Sprite actingSprite_1;
    public Sprite actingSprite_2;
}