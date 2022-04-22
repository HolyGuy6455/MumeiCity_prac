using UnityEngine;
using UnityEngine.UI;

public class NamingWindow : MonoBehaviour{
    [SerializeField] Animator taskUIAnimator;
    [SerializeField] InputField inputField;
    [SerializeField] HouseUI houseUI;
    public PersonData personData;
    public void SetVisible(bool value){
        taskUIAnimator.SetBool("Naming",value);
    }
    public void UpdateUI(){
        inputField.text = personData.name;
    }

    public void SaveName(){
        personData.name = inputField.text;
    }

    public void SetPersonData(int index){
        personData = houseUI._personStatusViews[index]._personData;
    }
}