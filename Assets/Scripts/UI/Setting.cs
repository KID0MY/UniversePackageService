using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using UnityEngine.UIElements;
using UnityEngine.UI;
using TMPro;

public class setting_ : MonoBehaviour
{
 //   [Header("Sens Settings")]
    public CharacterCamera playerCam;
    public QuestManager questManager;
    public Slider sensSlider;
    public float newMouseSens; 
    public float oldMouseSens;

    //[Header("Resolution Settings")]
    public TMP_Dropdown resoDropList;
    public Toggle screenToggle;

    Resolution[] resolutions_;
    bool isFullScreened;
    int currentreso;
    List<Resolution> selectResoList = new List<Resolution>();





    void  Start()
    {
        questManager = GameObject.Find("QuestManager").GetComponent<QuestManager>(); 
        sensSlider.onValueChanged.AddListener(changeSens);
        oldMouseSens = questManager.currentSensitivity;
        sensSlider.value = oldMouseSens;
        if (playerCam != null)
        {
            playerCam.lookSensitivity = oldMouseSens;
        }
        questManager.currentSensitivity = sensSlider.value;
        isFullScreened = true;

        //setting up possible resolutions 
        resolutions_ = Screen.resolutions;
        List<string> resolutionList = new List<string>();
        string newRes; 
        foreach(Resolution res in resolutions_)
        {
            newRes= res.width.ToString() + "x" + res.height.ToString();
            if (!resolutionList.Contains(newRes))
            {
                resolutionList.Add(newRes);
                selectResoList.Add(res);
               // resolutionList.Add(res.ToString());
            }
        }
        resoDropList.AddOptions(resolutionList);
    }


    public void changeSens(float newVal)
    {
        // oldMouseSens = playerCam.lookSensitivity;
        questManager.currentSensitivity = newVal;
        Debug.Log("Current sens value is " + playerCam.lookSensitivity);
        if (playerCam != null)
        {
            playerCam.lookSensitivity = questManager.currentSensitivity;
        }
    }
    public void changeReso()
    {
        currentreso = resoDropList.value;
        Screen.SetResolution(selectResoList[currentreso].width, selectResoList[currentreso].height,isFullScreened);
    }
    public void fullScreenView()
    {
        isFullScreened = screenToggle.isOn;
        Screen.SetResolution(selectResoList[currentreso].width, selectResoList[currentreso].height, isFullScreened);


    }
}
