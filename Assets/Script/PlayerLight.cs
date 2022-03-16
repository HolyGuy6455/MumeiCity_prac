using UnityEngine;


public class PlayerLight : MonoBehaviour{
    [SerializeField] GameObject player;
    [SerializeField] Color brightLight;
    [SerializeField] Color dimLight;
    [SerializeField] UnityEngine.Rendering.Universal.Light2D light2D;
    [SerializeField] float transitionSpeed = 1.0f;
    [SerializeField] float innerRadiusMax = 1.0f;
    [SerializeField] float outerRadiusMax = 5.0f;
    [SerializeField] float opacity = 0.0f;
    void Start(){
        
    }

    void Update(){
        Vector3 shadowPostion = new Vector3();
        shadowPostion.x = player.transform.position.x;
        shadowPostion.y = (player.transform.position.y+player.transform.position.z);
        this.transform.position = shadowPostion;
        
        if(GameManager.Instance.GetToolInfoNowHold().toolType == ToolType.LANTERN){
            light2D.color = Color.Lerp(brightLight,dimLight,GameManager.Instance.timeManager._blendValue);
            opacity = Mathf.Lerp(opacity,1.0f,Time.deltaTime*transitionSpeed);
            light2D.shadowIntensity = GameManager.Instance.timeManager._blendValue/2;
        }else{
            opacity = Mathf.Lerp(opacity,0.0f,Time.deltaTime*transitionSpeed);
        }
        light2D.pointLightInnerRadius = opacity*innerRadiusMax;
        light2D.pointLightOuterRadius = opacity*outerRadiusMax;
        // light2D.intensity = opacity;
    }
}
