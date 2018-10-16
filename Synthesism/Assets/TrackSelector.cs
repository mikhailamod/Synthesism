using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrackSelector : MonoSingleton<TrackSelector> {

    public bool isMutliplayer = false;
    int trackChoice = 0;//0 is Emerald, 1 is Egypt
    public bool canChooseTrack = false;
    CarSelector carSelector;

    private void Update()
    {
        if(canChooseTrack)
        {
            if(Input.GetButtonDown(ControllerInfo.HORIZONTAL_MOVES[0]) ||
                Input.GetButtonDown(ControllerInfo.HORIZONTAL_MOVES[1]))
            {
                if(trackChoice == 0)
                {
                    trackChoice = 1;
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                }
                else if(trackChoice == 1)
                {
                    trackChoice = 0;
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
                }
            }//end if
            //confirm
            else if(Input.GetButtonUp(ControllerInfo.POWER_UPS[0]) ||
                Input.GetButtonUp(ControllerInfo.POWER_UPS[1]))
            {
                canChooseTrack = false;
                carSelector.setToActive(isMutliplayer);
            }
        }
    }

    public void setMultiplayer(bool m)
    {
        isMutliplayer = m;
    }
    public void setTrackChoice(int t)
    {
        trackChoice = t;
    }

    public void setCanChoose(bool c)
    {
        canChooseTrack = c;
    }

    public void setCarSelector(CarSelector s)
    {
        carSelector = s;
    }

    public int getTrackChoice() { return trackChoice; }
}
