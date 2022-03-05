using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour{

    [SerializeField] Animator animator;
    Dictionary<string, int> orderIndex;
    [SerializeField] Text saveTitle_1;
    [SerializeField] Text saveTitle_2;
    [SerializeField] Text saveTitle_3;
    [SerializeField] List<string> order;

    private void Awake() {
        orderIndex = new Dictionary<string, int>();
        for (int i = 0; i < order.Count; i++){
            orderIndex[order[i]] = i;
        }
    }

    public void SetOrder(string orderStr){
        if(orderIndex.ContainsKey(orderStr)){
            animator.SetInteger("Order",orderIndex[orderStr]);
        }else{
            animator.SetInteger("Order",0);
        }
    }

    public void UpdateUI(){
        SaveLoadManager.SaveMetaFile saveMetaFile = GameManager.Instance.saveLoadManager.saveMetaFile;
        saveTitle_1.text = saveMetaFile.metaName[1];
        saveTitle_2.text = saveMetaFile.metaName[2];
        saveTitle_3.text = saveMetaFile.metaName[3];
    }
    
}
