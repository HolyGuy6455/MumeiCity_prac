using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public delegate void UpdateUI();
    private static GameManager singleton_instance = null;
    public Transform PlayerTransform;
    [SerializeField] int selectedTool = 0;
    [SerializeField] HitCollision hitCollision;
    [SerializeField] HitCollision heatCollision;
    public SpriteRenderer ToolView;
    public Image ToolUIImage;
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
    public AchievementManager achievementManager;
    public TaskUIBundle taskUIBundle;
    [SerializeField] private Animator taskUIAnimator;
    public PlayerInput playerInput;

    [SerializeField] ToolType[] tools;
    public int _selectedTool{get{return selectedTool;}}
    [SerializeField] List<ToolInfo> toolInfos;
    public Dictionary<ToolType,ToolInfo> toolInfoDictionary;
    [SerializeField] Image toolBanished;
    
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
        GUIDE_BOOK,
        ACHIEVEMENT
    }
    public GameTab presentGameTab;
    private GameTab pastGameTab;
    public BuildingObject interactingBuilding;
    public Interactable nearestInteractable;
    public PersonBehavior nearestPerson;
    [SerializeField] InteractUI interactUI;
    [SerializeField] PersonInfoUI personInfoUI;
    [SerializeField] PauseUI pauseUI;
    public GuideBookUI guideBookUI;
    
    [SerializeField] Sence sence;
    public Sence sence_{get{return sence;}}
    Predicate<GameObject> interactableSenseFilter;
    Predicate<GameObject> personSenseFilter;
    
    public bool mouseOnUI;
    public static GameManager Instance{
        get{
            if(singleton_instance != null){
                return singleton_instance;
            }
            GameObject gameManagerObject = GameObject.FindGameObjectWithTag("GameManager");
            if(gameManagerObject != null){
                return gameManagerObject.GetComponent<GameManager>();
            }
            return null;
        }
    }

    private void Awake() {
        singleton_instance = this;

        toolInfoDictionary = new Dictionary<ToolType, ToolInfo>();
        foreach (ToolInfo info in toolInfos){
            toolInfoDictionary[info.toolType] = info;
        }
    }

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

        SelectTool(1);
    }

    private void OnValidate () {
        if(presentGameTab != pastGameTab){
            ChangeGameTab(presentGameTab);
        }
    }

    private void Update() {
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
            case "Normal":
                ChangeGameTab(GameTab.NORMAL);
                break;
            case "Building":
                ChangeGameTab(GameTab.BUILDING);
                break;
            case "Inventory":
                ChangeGameTab(GameTab.ITEM);
                break;
            case "House":
                ChangeGameTab(GameTab.HOUSE);
                break;
            case "Superintedent":
                ChangeGameTab(GameTab.SUPERINTENDENT);
                break;
            case "Manufacturer":
                ChangeGameTab(GameTab.MANUFACTURER);
                break;
            case "Laboratory":
                ChangeGameTab(GameTab.LABORATORY);
                break;
            case "GuideBook":
                ChangeGameTab(GameTab.GUIDE_BOOK);
                break;
            case "Achievement":
                ChangeGameTab(GameTab.ACHIEVEMENT);
                break;
            default:
                break;
        }
    }

    public void ChangeGameTab(GameTab gameTab){
        presentGameTab = gameTab;
        bool playerMovementEnabled = false;
        bool buildingShow = false;
        BuildingObject buildingObject = null;
        if(nearestInteractable != null){
            buildingObject = nearestInteractable.GetComponentInParent<BuildingObject>();
        }
        
        pastGameTab = presentGameTab;
        
        switch (presentGameTab)
        {
            case GameTab.NORMAL:
                playerMovementEnabled = true;
                taskUIAnimator.SetInteger("SelectedUI",0);
                break;

            case GameTab.ITEM:
                taskUIAnimator.SetInteger("SelectedUI",1);
                break;

            case GameTab.BUILDING:
                taskUIAnimator.SetInteger("SelectedUI",2);
                playerMovementEnabled = true;
                buildingShow = true;
                if(buildingManager.onToolChangedCallback != null){
                    buildingManager.onToolChangedCallback.Invoke();
                }
                break;

            case GameTab.HOUSE:
                interactingBuilding = buildingObject;
                taskUIAnimator.SetInteger("SelectedUI",4);
                break;

            case GameTab.SUPERINTENDENT:
                interactingBuilding = buildingObject;
                taskUIAnimator.SetInteger("SelectedUI",3);
                break;

            case GameTab.MANUFACTURER:
                interactingBuilding = buildingObject;
                taskUIAnimator.SetInteger("SelectedUI",5);
                break;

            case GameTab.LABORATORY:
                interactingBuilding = buildingObject;
                taskUIAnimator.SetInteger("SelectedUI",6);
                break;

            case GameTab.GUIDE_BOOK:
                taskUIAnimator.SetInteger("SelectedUI",7);
                break;
            
            case GameTab.ACHIEVEMENT:
                taskUIAnimator.SetInteger("SelectedUI",8);
                break;

            default:
                break;
        }

        playerMovement.stop = !playerMovementEnabled;
        buildingManager.constructionArea.show = buildingShow;
        if(!buildingShow){
            buildingManager.constructionArea.SetBuildingData(null);
        }
    }

    public ToolInfo GetToolInfoNowHold(){
        return toolInfoDictionary[tools[this.selectedTool]];
    }

    public void SelectTool(int index){
        if(index < 0 || index > tools.Length -1){
            Debug.Log("Wrong Tool Index!");
            return;
        }
        this.selectedTool = index;
        ToolInfo toolNowHold = GetToolInfoNowHold();
        ToolUIImage.sprite = toolInfos[index].toolWhiteSprite;
        if(toolNowHold.isItEnable()){
            ToolView.sprite = toolInfos[index].toolSprite;
            hitCollision.tool = tools[this.selectedTool];
            toolBanished.enabled = false;
        }else{
            ToolView.sprite = emptySprite;
            hitCollision.tool = ToolType.NONE;
            toolBanished.enabled = true;
        }
        

        if(toolNowHold.toolType == ToolType.LANTERN){
            heatCollision.gameObject.SetActive(true);
        }else{
            heatCollision.gameObject.SetActive(false);
        }
        if(buildingManager.onToolChangedCallback != null){
            buildingManager.onToolChangedCallback.Invoke();
        }
    }

    public void OnNextTool(InputAction.CallbackContext value){
        if(!value.performed){
            return;
        }
        OnNextTool();
    }

    public void OnNextTool(){
        if(selectedTool > tools.Length -2){
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
        OnBeforeTool();
    }

    public void OnBeforeTool(){
        if(selectedTool < 1){
            SelectTool(tools.Length-1);
        }else{
            SelectTool(selectedTool-1);
        }
        if(buildingManager.onToolChangedCallback != null){
            buildingManager.onToolChangedCallback.Invoke();
        }
    }
    
    public void OnInteract(InputAction.CallbackContext value){
        if(value.performed){
            Interactable interactable = GameManager.Instance.nearestInteractable;
            if(interactable != null){
                interactable.Interact();
            }
        }
    }

    public void OnInventory(InputAction.CallbackContext value){
        if(!value.performed){
            return;
        }
        ChangeGameTab(GameTab.ITEM);
    }

    public void OnBuliding(InputAction.CallbackContext value){
        if(!value.performed){
            return;
        }
        if(presentGameTab == GameTab.BUILDING){
            ChangeGameTab(GameTab.NORMAL);
        }else{
            ChangeGameTab(GameTab.BUILDING);
        }
    }

    public void OnMenu(InputAction.CallbackContext value){
        if(value.performed){
            if(presentGameTab != GameTab.NORMAL){
                OnExitTask(value);
            }else{
                pauseUI.Pause();
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

    public void OnAchievement(InputAction.CallbackContext value){
        if(value.started)
            Debug.Log("OnAchievement started");
        if(value.performed)
            Debug.Log("OnAchievement performed");
        if(!value.performed){
            return;
        }
        ChangeGameTab(GameTab.ACHIEVEMENT);
    }

    public void OnExitTask(InputAction.CallbackContext value){
        if(!value.performed){
            return;
        }
        ChangeGameTab(GameTab.NORMAL);
    }

    public void SwitchActionMap(string actionMap){
        playerInput.SwitchCurrentActionMap(actionMap);
    }

    public void QuitGame(){
        Application.Quit();
        // UnityEditor.EditorApplication.isPlaying = false;
    }

    public void SetResolution(string resolution){
        switch (resolution){
            case "640_360":
                Screen.SetResolution(640, 360, false);
                break;
            case "1024_576":
                Screen.SetResolution(1024, 576, false);
                break;
            case "1600_900":
                Screen.SetResolution(1600, 900, false);
                break;
            case "1920_1080":
                Screen.SetResolution(1920, 1080, false);
                break;
            default:
                break;
        }
    }

}


[Serializable]
public class ToolInfo{
    public string name;
    public ToolType toolType;
    public Sprite toolSprite;
    public Sprite toolIntactSprite;
    public Sprite toolWhiteSprite;
    public string unlockKey;
    public string buildingNeeds;
    public bool isItEnable(){
        return GameManager.Instance.achievementManager.isDone(unlockKey);
    }
}