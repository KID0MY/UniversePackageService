using UnityEngine;

public class tasks : ScriptableObject

{
    //prob best to create a script thats sole purpose is to track task
    public string taskName; 
    public string taskDescription;
    public string taskType;
    public bool isCompleted_ = false; 

    public void deleteTask()
    {
        //placeholder method!
    }
}
