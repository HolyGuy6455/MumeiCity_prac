using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

/*
 * 인게임에서 보이는 건물의 GameObject
 */
public class BuildingObject : MonoBehaviour
{
    public BuildingData buildingData;
    public SpriteRenderer spriteRenderer;
    public GameObject spriteObject;
    public GameObject shadowObject;
    [SerializeField] TimeEventQueueTicket hiringEvent;
    [SerializeField] TimeEventQueueTicket growupEvent;
    [SerializeField] Animator animator;
 
    private void Start() {
        // TODO
        // 내가 에디터에서 만들때와 BuildingManager에서 만들때
        // Initilize()의 실행 시점이 달라야하는데 그렇지않아 문제가 생긴다
        // 이 부분은 조금 더 고민이 필요해 보인다. 일단 지금은 덮어두기식으로 간단하게 넘어간다

        buildingData.positionX = ((int)Mathf.Round(this.transform.position.x));
        buildingData.positionY = ((int)Mathf.Round(this.transform.position.y));
        buildingData.positionZ = ((int)Mathf.Round(this.transform.position.z));

        switch (buildingData.buildingPreset.name){
            case "ForesterHut":
                buildingData.mediocrityData = new SuperintendentData();
                SuperintendentData superintendentData = buildingData.mediocrityData as SuperintendentData;
                superintendentData.workList = new bool[buildingData.buildingPreset.taskPresets.Count];
                break;
            case "Tent":
                buildingData.mediocrityData = new HouseData(12);
                break;
            default:
                break;
        }

        Initialize(this.buildingData);
        HirePerson();
        if(buildingData.buildingPreset.workplace){
            string ticketName = "building"+this.buildingData.id+"_hire";
            hiringEvent = GameManager.Instance.timeManager.AddTimeEventQueueTicket(1, ticketName, true, HirePerson);
        }
        Vector3 shadowPostion = new Vector3();
        shadowPostion.x = this.transform.position.x;
        shadowPostion.y = (this.transform.position.y+this.transform.position.z);
        shadowObject.transform.position = shadowPostion;

        if(buildingData.buildingPreset.growUpTerm != 0){
            int term = buildingData.buildingPreset.growUpTerm;
            string ticketName = "building"+this.GetInstanceID()+"_growup";
            growupEvent = GameManager.Instance.timeManager.AddTimeEventQueueTicket(term,ticketName,false, SwapPresetToGrow);
        }
    }

    private void HirePerson(){
        if(buildingData.buildingPreset.workplace && buildingData.workerID == 0){
            List<PersonBehavior> people = PeopleManager.GetWholePeopleList();
            people = people.FindAll(person=> (person != null)&&((person as PersonBehavior).personData.workplaceID == 0) );
            if(people.Count > 0){
                people[0].personData.workplaceID = this.buildingData.id;
                this.buildingData.workerID = people[0].personData.id;
            }
        }
        buildingData.mediocrityData.SaveMediocrityData();
    }

    // 오브젝트의 초기화
    public void Initialize(BuildingData buildingData){
        BuildingPreset buildingPreset = buildingData.buildingPreset;
        this.buildingData = buildingData;
        this.transform.localScale = buildingPreset.scale;
        spriteRenderer.sprite = buildingPreset.sprite;
        if( !buildingData.buildingPreset.interactable ){
            Interactable interactable = this.GetComponentInChildren<Interactable>();
            if(interactable != null){
                interactable.gameObject.SetActive(false);
            }
        }

        if(buildingPreset.dropAmounts.Count > 0){
            this.gameObject.AddComponent<ItemDroper>().InitializeItemDrop(buildingPreset.dropAmounts);
        }

        Hittable hittable = GetComponent<Hittable>();
        hittable.DeadEventHandler += 
            delegate(Hittable component){
                GameManager.Instance.buildingManager.astarPath.Scan();
            };
        // hittable.HitEventHandler += CheckHP;
        hittable.SetEffectiveTool(buildingPreset.removalTool);
        hittable.HP = buildingPreset.healthPointMax;
        switch (buildingPreset.brokenStyle){
            case BuildingPreset.BrokenStyle.NONE:
                animator.SetInteger("BrokenStyle",0);
                break;
            case BuildingPreset.BrokenStyle.FALL_DOWN:
                animator.SetInteger("BrokenStyle",1);
                break;
            case BuildingPreset.BrokenStyle.COLLAPSE:
                animator.SetInteger("BrokenStyle",2);
                break;
            case BuildingPreset.BrokenStyle.SWAP:
                animator.SetInteger("BrokenStyle",3);
                break;
            default:
                break;
        }

        Light2D light = GetComponentInChildren<Light2D>();
        light.pointLightOuterRadius = buildingPreset.lightSourceIntensity;
    }

    public void ShowWindow(){
        GameManager.Instance.interactingBuilding = this.buildingData;
        GameManager.GameTab gameTab = buildingData.buildingPreset.gameTab;
        GameManager.Instance.ChangeGameTab(gameTab);
    }

    public void SwapPresetToRuin(){
        buildingData.code = buildingData.buildingPreset.ruinPreset.code;
        Initialize(buildingData);
        animator.SetBool("isDead",false);
    }

    public void SwapPresetToGrow(){
        buildingData.code = buildingData.buildingPreset.grownPreset.code;
        Initialize(buildingData);
    }

    [ContextMenu("Do Something")]
    void DoSomething()
    {
        Initialize(this.buildingData);
    }

    // public void CheckHP(Hittable hittable){
    //     if(buildingData.buildingPreset.spriteBroken == null){
    //         return;
    //     }
    //     float percentage = (float)hittable.HP / (float)buildingData.buildingPreset.healthPointMax;
    //     if( percentage < 0.5f ){
    //         spriteRenderer.sprite = buildingData.buildingPreset.spriteBroken;
    //     }else{
    //         spriteRenderer.sprite = buildingData.buildingPreset.sprite;
    //     }
    // }

}
