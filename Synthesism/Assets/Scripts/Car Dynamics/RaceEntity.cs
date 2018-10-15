using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(CarController))]
public class RaceEntity : MonoBehaviour {

    public bool isAi = false;
    public int carType;//0 is slow car, 1 is fast car

    public TextMeshProUGUI lapCountLabel;
    public TextMeshProUGUI lapTimeLabel;
    public TextMeshProUGUI bestTimeLabel;

    private void Start()
    {
        RaceManager.instance.registerCar(this);
    }

    public float lapTime;
    public float bestLapTime;
    private float startLapTime;

    public void StartRace()
    {
        startLapTime = Time.time;
        if(!isAi) lapCountLabel.text = RaceManager.instance.getLap(this) + "/" + RaceManager.instance.numLaps;
        bestLapTime = float.MaxValue;
    }

    private void Update()
    {
        if(RaceManager.instance.raceStarted && !isAi)
        {
            lapTime = Time.time - startLapTime;
            lapTimeLabel.text = milliTimeToString(lapTime);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!RaceManager.instance.raceStarted)
        {
            return;
        }

        if(other.CompareTag("Checkpoint"))
        {
            int checkPointNum = other.GetComponent<Checkpoint>().checkPointNum;
            if(RaceManager.instance.checkpoint(this, checkPointNum))
            {
                Debug.Log("Lap Completed");
                if(!isAi) lapCountLabel.text = RaceManager.instance.getLap(this) + "/" + RaceManager.instance.numLaps;
                if (lapTime < bestLapTime)
                {
                    bestLapTime = lapTime;
                    if(!isAi) bestTimeLabel.text = milliTimeToString(bestLapTime);
                }
                startLapTime = Time.time;
            }
        }
    }

    private string milliTimeToString(float time)
    {
        float seconds = (int)( time % 60);
        float minutes = (int) ((time / 60) % 60);

        string minuteStr = (minutes < 10) ? "0" + minutes : minutes + "";
        string secondStr = (seconds < 10) ? "0" + seconds : seconds + "";

        return minuteStr + ":" + secondStr;
    }
}
