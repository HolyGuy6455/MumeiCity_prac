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
    [SerializeField] HitBoxCollision hitBoxCollision;
    public SpriteRenderer ToolView;
    public PlayerMovement playerMovement;
    public Inventory inventory;
    [HideInInspector] public BuildingManager buildingManager;
    [HideInInspector] public ItemManager itemManager;
    [HideInInspector] public PeopleManager peopleManager;
    [HideInInspector] public CameraManager cameraManager;
    [HideInInspector] public TimeManager timeManager;
    [SerializeField] private Animator taskUI;
    public Animator pauseAnimator;
    bool gameIsPause = false;
    public GameObject itemPickupParent;
    public Interactable nearestInteractable;
    public BuildingData interactingBuilding;
    [SerializeField] InteractUI interactUI;
    public enum GameTab
    {
        NORMAL,
        ITEM,
        BUILDING,
        FORESTER,
        TENT,
        FOODSTORAGE,
        MINE

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
        timeManager = this.GetComponent<TimeManager>();

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
            ChangeGameTab(presentGameTab);
        }
    }

    private void Update() {
        cameraManager.CameraZoom();

        if(Input.GetButtonDown("Inventory")){
            ChangeGameTab((presentGameTab != GameTab.ITEM) ? GameTab.ITEM : GameTab.NORMAL);
        }

        if(Input.GetButtonDown("ChangeTool")){
            SelectToolNext();
            if(buildingManager.onToolChangedCallback != null){
                buildingManager.onToolChangedCallback.Invoke();
            }
            hitBoxCollision.tool = GetToolNowHold().toolType;
        }
        
        if(Input.GetButtonDown("Building")){
            ChangeGameTab((presentGameTab != GameTab.BUILDING) ? GameTab.BUILDING : GameTab.NORMAL);
        }
        if(Input.GetButtonDown("Menu")){
            if(presentGameTab != GameTab.NORMAL){
                ChangeGameTab(GameTab.NORMAL);
            }else{
                gameIsPause = !gameIsPause;
                pauseAnimator.SetBool("isVisible",gameIsPause);
                Time.timeScale = gameIsPause? 0:1;
                playerMovement.enabled = !gameIsPause;
            }
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
            nearestInteractable = null;
            interactUI.visible = false;
        }
    }

    public void ChangeGameTab(GameTab gameTab){
        presentGameTab = gameTab;
        bool playerMovementEnabled = false;
        bool buildingShow = false;
        
        pastGameTab = presentGameTab;
        
        switch (presentGameTab)
        {
            case GameTab.NORMAL:
                playerMovementEnabled = true;
                taskUI.SetInteger("SelectedUI",0);
                break;

            case GameTab.ITEM:
                taskUI.SetInteger("SelectedUI",1);
                break;

            case GameTab.BUILDING:
                taskUI.SetInteger("SelectedUI",2);
                playerMovementEnabled = true;
                buildingShow = true;
                if(buildingManager.onToolChangedCallback != null){
                    buildingManager.onToolChangedCallback.Invoke();
                }
                break;

            case GameTab.FORESTER:
                taskUI.SetInteger("SelectedUI",3);
                break;

            case GameTab.TENT:
                taskUI.SetInteger("SelectedUI",4);
                break;

            case GameTab.FOODSTORAGE:
                taskUI.SetInteger("SelectedUI",5);
                break;

            case GameTab.MINE:
                taskUI.SetInteger("SelectedUI",6);
                break;

            default:
                break;
        }

        playerMovement.stop = !playerMovementEnabled;
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
