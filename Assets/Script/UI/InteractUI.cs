using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractUI : MonoBehaviour
{
    [SerializeField] RectTransform UIRectTransform;
    [SerializeField] new Camera camera;
    [SerializeField] Animator animator;
    [SerializeField] Image actingImage;
    [SerializeField] List<InteractTypeSprite> interactTypeIcons;
    Dictionary<Interactable.InteractType,Sprite> _interactTypeIconsDictionary = new Dictionary<Interactable.InteractType, Sprite>();
    public bool visible{
        get{
            return animator.GetBool("isVisible");
        }
        set{
            animator.SetBool("isVisible",value);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        foreach (InteractTypeSprite item in interactTypeIcons){
            _interactTypeIconsDictionary[item.interactType] = item.sprite;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveUIPositionFromTransform(Transform targetTransform){
        Vector2 ViewportPosition=camera.WorldToScreenPoint(targetTransform.position);
        UIRectTransform.position = new Vector3(ViewportPosition.x,ViewportPosition.y,0);
    }

    public void UpdateIcon(Interactable.InteractType interactType){
        actingImage.sprite = _interactTypeIconsDictionary[interactType];
    }
}

[System.Serializable]
public class InteractTypeSprite{
    public Interactable.InteractType interactType;
    public Sprite sprite;
}