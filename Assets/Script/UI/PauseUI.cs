using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PauseUI : MonoBehaviour{

    [SerializeField] Animator animator;
    Dictionary<string, int> orderIndex;
    [SerializeField] Text saveTitle_1;
    [SerializeField] Text saveTitle_2;
    [SerializeField] Text saveTitle_3;
    [SerializeField] List<string> orderList;
    [SerializeField] string orderStr;
    [SerializeField] Image mousePointerImage;

    private void Awake() {
        orderIndex = new Dictionary<string, int>();
        for (int i = 0; i < orderList.Count; i++){
            orderIndex[orderList[i]] = i;
        }
    }

    public void SetOrder(string orderStr){
        if(orderIndex.ContainsKey(orderStr)){
            animator.SetInteger("Order",orderIndex[orderStr]);
        }else{
            animator.SetInteger("Order",0);
        }
        this.orderStr = orderStr;
    }

    public void UpdateUI(){
        SaveLoadManager.SaveMetaFile saveMetaFile = GameManager.Instance.saveLoadManager._saveMetaFile;
        saveTitle_1.text = saveMetaFile.metaName[1];
        saveTitle_2.text = saveMetaFile.metaName[2];
        saveTitle_3.text = saveMetaFile.metaName[3];
    }

    public void Pause(){
        animator.SetBool("isVisible",true);
        Time.timeScale = 0;
        GameManager.Instance.playerMovement.enabled = false;
        GameManager.Instance.playerInput.SwitchCurrentActionMap("Pause");
        Cursor.visible = true;
        mousePointerImage.enabled = false;
    }

    public void PauseCancle(){
        animator.SetBool("isVisible",false);
        Time.timeScale = 1;
        GameManager.Instance.playerMovement.enabled = true;
        GameManager.Instance.playerInput.SwitchCurrentActionMap("PlayerControl");
        Cursor.visible = false;
        mousePointerImage.enabled = true;
    }

    public void OnExitPause(InputAction.CallbackContext value){
        if(value.performed){
            OnExitPause();
        }
    }

    public void OnExitPause(){
        if(orderStr == "Root"){
            PauseCancle();
        }else{
            SetOrder("Root");
        }
    }
    
}
