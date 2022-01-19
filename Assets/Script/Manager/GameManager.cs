using System;
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
    [SerializeField] int selectedTool = 0;
    [SerializeField] HitCollision hitCollision;
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
    public Sprite emptySprite;
    public enum GameTab
    {
        NORMAL,
        ITEM,
        BUILDING,
        FORESTER_HUT,
        TENT,
        FOODSTORAGE,
        MINE

    }
    public GameTab presentGameTab;
    private GameTab pastGameTab;
    public BuildingData interactingBuilding;
    public Interactable nearestInteractable;
    public PersonBehavior nearestPerson;
    [SerializeField] InteractUI interactUI;
    [SerializeField] PersonInfoUI personInfoUI;
    
    [SerializeField] Sence sence;
    public Sence sence_{get{return sence;}}
    Predicate<GameObject> interactableSenseFilter;
    Predicate<GameObject> personSenseFilter;
    public int _selectedTool{get{return selectedTool;}}
    public bool mouseOnUI;
    public static GameManager Instance{
        get{
            return singleton_instance;
        }
    }
    [SerializeField] HitCollision heatCollision;

    private void Start() {
        buildingManager = this.GetComponent<BuildingManager>();
        itemManager = this.GetComponent<ItemManager>();
        peopleManager = this.GetComponent<PeopleManager>();
        cameraManager = this.GetComponent<CameraManager>();
        timeManager = this.GetComponent<TimeManager>();

        interactableSenseFilter = delegate(GameObject gameObject){
            if(gameObject == null)
                return false;
            if(gameObject.tag != "Interactable")
                return false;
            Interactable interactable = gameObject.GetComponent<Interactable>();
            if(interactable == null)
                return false;
            return true;
        };
        personSenseFilter = delegate(GameObject gameObject){
            if(gameObject == null)
                return false;
            if(gameObject.tag != "Person")
                return false;
            PersonBehavior personBehavior = gameObject.GetComponent<PersonBehavior>();
            if(personBehavior == null)
                return false;
            if(personBehavior.personData.sleep == true)
                return false;
            return true;
        };
        sence.filter = interactableSenseFilter;

        SelectTool(0);
    }

    private void OnValidate () {
        if(presentGameTab != pastGameTab){
            ChangeGameTab(presentGameTab);
        }
    }

    public void ChangeGameTab(string gameTabName){
        switch (gameTabName){
            case "Inventory":
                ChangeGameTab((presentGameTab != GameTab.ITEM) ? GameTab.ITEM : GameTab.NORMAL);
                break;
            case "Building":
                ChangeGameTab((presentGameTab != GameTab.BUILDING) ? GameTab.BUILDING : GameTab.NORMAL);
                break;
            default:
                break;
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
        sence.filter = interactableSenseFilter;
        GameObject nearestInteractableObject = sence.FindNearest();
        if(nearestInteractableObject != null){
            interactUI.MoveUIPositionFromTransform(nearestInteractableObject.transform);
            interactUI.visible = true;
            nearestInteractable = nearestInteractableObject.GetComponent<Interactable>();
            interactUI.UpdateIcon(nearestInteractable.interactType);
        }else{
            nearestInteractable = null;
            interactUI.visible = false;
        }

        sence.filter = personSenseFilter;
        GameObject nearestPersonObject = sence.FindNearest();
        if(nearestPersonObject != null){
            personInfoUI.MoveUIPositionFromTransform(nearestPersonObject.transform);
            personInfoUI.visible = true;
            nearestPerson = nearestPersonObject.GetComponent<PersonBehavior>();
            personInfoUI.UpdateIcon(nearestPerson.personData);
        }else{
            nearestPerson = null;
            personInfoUI.visible = false;
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

            case GameTab.FORESTER_HUT:
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
        hitCollision.tool = tools[this.selectedTool].toolType;

        if(GetToolNowHold().toolType == Tool.ToolType.LANTERN){
            heatCollision.gameObject.SetActive(true);
        }else{
            heatCollision.gameObject.SetActive(false);
        }
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
}
