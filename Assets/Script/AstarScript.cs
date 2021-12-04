using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class AstarScript : MonoBehaviour
{
    public AstarPath astarPath;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P)){
            astarPath.Scan();
            Time.timeScale = (Time.timeScale == 0)? 1:0;
        }
    }
}
