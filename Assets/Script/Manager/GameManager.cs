using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public delegate void UpdateUI();
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
    public Inventory inventory;
    [HideInInspector] public BuildingManager buildingManager;
    [HideInInspector] public ItemManager itemManager;
    [HideInInspector] public PeopleManager peopleManager;
    [HideInInspector] public CameraManager cameraManager;
    public Animator inventoryAnimator;
    public Animator buildingAnimator;
    public Animator pauseAnimator;
    bool gameIsPause = false;
    public GameObject itemPickupParent;
    public Interactable nearestInteractable;
    // [SerializeField] Interactable _interactableClosest;
    // public Interactable interactableClosest{get{return _interactableClosest;}}
    // public List<Interactable> interactableList = new List<Interactable>();
    [SerializeField] InteractUI interactUI;
    public enum GameTab
    {
        NORMAL,
        ITEM,
        BUILDING
    }
    public GameTab presentGameTab;
    private GameTab pastGameTab;
    [SerializeField] Sence _sence;
    public Sence sence{get{return _sence;}}
    public static GameManager Instance{
        get{
            return singleton_instance;
        }
    }

    private void Start() {
        buildingManager = this.GetComponent<BuildingManager>();
        itemManager = this.GetComponent<ItemManager>();
        peopleManager = this.GetComponent<PeopleManager>();
        cameraManager = this.GetComponent<CameraManager>();

        _sence.filter = delegate(GameObject gameObject){
            if(gameObject == null){
                return false;
            }
            if(gameObject.tag != "Interactable"){
                return false;
            }
            Interactable interactable =  gameObject.GetComponent<Interactable>();
            return interactable != null;
        };
    }

    private void OnValidate () {
        if(presentGameTab != pastGameTab){
            GameTabChanged();
        }
    }

    private void Update() {
        cameraManager.CameraZoom();

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
            if(presentGameTab != GameTab.BUILDING){
                presentGameTab = GameTab.BUILDING;
            }else{
                presentGameTab = GameTab.NORMAL;
            }
            GameTabChanged();
        }
        if(Input.GetButtonDown("Menu")){
            gameIsPause = !gameIsPause;
            pauseAnimator.SetBool("isVisible",gameIsPause);
            Time.timeScale = gameIsPause? 0:1;
            playerMovement.enabled = !gameIsPause;
        }
        if(Input.GetButtonDown("View")){
            Debug.Log("View");
        }

        GameObject nearestInteractableObject = _sence.FindNearest(PlayerTransform.position);
        if(nearestInteractableObject != null){
            interactUI.MoveUIPositionFromTransform(nearestInteractableObject.transform);
            interactUI.visible = true;
            nearestInteractable = nearestInteractableObject.GetComponent<Interactable>();
            interactUI.UpdateIcon(nearestInteractable.interactType);
        }else{
            interactUI.visible = false;
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

    // public void AddInteractable(Interactable interactable){
    //     if(interactableList.Contains(interactable)){
    //         return;
    //     }
    //     interactableList.Add(interactable);
    // }


    // public void RemoveInteractable(Interactable interactable){
    //     interactableList.Remove(interactable);
    // }
}
