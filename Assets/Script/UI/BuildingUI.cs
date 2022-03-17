using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingUI : MonoBehaviour{
    public List<BuildingPreset> buildingDataList;

    [SerializeField] Image[] toolButtonsSelectionImages;
    [SerializeField] GameObject buildingArticlesList;
    [SerializeField] GameObject resourceViewList;
    [SerializeField] BuildingArticle[] buildingArticles; 
    [SerializeField] ResourceView[] resourceViews;
    [SerializeField] Image infoImage;
    [SerializeField] Text infoName;
    [SerializeField] Text infoDescription;
    [SerializeField] Sprite selectedToolSprite;
    [SerializeField] Sprite otherToolSprite;
    [SerializeField] Button buildButton;
    [SerializeField] GameObject ready;
    [SerializeField] GameObject notReady;
    [SerializeField] Image toolNotReady;
    [SerializeField] Image buildingToolNeedsImage;
    [SerializeField] Text buildingToolNeedsText;
    RectTransform buildingArticlesListRect;

    // Start is called before the first frame update
    void Start(){
        GameManager.Instance.buildingManager.onToolChangedCallback += UpdateUI;
        buildingArticlesListRect = buildingArticlesList.GetComponent<RectTransform>();
        buildingArticles = buildingArticlesList.GetComponentsInChildren<BuildingArticle>();
        resourceViews = resourceViewList.GetComponentsInChildren<ResourceView>();
        UpdateUI();
        UpdateInfomation();
    }

    void UpdateUI(){
        ToolInfo toolInfo = GameManager.Instance.GetToolInfoNowHold();
        bool isUnlock = GameManager.Instance.achievementManager.isDone(toolInfo.unlockKey);
        if(!isUnlock){
            ready.SetActive(false);
            notReady.SetActive(true);
            toolNotReady.sprite = toolInfo.toolIntactSprite;
            BuildingPreset buildingPreset = BuildingManager.GetBuildingPreset(toolInfo.buildingNeeds);
            buildingToolNeedsText.text = buildingPreset.name;
            buildingToolNeedsImage.sprite = buildingPreset.sprite;
            return;
        }
        ready.SetActive(true);
        notReady.SetActive(false);
        buildingDataList = BuildingManager.GetGroupedListByBuildType(toolInfo.toolType);
        int minCount = Mathf.Max(buildingDataList.Count,5);
        int selectedTool = GameManager.Instance._selectedTool;

        for (int i = 0; i < toolButtonsSelectionImages.Length; i++){
            if(selectedTool == i){
                toolButtonsSelectionImages[i].sprite = selectedToolSprite;
            }else{
                toolButtonsSelectionImages[i].sprite = otherToolSprite;
            }
        }

        for (int i = 0; i < buildingArticles.Length; i++){
            if(i < buildingDataList.Count){
                buildingArticles[i].gameObject.SetActive(true);
                buildingArticles[i].UpdatePreset(buildingDataList[i]);
            }else{
                buildingArticles[i].gameObject.SetActive(false);
                buildingArticles[i].UpdatePreset(null);
            }
        }
        buildingArticlesListRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, minCount*20);
        buildingArticles[0].ArticleSelect();
    }

    public void UpdateInfomation(){
        BuildingPreset selectedPreset = GameManager.Instance.buildingManager._nowBuilding;
        if(selectedPreset == null){
            infoImage.sprite = GameManager.Instance.emptySprite;
            infoName.text = "";
            infoDescription.text = "";
            for (int i = 0; i < resourceViews.Length; i++){
                resourceViews[i].gameObject.SetActive(false);
                resourceViews[i].UpdateResource(null);
            }
            return;
        }
        infoImage.sprite = selectedPreset.sprite;
        infoName.text = selectedPreset.name;
        infoDescription.text = selectedPreset.info;

        List<NecessaryResource> resourceList = selectedPreset.resourceList;
        for (int i = 0; i < resourceViews.Length; i++){
            if(i < resourceList.Count){
                resourceViews[i].gameObject.SetActive(true);
                resourceViews[i].UpdateResource(resourceList[i]);
            }else{
                resourceViews[i].gameObject.SetActive(false);
                resourceViews[i].UpdateResource(null);
            }
        }
    }

    public void OnToolButtonClick(int index){
        GameManager.Instance.SelectTool(index);
        UpdateUI();
    }

    public void OnBuildButtonClick(){
        GameManager.Instance.buildingManager.BuildOnProcess();
        UpdateInfomation();
    }

    public void CheckConstructionArea(){
        if(GameManager.Instance.buildingManager.constructionArea.isThereObstacle()){
            buildButton.interactable = false;
        }else{
            buildButton.interactable = true;
        }
    }

}