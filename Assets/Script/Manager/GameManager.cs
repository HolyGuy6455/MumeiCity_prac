using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public delegate void UpdateUI();
    private static GameManager singleton_instance = null;
    public Transform PlayerTransform;
    public List<Tool> tools = new List<Tool>();
    [SerializeField] int selectedTool = 0;
    [SerializeField] HitCollision hitCollision;
    public SpriteRenderer ToolView;
    public PlayerMovement playerMovement;
    public Inventory inventory;
    public BuildingManager buildingManager;
    public ItemManager itemManager;
    public PeopleManager peopleManager;
    public CameraManager cameraManager;
    public TimeManager timeManager;
    public SaveLoadManager saveLoadManager;
    public MobManager mobManager;
    public GridMapManager gridMapManager;
    [SerializeField] private Animator taskUI;

    [SerializeField] PlayerInput playerInput;
    
    public Animator pauseAnimator;
    bool gameIsPause = false;
    
    public Sprite emptySprite;
    public enum GameTab
    {
        NORMAL,
        ITEM,
        BUILDING,
        HOUSE,
        SUPERINTENDENT,
        MANUFACTURER,
        LABORATORY,
        GUIDE_BOOK

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
            return GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            // Debug.Log("singleton_instance - " + singleton_instance);
            // return singleton_instance;
        }
    }
    [SerializeField] HitCollision heatCollision;
    

    private void Start() {
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

    private void Update() {
        // if(Input.GetButtonDown("Building")){
        //     ChangeGameTab((presentGameTab != GameTab.BUILDING) ? GameTab.BUILDING : GameTab.NORMAL);
        // }
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

    public void ChangeGameTab(string gameTab){
        switch (gameTab){
            case "Building":
                ChangeGameTab(GameTab.BUILDING);
                break;
            case "Inventory":
                ChangeGameTab(GameTab.ITEM);
                break;
            default:
                break;
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

            case GameTab.HOUSE:
                taskUI.SetInteger("SelectedUI",4);
                break;

            case GameTab.SUPERINTENDENT:
                taskUI.SetInteger("SelectedUI",3);
                break;

            case GameTab.MANUFACTURER:
                taskUI.SetInteger("SelectedUI",5);
                break;

            case GameTab.LABORATORY:
                taskUI.SetInteger("SelectedUI",6);
                break;

            case GameTab.GUIDE_BOOK:
                taskUI.SetInteger("SelectedUI",7);
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

    public void OnNextTool(InputAction.CallbackContext value){
        if(!value.performed){
            return;
        }
        if(selectedTool > tools.ToArray().Length -2){
            SelectTool(0);
        }else{
            SelectTool(selectedTool+1);
        }
        if(buildingManager.onToolChangedCallback != null){
            buildingManager.onToolChangedCallback.Invoke();
        }
    }

    public void OnBeforeTool(InputAction.CallbackContext value){
        if(!value.performed){
            return;
        }
        if(selectedTool < 1){
            SelectTool(tools.ToArray().Length-1);
        }else{
            SelectTool(selectedTool-1);
        }
        if(buildingManager.onToolChangedCallback != null){
            buildingManager.onToolChangedCallback.Invoke();
        }
    }
    
    public void OnInteract(InputAction.CallbackContext value){
        if(value.performed){
            if(presentGameTab == GameTab.BUILDING){
                GameManager.Instance.buildingManager.Build();
            }else{
                Interactable interactable = GameManager.Instance.nearestInteractable;
                if(interactable != null){
                    interactable.Interact();
                }
            }
        }
    }

    public void OnInventory(InputAction.CallbackContext value){
        if(value.started)
            Debug.Log("OnInventory started");
        if(value.performed)
            Debug.Log("OnInventory performed");
        if(!value.performed){
            return;
        }
        ChangeGameTab(GameTab.ITEM);
    }

    public void OnBuliding(InputAction.CallbackContext value){
        if(!value.performed){
            return;
        }
        Debug.Log("presentGameTab - " + presentGameTab);
        ChangeGameTab((presentGameTab != GameTab.BUILDING) ? GameTab.BUILDING : GameTab.NORMAL);
    }

    public void OnMenu(InputAction.CallbackContext value){
        if(value.performed){
            if(presentGameTab != GameTab.NORMAL){
                OnExitTask(value);
            }else{
                gameIsPause = true;
                pauseAnimator.SetBool("isVisible",true);
                Time.timeScale = 0;
                playerMovement.enabled = false;
                playerInput.SwitchCurrentActionMap("Pause");
            }
        }
    }

    public void OnGuide(InputAction.CallbackContext value){
        if(value.started)
            Debug.Log("OnGuide started");
        if(value.performed)
            Debug.Log("OnGuide performed");
        if(!value.performed){
            return;
        }
        ChangeGameTab(GameTab.GUIDE_BOOK);
    }

    public void OnExitTask(InputAction.CallbackContext value){
        if(!value.performed){
            return;
        }
        ChangeGameTab(GameTab.NORMAL);
    }

    public void OnExitPause(InputAction.CallbackContext value){
        if(value.performed){
            gameIsPause = false;
            pauseAnimator.SetBool("isVisible",false);
            Time.timeScale = 1;
            playerMovement.enabled = true;
            playerInput.SwitchCurrentActionMap("PlayerControl");
        }
    }


    private void Awake() {
        singleton_instance = this;
    }

    public void SwitchActionMap(string actionMap){
        playerInput.SwitchCurrentActionMap(actionMap);
    }

    public void QuitGame(){
        Application.Quit();
    }
}
