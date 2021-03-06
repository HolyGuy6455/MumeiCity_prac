using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public delegate void UpdateUI();
    public LinkedList<Interactable> interactableList = new LinkedList<Interactable>();
    
    private static GameManager singleton_instance = null;
    public Transform PlayerTransform;
    /*
     * selectedTool
     * [0] = axe
     * [1] = lantern
     * [2] = shovel
     */
    public List<Tool> tools = new List<Tool>();
    private int selectedTool = 0;
    public SpriteRenderer ToolView;
    public PlayerMovement playerMovement;
    public InventoryManager inventoryManager;
    public BuildingManager buildingManager;
    public Animator inventoryAnimator;
    public Animator buildingAnimator;
    public enum GameTab
    {
        NORMAL,
        ITEM,
        BUILDING
    }
    public GameTab presentGameTab;
    private GameTab pastGameTab;

    public static GameManager Instance{
        get{
            return singleton_instance;
        }
    }

    private void OnValidate () {
        if(presentGameTab != pastGameTab){
            GameTabChanged();
        }
    }

    private void GameTabChanged(){
        bool playerMovementEnabled = false;
        bool inventoryShow = false;
        bool buildingShow = false;
        
        pastGameTab = presentGameTab;
        
        switch (presentGameTab)
        {
            case GameTab.NORMAL:
                playerMovementEnabled = true;
                break;

            case GameTab.ITEM:
                inventoryShow = true;
                break;

            case GameTab.BUILDING:
                playerMovementEnabled = true;
                buildingShow = true;
                if(buildingManager.onToolChangedCallback != null){
                    buildingManager.onToolChangedCallback.Invoke();
                }
                break;

            default:
                break;
        }

        playerMovement.enabled = playerMovementEnabled;
        inventoryAnimator.SetBool("Show",inventoryShow);
        buildingAnimator.SetBool("Show",buildingShow);
        if(!buildingShow){
            buildingManager.constructionArea.SetBuildingData(null);
        }
    }

    private void Update() {
        if(Input.GetButtonDown("Inventory")){
            if(presentGameTab != GameTab.ITEM){
                presentGameTab = GameTab.ITEM;
            }else{
                presentGameTab = GameTab.NORMAL;
            }
            GameTabChanged();
        }

        if(Input.GetButtonDown("ChangeTool")){
            SelectToolNext();
            if(buildingManager.onToolChangedCallback != null){
                buildingManager.onToolChangedCallback.Invoke();
            }
        }
        
        if(Input.GetButtonDown("Building")){
            // GameManager.Instance.BuildTent();
            if(presentGameTab != GameTab.BUILDING){
                presentGameTab = GameTab.BUILDING;
            }else{
                presentGameTab = GameTab.NORMAL;
            }
            GameTabChanged();
        }

    }

    public Tool GetToolNowHold(){
        return tools[this.selectedTool];
    }

    public void SelectTool(int index){
        if(index < 0 || index > tools.ToArray().Length -1){
            Debug.Log("Wrong Tool Index!");
            return;
        }
        this.selectedTool = index;
        ToolView.sprite = tools[index].icon;
    }

    public void SelectToolNext(){
        if(selectedTool > tools.ToArray().Length -2){
            SelectTool(0);
        }else{
            SelectTool(selectedTool+1);
        }
        
    }

    private void Awake() {
        singleton_instance = this;
    }

    public void AddInteractable(Interactable interactable){
        interactableList.AddLast(interactable);
    }


    public void RemoveInteractable(Interactable interactable){
        interactableList.Remove(interactable);
    }

    public Interactable GetFirstInteractable(){
        if(interactableList.Count < 1){
            return null;
        }
        return interactableList.First.Value;
    }
}
