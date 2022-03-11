public class SuperintendentFunction : IFacilityFunction{
    // 일하는곳 추가 정보
    public bool[] workList;

    public SuperintendentFunction(int count){
        workList = new bool[count];
    }

    public void ReloadMediocrityData(BuildingData buildingData){
        string[] splitString = buildingData.content.Split();
        workList = new bool[splitString.Length];
        for (int i = 0; i < workList.Length; i++){
            workList[i] = (splitString[i].CompareTo("t")==0);
        }
    }

    public void SaveMediocrityData(BuildingData buildingData){
        string result = "";
        for (int i = 0; i < workList.Length; i++){
            result += (workList[i])?"t ":"f ";
        }
        buildingData.content = result;
    }
}